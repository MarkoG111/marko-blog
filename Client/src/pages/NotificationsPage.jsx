import { useEffect, useState } from "react"
import * as signalR from '@microsoft/signalr'
import { useDispatch, useSelector } from "react-redux";
import { setUnreadCount } from "../redux/notificationsSlice";

export default function Notifications() {
  const [notifications, setNotifications] = useState([])
  const [hasNewNotifications, setHasNewNotifications] = useState(false)
  const [page, setPage] = useState(1)
  const [pageCount, setPageCount] = useState(0)
  const [fetching, setFetching] = useState(false)
  const [type, setType] = useState(null)

  const dispatch = useDispatch()
  const { currentUser } = useSelector((state) => state.user)

  const calculatePerPage = () => {
    const notificationHeight = 80
    const screenHeight = window.innerHeight
    return Math.ceil(screenHeight / notificationHeight) * 2
  }

  const fetchNotifications = async (pageNumber, type) => {
    try {
      setFetching(true)

      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const dynamicPerPage = calculatePerPage()

      const url = new URL(`/api/Notifications`, window.location.origin)
      url.searchParams.append("page", pageNumber)
      url.searchParams.append("perPage", dynamicPerPage)

      if (type != null) {
        url.searchParams.append("type", type)
      }

      const response = await fetch(url, {
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
        setNotifications((prev) => [...prev, ...data.items])

        const unreadCount = data.items.filter((n) => !n.isRead).length
        setHasNewNotifications(unreadCount > 0)
        dispatch(setUnreadCount(unreadCount))
        setPageCount(data.pageCount)
      }
    } catch (error) {
      console.log(error)
    } finally {
      setFetching(false)
    }
  }

  const handleScroll = () => {
    const scrollPosition = window.innerHeight + document.documentElement.scrollTop
    const bottomPosition = document.documentElement.offsetHeight - 50

    if (scrollPosition >= bottomPosition && page < pageCount && !fetching) {
      setPage((prevPage) => prevPage + 1)
    }
  }

  const handleTypeChange = (newType) => {
    setType(newType)
    setPage(1)
    setNotifications([])
  }

  useEffect(() => {
    fetchNotifications(1, type)
  }, [type]);

  useEffect(() => {
    window.addEventListener("scroll", handleScroll)

    return () => {
      window.removeEventListener("scroll", handleScroll)
    }
  }, [fetching, page, pageCount])

  useEffect(() => {
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

    return () => {
      connection.stop()
    }

  }, [dispatch])

  console.log(notifications)

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
