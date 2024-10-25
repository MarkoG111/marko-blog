import { useEffect, useState } from "react"
import * as signalR from '@microsoft/signalr'
import { useDispatch, useSelector } from "react-redux";
import { setUnreadCount } from "../redux/notificationsSlice";

export default function Notifications() {
  const [notifications, setNotifications] = useState([])
  const [hasNewNotifications, setHasNewNotifications] = useState(false)
  const { currentUser } = useSelector((state) => state.user)

  const dispatch = useDispatch()

  useEffect(() => {
    const fetchNotifications = async () => {
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

          dispatch(setUnreadCount(unreadCount))
        }
      } catch (error) {
        console.log(error)
      }
    }

    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5173/notificationsHub")
      .withAutomaticReconnect()
      .build()

    connection.start()
      .then(() => {
        console.log('connected')

        connection.on("ReceiveNotification", (notification) => {
          console.log('Received notification:', notification);

          setNotifications((prevNotification) => [notification, ...prevNotification])

          setHasNewNotifications(true)
          dispatch(setUnreadCount((prevCount) => prevCount + 1))
        })

        connection.on("NotificationsMarkedAsRead", () => {
          setNotifications((prev) => prev.map((notification) => ({ ...notification, isRead: true })))

          setHasNewNotifications(false)
          dispatch(setUnreadCount(0))
        })
      })
      .catch(error => console.error('Connection failed ', error))

    fetchNotifications()

    return () => {
      connection.stop()
    }

  }, [currentUser.id, dispatch])

  console.log(notifications)

  return (
    <div className="flex flex-col md:flex-row h-screen bg-gray-100 dark:bg-gray-800 px-96">
      <div className="p-4 md:w-1/4 bg-white dark:bg-gray-800 shadow-lg border-r border-gray-300 dark:border-gray-700">
        <h1 className="text-2xl font-bold mb-4 mt-1">Notifications</h1>
        <ul className="space-y-2">
          <li className="cursor-pointer hover:bg-gray-200 dark:hover:bg-indigo-600 hover:rounded-md p-2">All</li>
          <li className="cursor-pointer hover:bg-gray-200 dark:hover:bg-indigo-600 hover:rounded-md p-2">Posts</li>
          <li className="cursor-pointer hover:bg-gray-200 dark:hover:bg-indigo-600 hover:rounded-md p-2">Comments</li>
          <li className="cursor-pointer hover:bg-gray-200 dark:hover:bg-indigo-600 hover:rounded-md p-2">Likes</li>
        </ul>
      </div>

      <div className="flex-1 pr-6 pl-6 bg-white dark:bg-gray-800">
        <div className="mb-4">
        </div>
        <div className="space-y-4">
          {notifications.map((notification) => (
            <div key={notification.id} className={`p-4 border-b border-gray-300 dark:border-gray-400 rounded-lg shadow-sm hover:bg-gray-50 dark:hover:bg-gray-700 ${notification.isRead ? "" : "border-blue-500"}`}>
              <p className="text-gray-700 dark:text-white">{notification.content}</p>
              <p className="text-sm text-gray-500">{new Date(notification.createdAt).toLocaleString()}</p>
            </div>
          ))}
        </div>
      </div>
    </div>
  )
}
