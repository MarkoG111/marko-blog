import { useContext, useEffect, useMemo, useState } from "react"
import { useNavigate } from "react-router"
import { NotificationsContext } from "../contexts/NotificationsContext"
import { timeAgo, hoverActualDate } from "../utils/timeAgo"

export default function Notifications() {
  const [type, setType] = useState(null)

  const { notifications, setNotifications, hasNewNotifications } = useContext(NotificationsContext)

  let navigate = useNavigate()

  const handleTypeChange = (newType) => {
    setType(newType)
  }

  useEffect(() => {
    if (hasNewNotifications) {
      setNotifications((prevNotifications) =>
        prevNotifications.map((notification) =>
          !notification.isRead && notification.isNew !== true
            ? { ...notification, isNew: true }
            : notification
        )
      )
    }
  }, [hasNewNotifications, setNotifications])

  const sortedNotifications = useMemo(() => [...notifications].sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt)), [notifications])

  const filteredNotifications = type ? sortedNotifications.filter((notification) => notification.type === type) : sortedNotifications

  const handleNotificationClick = async (notification) => {
    if (!notification.isRead) {
      setNotifications((prevNotifications) =>
        prevNotifications.map((n) =>
          n.id === notification.id ? { ...n, isRead: true } : n
        )
      )
    }

    if (notification.link) {
      navigate(notification.link)
    }
  }

  return (
    <div className="flex h-screen bg-gray-100 dark:bg-gray-800 px-96">
      <div className="flex flex-col h-full p-4 md:w-1/4 bg-white dark:bg-gray-800 shadow-lg border-r border-gray-300 dark:border-gray-700">
        <h1 className="text-2xl font-bold mb-4 mt-1">Notifications</h1>
        <ul className="space-y-2">
          <li onClick={() => handleTypeChange(null)} className={`cursor-pointer hover:bg-gray-200 dark:hover:bg-indigo-600 hover:rounded-md p-2 ${type === null ? 'bg-gray-100 dark:bg-indigo-700 text-gray-600 dark:text-blue-300 rounded-md' : ''}`}
          >All</li>
          <li onClick={() => handleTypeChange(1)} className={`cursor-pointer hover:bg-gray-200 dark:hover:bg-indigo-600 hover:rounded-md p-2 ${type === 1 ? 'bg-gray-100 dark:bg-indigo-700 text-gray-600 dark:text-blue-300 rounded-md' : ''}`}
          >Posts</li>
          <li onClick={() => handleTypeChange(2)} className={`cursor-pointer hover:bg-gray-200 dark:hover:bg-indigo-600 hover:rounded-md p-2 ${type === 2 ? 'bg-gray-100 dark:bg-indigo-700 text-gray-600 dark:text-blue-300 rounded-md' : ''}`}
          >Comments</li>
          <li onClick={() => handleTypeChange(3)} className={`cursor-pointer hover:bg-gray-200 dark:hover:bg-indigo-600 hover:rounded-md p-2 ${type === 3 ? 'bg-gray-100 dark:bg-indigo-700 text-gray-600 dark:text-blue-300 rounded-md' : ''}`}
          >Likes</li>
        </ul>
      </div>

      {/* Notifications container without overflow control */}
      <div className="notifications-container flex-1 h-full pr-6 pl-6 bg-white dark:bg-gray-800 overflow-y-auto">
        <div className="space-y-4 mt-4">
          {filteredNotifications.length == 0 ? (
            <p>No notifications...</p>
          ) : (
            filteredNotifications.map((notification) => (
              <div key={notification.id}
                className={`p-4 border-b rounded-lg shadow-sm hover:bg-gray-50 dark:hover:bg-gray-700 cursor-pointer
                ${notification.isNew ? "border-blue-500 dark:border-blue-500" : "border-gray-300 dark:border-gray-400"}
              `}
                onClick={() => handleNotificationClick(notification)}
              >
                <p className="text-gray-700 dark:text-white">{notification.content}</p>
                <p className="text-sm text-gray-500" title={hoverActualDate(notification.createdAt)}>{timeAgo(notification.createdAt)}</p>
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  )
}
