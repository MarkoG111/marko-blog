import React, { createContext, useEffect, useState } from "react"
import { useDispatch, useSelector } from "react-redux"
import { setUnreadCount } from "../redux/notificationsSlice";

export const NotificationsContext = createContext()

export const NotificationsProvider = ({ children }) => {
  const [notifications, setNotifications] = useState([])
  const [hasNewNotifications, setHasNewNotifications] = useState(false)
  const { currentUser } = useSelector((state) => state.user)

  const dispatch = useDispatch()

  useEffect(() => {
    const fetchNotifications = async () => {
      if (!currentUser?.id) return

      try {
        const token = localStorage.getItem("token")
        if (!token) {
          throw new Error("Token not found")
        }

        const response = await fetch(`/api/Notifications`, {
          method: "GET",
          headers: {
            "Authorization": `Bearer ${token}`
          },
          params: {
            idUser: currentUser.id
          }
        })

        if (response.ok) {
          const data = await response.json()
          setNotifications(data.items)

          const newUnreadCount = data.items.filter((n) => !n.isRead).length
          setHasNewNotifications(newUnreadCount > 0)

          dispatch(setUnreadCount(newUnreadCount))
        }
      } catch (error) {
        console.log(error)
      }
    }

    fetchNotifications()

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
      }
    } catch (error) {
      console.log(error)
    }
  }

  return (
    <NotificationsContext.Provider value={{ notifications, setNotifications, hasNewNotifications, setHasNewNotifications, markAllAsRead }}>
      {children}
    </NotificationsContext.Provider>
  )
}