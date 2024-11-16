import React, { createContext, useCallback, useEffect, useRef, useState } from "react"
import { useDispatch, useSelector } from "react-redux"
import { setUnreadCount } from "../redux/notificationsSlice"
import * as signalR from '@microsoft/signalr'

export const NotificationsContext = createContext()

export const NotificationsProvider = ({ children }) => {
  const [notifications, setNotifications] = useState([])
  const [hasNewNotifications, setHasNewNotifications] = useState(false)
  const { currentUser } = useSelector((state) => state.user)

  const dispatch = useDispatch()

  let connection = useRef(null)

  // Use useCallback to memoize updateUnreadCount
  const updateUnreadCount = useCallback((notifications) => {
    const newUnreadCount = notifications.filter((n) => !n.isRead).length
    setHasNewNotifications(newUnreadCount > 0)
    dispatch(setUnreadCount(newUnreadCount))
  }, [dispatch])

  useEffect(() => {
    const fetchNotifications = async () => {
      if (!currentUser?.id) return

      try {
        const token = localStorage.getItem("token")
        if (!token) {
          throw new Error("Token not found")
        }

        const response = await fetch(`/api/Notifications?idUser=${currentUser.id}`, {
          method: "GET",
          headers: {
            "Authorization": `Bearer ${token}`
          }
        })

        if (response.ok) {
          const data = await response.json()
          setNotifications(data.items)
          updateUnreadCount(data.items)
        } else {
          throw new Error("Failed to fetch notifications")
        }
      } catch (error) {
        console.log(error)
      }
    }

    fetchNotifications()

  }, [currentUser, dispatch])

  useEffect(() => {
    const startSignalRConnection = async () => {
      // Check if the connection already exists and is connected
      if (connection.current && connection.current.state === "Connected") {
        return // Avoid creating multiple connections
      }

      try {
        const token = localStorage.getItem("token")
        if (!token) {
          throw new Error("Token not found")
        }

        connection.current = new signalR.HubConnectionBuilder()
          .withUrl("/notificationsHub", {
            accessTokenFactory: () => token,
          })
          .configureLogging(signalR.LogLevel.Information)  // Enable detailed logs
          .withAutomaticReconnect([0, 2000, 10000, 30000])
          .build()

        connection.current.onreconnecting(error => {
          console.log("Reconnecting due to error:", error)
        })

        connection.current.onreconnected(connectionId => {
          console.log("Reconnected. New connection ID:", connectionId)
        })

        connection.current.on("ReceiveNotification", notification => {
          console.log("Notification received:", notification);
          dispatch(setUnreadCount(prevCount => prevCount + 1))
          setNotifications(prev => [{ ...notification }, ...prev]);

          setHasNewNotifications(true)
        })

        connection.current.on("NotificationsMarkedAsRead", () => {
          setNotifications(prev =>
            prev.map(notification => ({ ...notification, isRead: true }))
          )
          dispatch(setUnreadCount(0))
        })

        await connection.current.start()
        console.log("SignalR connected globally")

        connection.current.invoke("JoinGroup", currentUser.id.toString())
          .catch(err => {
            console.error("Error joining group:", err);
            console.error(err.stack);
          });
      } catch (error) {
        console.error("Connection failed", error)
      }
    }

    if (currentUser?.roleName === 'Author' && connection.current == null) {
      startSignalRConnection()
    }

    return () => {
      connection.current?.stop() // Clean up the connection on unmount
      connection.current = null
    }
  }, [currentUser, dispatch])


  const markAllAsRead = async () => {
    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const response = await fetch(`/api/Notifications/mark-all-as-read`, {
        method: "PATCH",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json",
        }
      })

      if (response.ok) {
        setNotifications((prevNotifications) => prevNotifications.map((notification) => ({ ...notification, isRead: true })))

        setHasNewNotifications(false)
        dispatch(setUnreadCount(0))
      } else {
        throw new Error("Failed to mark notifications as read")
      }
    } catch (error) {
      console.log(error)
    }
  }

  return (
    <NotificationsContext.Provider value={{ notifications, setNotifications, hasNewNotifications, markAllAsRead }}>
      {children}
    </NotificationsContext.Provider>
  )
}