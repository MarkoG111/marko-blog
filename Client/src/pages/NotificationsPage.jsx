import { useContext, useEffect, useState } from "react"
import { NotificationsContext } from "../contexts/NotificationsContext"
import { timeAgo, hoverActualDate } from "../utils/timeAgo"

export default function Notifications() {
  const [type, setType] = useState(null)
  const [firstLoad, setFirstLoad] = useState(true)

  const { notifications, setNotifications, hasNewNotifications } = useContext(NotificationsContext)

  const handleTypeChange = (newType) => {
    setType(newType)
  }

  useEffect(() => {
    if (hasNewNotifications) {
      setNotifications((prevNotifications) =>
        prevNotifications.map((notification) => ({ ...notification, isNew: true })))
    }
  }, [hasNewNotifications, setNotifications])

  useEffect(() => {
    if (firstLoad) {
      setNotifications((prevNotifications) =>
        prevNotifications.map((notification) => ({ ...notification, isNew: true }))
      )
      setFirstLoad(false)
    } else {
      setNotifications((prevNotifications) =>
        prevNotifications.map((notification) => ({ ...notification, isNew: false }))
      )
    }
  }, [firstLoad, setNotifications])

  const sortedNotifications = [...notifications].sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt))

  return (
    <div className="flex h-screen bg-gray-100 dark:bg-gray-800 px-96">
      <div className="flex flex-col h-full p-4 md:w-1/4 bg-white dark:bg-gray-800 shadow-lg border-r border-gray-300 dark:border-gray-700">
        <h1 className="text-2xl font-bold mb-4 mt-1">Notifications</h1>
        <ul className="space-y-2">
          <li onClick={() => handleTypeChange(null)} className="cursor-pointer hover:bg-gray-200 dark:hover:bg-indigo-600 hover:rounded-md p-2">All</li>
          <li onClick={() => handleTypeChange(1)} className="cursor-pointer hover:bg-gray-200 dark:hover:bg-indigo-600 hover:rounded-md p-2">Posts</li>
          <li onClick={() => handleTypeChange(2)} className="cursor-pointer hover:bg-gray-200 dark:hover:bg-indigo-600 hover:rounded-md p-2">Comments</li>
          <li onClick={() => handleTypeChange(3)} className="cursor-pointer hover:bg-gray-200 dark:hover:bg-indigo-600 hover:rounded-md p-2">Likes</li>
        </ul>
      </div>

      {/* Notifications container without overflow control */}
      <div className="notifications-container flex-1 h-full pr-6 pl-6 bg-white dark:bg-gray-800 overflow-y-auto">
        <div className="space-y-4 mt-4">
          {notifications.length == 0 ? (
            <p>Loading notifications...</p>
          ) : (
            sortedNotifications.filter(notification => type ? notification.type === type : true).map((notification, index) => (
              <div key={notification.id || index} className={`p-4 border-b rounded-lg shadow-sm hover:bg-gray-50 dark:hover:bg-gray-700 ${notification.isRead ? "border-gray-300 dark:border-gray-400" : "border-blue-500 dark:border-blue-500"} ${notification.isNew ? "border-blue-500 dark:border-blue-500" : ""}`}>
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
