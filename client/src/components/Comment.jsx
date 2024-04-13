<<<<<<< HEAD
/* eslint-disable react/prop-types */

import { useEffect, useState } from "react"
import moment from 'moment'
import { FaThumbsUp, FaThumbsDown } from 'react-icons/fa'

export default function Comment({ comment, onLike, onDislike }) {
  const [user, setUser] = useState({})
  const formattedTime = moment(comment.createdAt).fromNow();
  
  useEffect(() => {
    const getUser = async () => {
      try {
        const response = await fetch(`/api/Users/${comment.idUser}`)
        const data = await response.json()
        if (response.ok) {
          setUser(data)
        }
      } catch (error) {
        console.log(error)
      }
    }

    getUser()
  }, [comment])

  return (
    <div className="flex p-4 border-b dark:border-gray-600 text-sm">
      <div className="flex-shrink-0 mr-3">
        <img className="w-10 h-10 rounded-full bg-gray-200" src={user && user.profilePicture && user.profilePicture.startsWith('http') ? user.profilePicture : `api/Users/images/${user.profilePicture}`} alt={user.username} />
      </div>
      <div className="flex-1">
        <div className="flex items-center mb-1">
          <span className="font-bold mr-1 text-xs truncate">{user ? `@${user.username}` : 'anonymous user'}</span>
          <span className="text-gray-500 text-xs">
            {formattedTime}
          </span>
        </div>

        <p className="text-gray-500 pb-2">{comment.commentText}</p>

        <div>
          <button type="button" className="text-gray-400 hover:text-blue-500">
            <FaThumbsUp className="text-sm" />
          </button>

          <button type="button" className="text-gray-400 hover:text-red-500 ml-6">
            <FaThumbsDown className="text-sm" />
          </button>
        </div>
      </div>

      <div className="border h-8 border-gray-400 py-1 px-2 rounded-sm cursor-pointer">
        <p>Reply</p>
      </div>
    </div>
  )
}
=======
import { useEffect, useState } from "react"

export default function Comment({ comment }) {
  const [user, setUser] = useState({})
  console.log(user);


  return (
    <div>Comment</div>
  )
}
>>>>>>> 302b558e8d1e73a251f80e54cd26e042048d1532
