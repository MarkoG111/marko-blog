import React, { createContext, useEffect, useState } from "react"
import { useSelector } from "react-redux"

export const NotificationsContext = createContext()

export const NotificationsProvider = ({ children }) => {
  const [notifications, setNotifications] = useState([])
  const [hasNewNotifications, setHasNewNotifications] = useState(false)
  const { currentUser } = useSelector((state) => state.user)

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
          
          const unreadCount = data.items.filter((n) => !n.isRead).length

          setHasNewNotifications(unreadCount > 0)
        }
      } catch (error) {
        console.log(error)
      }
    }

      fetchNotifications()
  }, [currentUser])

  return (
    <NotificationsContext.Provider value={{ notifications, setNotifications, hasNewNotifications, setHasNewNotifications }}>
      {children}
    </NotificationsContext.Provider>
  )
}