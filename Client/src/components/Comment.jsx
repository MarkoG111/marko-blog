import PropTypes from 'prop-types'
import { useEffect, useState } from "react"
import moment from 'moment'
import { FaThumbsUp, FaThumbsDown } from 'react-icons/fa'
import { Button, Textarea } from 'flowbite-react'
import { useSelector } from "react-redux"

import ChildComment from './ChildComment'

export default function Comment({ comment, onLike, onDislike, onAddChildComment, childrenComments, onEdit, onDelete, setActiveReplyCommentId, activeReplyCommentId, comments }) {
  const [user, setUser] = useState({})
  const { currentUser } = useSelector((state) => state.user)

  const formattedTime = moment(comment.createdAt).fromNow()
  const isFirstReply = comment.idParent === null

  const [isEditing, setIsEditing] = useState(false)
  const [editedText, setEditedText] = useState(comment.commentText)

  const [childComment, setChildComment] = useState('')
  const isChildComment = childrenComments.some(child => child.idParent == comment.id)

  const isSmallScreen = window.innerWidth <= 768

  useEffect(() => {
    const getUser = async () => {
      try {
        const response = await fetch(`/api/Users/${comment.idUser}`, {
          method: "GET"
        })

        if (response.ok) {
          const data = await response.json()
          setUser(data)
        }
      } catch (error) {
        console.log(error)
      }
    }

    getUser()
  }, [comment])


  const openEdit = async () => {
    setIsEditing(true)
    setEditedText(comment.commentText)
  }

  const handleSave = async () => {
    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const response = await fetch(`/api/Comments/${comment.id}`, {
        method: "PUT",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: JSON.stringify({ commentText: editedText })
      })

      if (response.ok) {
        setIsEditing(false)
        onEdit(comment, editedText)
      }
    } catch (error) {
      console.log(error)
    }
  }

  const toggleReplyForm = (commentId) => {
    const allComments = [...comments, ...childrenComments]
    const commentToReply = allComments.find(comment => comment.id === commentId)

    if (commentToReply && commentToReply.username) {
      const replyTo = `@${commentToReply.username}  `
      setChildComment(replyTo)
    } else {
      setChildComment('')
    }

    setActiveReplyCommentId(commentToReply ? commentToReply.id : null)
  }

  return (
    <>
      <div className="flex p-4 border-b dark:border-gray-600 text-sm">
        <div className="flex-shrink-0 mr-3">
          <img className="w-10 h-10 rounded-full bg-gray-200" src={user && user.profilePicture && user.profilePicture.startsWith('http') ? user.profilePicture : `api/Users/images/${user.profilePicture}`} alt={user.username} />
        </div>

        <div className="flex-1">
          <div className="flex items-center mb-1">
            <span className="font-bold mr-1 text-xs truncate">{user ? `${user.username}` : 'anonymous user'}</span>
            <span className="text-gray-500 text-xs">
              {formattedTime}
            </span>
          </div>

          {isEditing ? (
            <>
              <Textarea placeholder='Leave a comment...' className="mb-2" maxLength='200' onChange={(e) => setEditedText(e.target.value)} value={editedText} />
              <div className="flex justify-end gap-2 text-xs">
                <Button type="button" gradientDuoTone='purpleToBlue' size='sm' onClick={() => handleSave()}>Save</Button>
                <Button type="button" gradientDuoTone='purpleToBlue' size='sm' outline onClick={() => setIsEditing(false)}>Cancel</Button>
              </div>
            </>
          ) : (
            <>
              {
                comment.isDeleted ? (
                  <>
                    <div className="flex p-3 text-sm text-gray-500 italic">
                      <div className="flex-1">
                        <p className="">Comment is removed</p>
                      </div>
                    </div>
                  </>) : (
                  <>
                    <p className="text-gray-500 pb-2">{comment.commentText}</p>

                    <div className="flex items-center pt-2 text-xs border-t dark:border-gray-700 max-w-fit gap-2">
                      <button type="button" onClick={() => onLike(comment.id)} className={`text-gray-400 hover:text-blue-500 ${currentUser && comment.likes && comment?.likes.some(like => like.idUser == currentUser.id && like.status == 1) && '!text-blue-500'}`}>
                        <FaThumbsUp className="text-sm" />
                      </button>
                      {comment.likes && comment.likes.filter((like) => like.status == 1).length}

                      <button type="button" onClick={() => onDislike(comment.id)} className={`text-gray-400 hover:text-red-500 ml-6 ${currentUser && comment.likes && comment.likes.some(like => like.idUser == currentUser.id && like.status == 2) && '!text-red-500'}`}>
                        <FaThumbsDown className="text-sm" />
                      </button>
                      {comment.likes && comment.likes.filter((like) => like.status == 2).length}

                      {currentUser && (currentUser.id == comment.idUser || currentUser.roleName == 'Admin') && (
                        <>
                          <button type="button" className="text-gray-400 hover:text-blue-500 ml-6" onClick={() => openEdit()}>Edit</button>
                          <button type="button" className="text-gray-400 hover:text-red-500 ml-6" onClick={() => onDelete(comment.id)}>Delete</button>
                        </>
                      )}
                    </div>
                  </>)
              }
            </>
          )}
        </div>

        {isEditing || comment.isDeleted || currentUser == null ? (
          <></>
        ) : (
          <>
            <div className="-ml-4">
              <Button className="" outline gradientDuoTone='purpleToBlue' onClick={() => toggleReplyForm(comment.id)}>Reply</Button>
            </div >
          </>
        )
        }
      </div>

      {activeReplyCommentId == comment.id &&
        (<div className="p-10">
          <form className="border border-teal-500 rounded-md p-3" onSubmit={(e) => onAddChildComment(e, comment.id, childComment)}>
            <Textarea placeholder='Leave a comment...' rows='3' maxLength='200' onChange={(e) => setChildComment(e.target.value)} value={childComment} />
            <div className='flex justify-between items-center mt-5'>
              <Button outline gradientDuoTone='purpleToBlue' type='submit'>Submit</Button>
            </div>
          </form>
        </div>)}

      {isChildComment && (
        <ChildComment
          childComments={childrenComments}
          parentCommentId={comment.id}
          onDelete={onDelete}
          onLike={onLike}
          onDislike={onDislike}
          onEdit={onEdit}
          onAddChildComment={onAddChildComment}
          activeReplyCommentId={activeReplyCommentId}
          setActiveReplyCommentId={setActiveReplyCommentId}
          comments={comments}
          isFirstReply={isFirstReply}
          isSmallScreen={isSmallScreen}
        />
      )}
    </>
  )
}

Comment.propTypes = {
  comment: PropTypes.object.isRequired,
  onLike: PropTypes.func.isRequired,
  onDislike: PropTypes.func.isRequired,
  onAddChildComment: PropTypes.func.isRequired,
  childrenComments: PropTypes.array.isRequired,
  onEdit: PropTypes.func.isRequired,
  onDelete: PropTypes.func.isRequired,
  setCommentToDelete: PropTypes.func.isRequired,
  setShowModal: PropTypes.func.isRequired,
  setActiveReplyCommentId: PropTypes.func.isRequired,
  activeReplyCommentId: PropTypes.number,
  comments: PropTypes.array.isRequired,
}
