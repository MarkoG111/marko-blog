import PropTypes from 'prop-types'
import { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import { Link } from 'react-router-dom'
import { HiOutlineExclamationCircle } from 'react-icons/hi'
import { Button, Textarea, Modal } from 'flowbite-react'
import { handleApiError } from '../utils/handleApiUtils'
import Comment from './Comment'
import { useError } from '../contexts/ErrorContext'
import { getAvatarSrc } from "../utils/getAvatarSrc"

import {
  checkIfAlreadyVoted,
  removeDislikeOrLikeIfPresent,
  updateCommentLikes,
} from '../utils/commentUtils'
export default function CommentSection({ idPost, onCommentsNumberChange }) {
  const { currentUser } = useSelector(state => state.user)

  const [post, setPost] = useState({})
  const [comment, setComment] = useState('')
  const [comments, setComments] = useState([])
  const [childComments, setChildComments] = useState([])
  const [commentsNumber, setCommentsNumber] = useState(0)
  const [activeReplyIdComment, setActiveReplyIdComment] = useState(null)
  const [showModalToDeleteComment, setShowModalToDeleteComment] = useState(false)
  const [commentToDelete, setCommentToDelete] = useState(null)

  const { showError } = useError()

  const handleCommentsNumberChange = (newCommentsNumber) => {
    setCommentsNumber(newCommentsNumber)
    onCommentsNumberChange(newCommentsNumber)
  }

  useEffect(() => {
    const fetchPostAndComments = async () => {
      try {
        const response = await fetch(`/api/posts/${idPost}`)

        if (response.ok) {
          const data = await response.json()
          const mainComments = data.comments
          const allChildComments = mainComments.flatMap(comment => comment.childrenComments || [])

          mainComments.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt))

          setPost(data)
          setComments(mainComments)

          setChildComments(allChildComments)

          const mainCommentsNotDeleted = mainComments.filter(comment => !comment.isDeleted).length
          const childCommentsNotDeleted = allChildComments.filter(comment => !comment.isDeleted).length

          handleCommentsNumberChange(mainCommentsNotDeleted + childCommentsNotDeleted)
        } else {
          await handleApiError(response, showError)
        }
      } catch (error) {
        showError(error.message)
      }
    }

    fetchPostAndComments()
  }, [idPost, showError])

  const handleSubmitComment = async (e) => {
    e.preventDefault()

    if (comment.length > 200) {
      return
    }

    try {
      const token = localStorage.getItem("token")
      if (!token) {
        showError("Token not found")
        return
      }

      const lastIdComment = comments.length > 0 ? Math.max(...comments.map(comment => comment.id)) : 0

      const body = JSON.stringify({
        Id: lastIdComment,
        CommentText: comment,
        IdPost: idPost,
        IdUser: currentUser.id,
        Username: currentUser.username
      })

      const response = await fetch(`/api/comments`, {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: body
      })

      if (response.ok) {
        const newComment = await response.json()

        setComments([newComment, ...comments])
        handleCommentsNumberChange(commentsNumber + 1)
        setComment('')
      } else {
        await handleApiError(response, showError)
      }
    } catch (error) {
      showError(error.message)
    }
  }

  const addChildComment = async (e, idComment, childComment) => {
    e.preventDefault()

    const token = localStorage.getItem("token")
    if (!token) {
      showError("Token not found")
      return
    }

    try {
      const body = JSON.stringify({
        IdPost: idPost,
        CommentText: childComment,
        IdParent: idComment,
        IdUser: currentUser.id,
        Username: currentUser.username
      })

      const response = await fetch(`/api/comments`, {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: body
      })

      if (response.ok) {
        const newChildComment = await response.json()

        setChildComments([newChildComment, ...childComments])

        setActiveReplyIdComment(null)
        handleCommentsNumberChange(commentsNumber + 1)
      } else {
        await handleApiError(response, showError)
      }
    }
    catch (error) {
      showError(error.message)
    }
  }

  const handleVoteComment = async (idComment, voteType) => {
    const token = localStorage.getItem("token")
    if (!token) {
      showError("Token not found")
      return
    }

    const isAlreadyVoted = checkIfAlreadyVoted(comments, idComment, currentUser.id, voteType) || checkIfAlreadyVoted(childComments, idComment, currentUser.id, voteType)

    if (isAlreadyVoted) {
      return
    }

    try {
      const body = JSON.stringify({
        IdUser: currentUser.id,
        IdPost: idPost,
        IdComment: idComment,
        Status: voteType,
      })

      const response = await fetch(`/api/comments/${idComment}/like`, {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body
      })

      if (response.ok) {
        const data = await response.json()

        const updatedComments = updateCommentLikes(removeDislikeOrLikeIfPresent(comments, idComment, currentUser.id, voteType === 1 ? 2 : 1), idComment, data, currentUser.id)
        const updatedChildComments = updateCommentLikes(removeDislikeOrLikeIfPresent(childComments, idComment, currentUser.id, voteType === 1 ? 2 : 1), idComment, data, currentUser.id)

        setComments(updatedComments)
        setChildComments(updatedChildComments)
      } else {
        await handleApiError(response, showError)
      }
    } catch (error) {
      showError(error.message)
    }
  }

  const handleLikeComment = (idComment) => handleVoteComment(idComment, 1)
  const handleDislikeComment = (idComment) => handleVoteComment(idComment, 2)

  const handleEditComment = async (comment, editedText) => {
    setComments(
      comments.map((c) =>
        c.id == comment.id ? { ...c, commentText: editedText } : c
      )
    )

    setChildComments(
      childComments.map((c) =>
        c.id == comment.id ? { ...c, commentText: editedText } : c
      )
    )
  }

  const handleDeleteComment = async (comment) => {
    setShowModalToDeleteComment(false)

    try {
      const token = localStorage.getItem("token")
      if (!token) {
        showError("Token not found")
        return
      }

      const body = JSON.stringify({
        isDeleted: 1
      })

      const url = currentUser.id === comment.idUser ? `/api/comments/${comment.id}/personal` : `/api/comments/${comment.id}`

      const response = await fetch(url, {
        method: "DELETE",
        headers: {
          "Authorization": `Bearer ${token}`
        },
        body: body
      })

      if (response.ok) {
        const updatedComments = comments.map(comment =>
          comment.id === commentToDelete.id ? { ...comment, isDeleted: 1 } : comment
        )
        setComments(updatedComments)

        const updatedChildComments = childComments.map(comment =>
          comment.id === commentToDelete.id ? { ...comment, isDeleted: 1 } : comment
        )
        setChildComments(updatedChildComments)

        handleCommentsNumberChange(commentsNumber - 1)
      } else {
        await handleApiError(response, showError)
      }
    } catch (error) {
      showError(error.message)
    }
  }

  return (
    <div className='max-w-2xl mx-auto w-full p-3'>
      {currentUser ? (
        <div className='flex items-center gap-1 my-5 text-gray-500 text-sm'>
          <p>Signed in as:</p>
          <img src={getAvatarSrc(currentUser.profilePicture)} referrerPolicy="no-referrer" alt='user' className='w-10 object-cover rounded-full' />
          <Link to={'/dashboard?tab=profile'} className='text-cyan-600 hover:underline'>
            @{currentUser.username}
          </Link>
        </div>
      ) : (<div className='text-teal-500 my-5 flex gap-1'>You must be signed in to comment. <Link to={'/sign-in'} className='text-blue-500 hover:underline'>Sign In</Link></div>)}

      {currentUser && (
        <form className='border border-teal-500 rounded-md p-3' onSubmit={handleSubmitComment}>
          <Textarea placeholder='Leave a comment...' rows='3' maxLength='200' onChange={(e) => setComment(e.target.value)} value={comment} />
          <div className='flex justify-between items-center mt-5'>
            <p className='text-gray-500 text-sm'>{200 - comment.length} characters remaining</p>
            <Button outline gradientDuoTone='purpleToBlue' type='submit'>Submit</Button>
          </div>
        </form>
      )}

      {comments.length === 0 ? (
        <p className="text-sm my-5">No comments yet!</p>
      ) : (
        <>
          {comments.map(comment => (
            <Comment key={comment.id}
              comment={comment}
              onLikeComment={handleLikeComment}
              onDislikeComment={handleDislikeComment}
              onAddChildComment={(e, idComment, childComment) => addChildComment(e, idComment, childComment)}
              childrenComments={childComments}
              onEditComment={handleEditComment}
              onDeleteComment={(comment) => {
                setShowModalToDeleteComment(true)
                setCommentToDelete(comment)
              }}
              setCommentToDelete={setCommentToDelete}
              setShowModalToDeleteComment={setShowModalToDeleteComment}
              setActiveReplyIdComment={setActiveReplyIdComment}
              activeReplyIdComment={activeReplyIdComment}
              comments={comments}
            />
          ))}
        </>
      )}

      <Modal show={showModalToDeleteComment} onClose={() => setShowModalToDeleteComment(false)} popup size='md'>
        <Modal.Header />
        <Modal.Body>
          <div className="text-center">
            <HiOutlineExclamationCircle className="h-14 w-14 text-gray-400 dark:text-gray-200 mb-4 mx-auto" />
            <h3 className="mb-5 text-lg text-gray-500 dark:text-gray-400">Are you sure you want to delete this comment?</h3>

            <div className="flex justify-center gap-4">
              <Button color="failure" onClick={() => handleDeleteComment(commentToDelete)}>
                Yes, I&apos;m sure
              </Button>
              <Button color="gray" onClick={() => setShowModalToDeleteComment(false)}>No, cancel</Button>
            </div>
          </div>
        </Modal.Body>
      </Modal>
    </div>
  )
}

CommentSection.propTypes = {
  idPost: PropTypes.number.isRequired,
  onCommentsNumberChange: PropTypes.func,
}