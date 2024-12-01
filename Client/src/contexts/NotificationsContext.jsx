import React, { createContext, useCallback, useEffect, useRef, useState } from "react"
import { useDispatch, useSelector } from "react-redux"
import { setUnreadCount } from "../redux/notificationsSlice"
import { useLocation } from "react-router-dom"
import * as signalR from '@microsoft/signalr'
import { useError } from "./ErrorContext"

export const NotificationsContext = createContext()

/* eslint-disable react/prop-types */
export const NotificationsProvider = ({ children }) => {
  const [notifications, setNotifications] = useState([])
  const [hasNewNotifications, setHasNewNotifications] = useState(false)

  const { currentUser } = useSelector((state) => state.user)

  const location = useLocation()

  const dispatch = useDispatch()

  let connection = useRef(null)

  const { showError } = useError()

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
          showError("Token not found")
          return
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
          const errorText = await response.text()
          const errorData = JSON.parse(errorText)

          if (Array.isArray(errorData.errors)) {
            errorData.errors.forEach((err) => {
              showError(err.ErrorMessage)
            })
          } else {
            const errorMessage = errorData.message || "An unknown error occurred.";
            showError(errorMessage)
          }
  
          return
        }
      } catch (error) {
        showError(error)
      }
    }

    fetchNotifications()
  }, [currentUser, dispatch, updateUnreadCount, showError])

  useEffect(() => {
    const startSignalRConnection = async () => {
      if (connection.current) {
        await connection.current.stop()
        connection.current = null
      }

      try {
        const token = localStorage.getItem("token")
        if (!token) {
          showError("Token not found")
          return
        }

        const hubConnection = new signalR.HubConnectionBuilder()
          .withUrl("/notificationsHub", {
            accessTokenFactory: () => token,
          })
          .configureLogging(signalR.LogLevel.Information)
          .withAutomaticReconnect([0, 2000, 10000, 30000])
          .build()

        hubConnection.onreconnecting(error => {
          console.log("Reconnecting due to error:", error)
        })

        hubConnection.onreconnected(connectionId => {
          console.log("Reconnected. New connection ID:", connectionId)
        })

        hubConnection.on("ReceiveNotification", (notification) => {
          console.log("Notification received:", notification)
          setNotifications((prev) => [notification, ...prev])
          updateUnreadCount([...notifications, notification])
        })

        await hubConnection.start()
        console.log("SignalR connected")

        await hubConnection.invoke("JoinGroup", currentUser.id.toString())

        connection.current = hubConnection
      } catch (error) {
        showError(error)
      }
    }

    if (currentUser?.roleName === 'Author' && currentUser?.id) {
      startSignalRConnection()
    }

    return () => {
      connection.current?.stop() 
    }
  }, [currentUser, updateUnreadCount, notifications, showError])

  useEffect(() => {
    const markAllAsReadOnPageChange = async () => {
      if (!currentUser || !currentUser.id) {
        return
      }

      setNotifications((prevNotifications) => prevNotifications.map((notification) => ({ ...notification, isRead: true })))

      try {
        const token = localStorage.getItem("token")
        if (!token) {
          showError("Token not found")
          return
        }

        await fetch(`/api/Notifications/mark-all-as-read`, {
          method: "PATCH",
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        })

        setHasNewNotifications(false)
        dispatch(setUnreadCount(0))
      } catch (error) {
        showError(error)
      }
    }

    markAllAsReadOnPageChange()
  }, [location, dispatch, currentUser, showError])

  return (
    <NotificationsContext.Provider value={{ notifications, setNotifications, hasNewNotifications }}>
      {children}
    </NotificationsContext.Provider>
  )
}