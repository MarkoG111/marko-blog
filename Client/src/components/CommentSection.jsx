import PropTypes from 'prop-types'
import { Button, Textarea, Modal } from 'flowbite-react'
import { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import { Link } from 'react-router-dom'
import { HiOutlineExclamationCircle } from 'react-icons/hi'

import Comment from './Comment'

import {
  checkIfAlreadyVoted,
  removeDislikeOrLikeIfPresent,
  updateCommentLikes,
  handleOptimisticUpdate
} from '../utils/commentUtils'
import { useError } from '../contexts/ErrorContext'

export default function CommentSection({ idPost }) {
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

  useEffect(() => {
    const fetchPostAndComments = async () => {
      try {
        const response = await fetch(`/posts/${idPost}`)

        if (response.ok) {
          const data = await response.json()

          setPost(data)
          setComments(data.comments.reverse())
          setChildComments(data.childrenComments.reverse())
          setCommentsNumber(data.comments.length + data.childrenComments.length)
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

      const response = await fetch(`/comments`, {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: body
      })

      if (response.ok) {
        const data = await response.json()

        setComments([data, ...comments])
        setCommentsNumber(commentsNumber + 1)
        setComment('')
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

      const response = await fetch(`/comments`, {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: body
      })

      if (response.ok) {
        const data = await response.json()

        setChildComments([data, ...childComments])
        setActiveReplyIdComment(null)
        setCommentsNumber(commentsNumber + 1)
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
    }
    catch (error) {
      showError(error.message)
    }
  }

  const handleLikeCommentOptimistic = (idComment) => {
    handleOptimisticUpdate(comments, setComments, idComment, 'like')
    handleLikeComment(idComment)
  }

  const handleLikeComment = async (idComment) => {
    try {
      const token = localStorage.getItem("token")
      if (!token) {
        showError("You must be logged in to like a comment.")
        return
      }

      const isAlreadyLiked = checkIfAlreadyVoted(comments, idComment, currentUser.id, 1)
      const isAlreadyLikedChild = checkIfAlreadyVoted(childComments, idComment, currentUser.id, 1)

      if (isAlreadyLiked || isAlreadyLikedChild) {
        return
      }

      const body = JSON.stringify({
        IdUser: currentUser.id,
        IdPost: idPost,
        IdComment: idComment,
        Status: 1,
      })

      const response = await fetch(`/comments/${idComment}/like`, {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: body
      })

      if (response.ok) {
        const data = await response.json()

        // Remove dislike if present
        const updatedComments = removeDislikeOrLikeIfPresent(comments, idComment, currentUser.id, 2)
        setComments(updatedComments)

        const updatedChildComments = removeDislikeOrLikeIfPresent(childComments, idComment, currentUser.id, 2)
        setChildComments(updatedChildComments)

        // Update comment likes
        const updatedCommentsWithLikes = updateCommentLikes(updatedComments, idComment, data, currentUser.id)
        setComments(updatedCommentsWithLikes)

        const updatedChildCommentsWithLikes = updateCommentLikes(updatedChildComments, idComment, data, currentUser.id)
        setChildComments(updatedChildCommentsWithLikes)
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
      showError(error.message || "An unknown error occurred.")
    }
  }

  const handleDislikeCommentOptimistic = (idComment) => {
    handleOptimisticUpdate(comments, setComments, idComment, 'dislike')
    handleDislikeComment(idComment)
  }

  const handleDislikeComment = async (idComment) => {
    try {
      const token = localStorage.getItem("token")
      if (!token) {
        showError("You must be logged in to dislike a comment.")
        return
      }

      const isAlreadyDisliked = checkIfAlreadyVoted(comments, idComment, currentUser.id, 2)
      const isAlreadyDislikedChild = checkIfAlreadyVoted(childComments, idComment, currentUser.id, 2)

      if (isAlreadyDisliked || isAlreadyDislikedChild) {
        return
      }

      const body = JSON.stringify({
        IdUser: currentUser.id,
        IdPost: idPost,
        IdComment: idComment,
        Status: 2
      })

      const response = await fetch(`/comments/${idComment}/like`, {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: body
      })

      if (response.ok) {
        const data = await response.json()

        // Remove like if present
        const updatedComments = removeDislikeOrLikeIfPresent(comments, idComment, currentUser.id, 1)
        setComments(updatedComments)

        const updatedChildComments = removeDislikeOrLikeIfPresent(childComments, idComment, currentUser.id, 1)
        setChildComments(updatedChildComments)

        // Update comment dislikes
        const updatedCommentsWithLikes = updateCommentLikes(updatedComments, idComment, data, currentUser.id)
        setComments(updatedCommentsWithLikes)

        const updatedChildCommentsWithLikes = updateCommentLikes(updatedChildComments, idComment, data, currentUser.id)
        setChildComments(updatedChildCommentsWithLikes)
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
      showError(error.message || "An unknown error occurred.")
    }
  }

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

  const handleDeleteComment = async (idComment) => {
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

      const response = await fetch(`/comments/${idComment}`, {
        method: "DELETE",
        headers: {
          "Authorization": `Bearer ${token}`
        },
        body: body
      })

      if (response.ok) {
        const updatedComments = comments.map(comment =>
          comment.id === idComment ? { ...comment, isDeleted: 1 } : comment
        )
        setComments(updatedComments)

        const updatedChildComments = childComments.map(comment =>
          comment.id === idComment ? { ...comment, isDeleted: 1 } : comment
        )
        setChildComments(updatedChildComments)

        setCommentsNumber(commentsNumber - 1)
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
      showError(error.message)
    }
  }

  return (
    <div className='max-w-2xl mx-auto w-full p-3'>
      {currentUser ? (
        <div className='flex items-center gap-1 my-5 text-gray-500 text-sm'>
          <p>Signed in as:</p>
          <img src={currentUser.profilePicture.startsWith('http') ? currentUser.profilePicture : `../users/images/${currentUser.profilePicture}`} alt='' className='w-10 object-cover rounded-full' />
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
              onLikeComment={handleLikeCommentOptimistic}
              onDislikeComment={handleDislikeCommentOptimistic}
              onAddChildComment={(e, idComment, childComment) => addChildComment(e, idComment, childComment)}
              childrenComments={childComments}
              onEditComment={handleEditComment}
              onDeleteComment={(idComment) => {
                setShowModalToDeleteComment(true)
                setCommentToDelete(idComment)
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
}