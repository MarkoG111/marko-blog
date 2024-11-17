import React, { createContext, useCallback, useEffect, useRef, useState } from "react"
import { useDispatch, useSelector } from "react-redux"
import { setUnreadCount } from "../redux/notificationsSlice"
import * as signalR from '@microsoft/signalr'

export const NotificationsContext = createContext()

/* eslint-disable react/prop-types */
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
      if (connection.current) {
        await connection.current.stop()
        connection.current = null
      }

      try {
        const token = localStorage.getItem("token")
        if (!token) {
          throw new Error("Token not found")
        }

        const hubConnection = new signalR.HubConnectionBuilder()
          .withUrl("/notificationsHub", {
            accessTokenFactory: () => token,
          })
          .configureLogging(signalR.LogLevel.Information)  // Enable detailed logs
          .withAutomaticReconnect([0, 2000, 10000, 30000])
          .build()

        hubConnection.onreconnecting(error => {
          console.log("Reconnecting due to error:", error)
        })

        hubConnection.onreconnected(connectionId => {
          console.log("Reconnected. New connection ID:", connectionId)
        })

        hubConnection.on("ReceiveNotification", (notification) => {
          console.log("Notification received:", notification);
          setNotifications((prev) => [notification, ...prev]);
          updateUnreadCount([...notifications, notification]); // Ensure count updates
        })

        await hubConnection.start()
        console.log("SignalR connected")

        await hubConnection.invoke("JoinGroup", currentUser.id.toString())

        connection.current = hubConnection
      } catch (error) {
        console.error("Connection failed", error)
      }
    }

    if (currentUser?.roleName === 'Author' && currentUser?.id) {
      startSignalRConnection()
    }

    return () => {
      connection.current?.stop() // Clean up the connection on unmount
    }
  }, [currentUser, updateUnreadCount])


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