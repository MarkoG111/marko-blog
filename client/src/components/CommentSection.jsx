/* eslint-disable react/prop-types */
import { Button, Textarea, Modal } from 'flowbite-react'
import { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import { Link } from 'react-router-dom'
import { HiOutlineExclamationCircle } from 'react-icons/hi'

import Comment from './Comment'
import { FaRegCommentDots, FaThumbsDown, FaThumbsUp } from 'react-icons/fa'

export default function CommentSection({ idPost }) {
  const { currentUser } = useSelector(state => state.user)
  const [post, setPost] = useState({})

  const [comment, setComment] = useState('')
  const [comments, setComments] = useState([])
  const [childComments, setChildComments] = useState([])
  const [commentsNumber, setCommentsNumber] = useState(0)
  const [activeReplyCommentId, setActiveReplyCommentId] = useState(null)
  const [showModal, setShowModal] = useState(false)
  const [commentToDelete, setCommentToDelete] = useState(null)
  const [errorMessage, setErrorMessage] = useState('')
  const [showErrorModal, setShowErrorModal] = useState(false)

  useEffect(() => {
    const fetchComments = async () => {
      try {
        const response = await fetch(`/api/Posts/${idPost}`, {
          method: "GET"
        })

        if (response.ok) {
          const data = await response.json()

          setCommentsNumber(data.comments.length + data.childrenComments.length)

          setComments(data.comments)
          setChildComments(data.childrenComments)
        }
      } catch (error) {
        console.error("Error fetching comments:", error)
      }
    }

    const getPost = async () => {
      try {
        const response = await fetch(`/api/Posts/${idPost}`, {
          method: "GET"
        })

        if (response.ok) {
          const data = await response.json()
          setPost(data)
        }
      } catch (error) {
        console.log(error)
      }
    }

    getPost()
    fetchComments()
  }, [idPost])

  useEffect(() => {
    if (showErrorModal) {
      setShowErrorModal(true)
    }
    const timer = setTimeout(() => {
      setShowErrorModal(false)
    }, 10000)

    return () => clearTimeout(timer)

  }, [showErrorModal])

  const handleSubmit = async (e) => {
    e.preventDefault()

    if (comment.length > 200) {
      return
    }

    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const lastIdComment = comments.length > 0 ? Math.max(...comments.map(comment => comment.id)) : 0

      const body = JSON.stringify({
        Id: lastIdComment,
        CommentText: comment,
        IdPost: idPost,
        IdUser: currentUser.id,
        Username: currentUser.username
      })

      const response = await fetch(`/api/Comments`, {
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
      }
    } catch (error) {
      console.log(error)
    }
  }

  const addChildComment = async (e, idComment, childComment) => {
    e.preventDefault()

    const token = localStorage.getItem("token")
    if (!token) {
      throw new Error("Token not found")
    }

    try {
      const body = JSON.stringify({
        IdPost: idPost,
        CommentText: childComment,
        IdParent: idComment,
        IdUser: currentUser.id,
        Username: currentUser.username
      })

      const response = await fetch(`/api/Comments`, {
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
        setActiveReplyCommentId(null)
        setCommentsNumber(commentsNumber + 1)
      }
    }
    catch (error) {
      console.log(error)
    }
  }

  const checkIfAlreadyVoted = (comments, idComment, idUser, status) => {
    return comments.some(comment => {
      if (comment.id === idComment && comment.likes) {
        return comment.likes.some(like => like.idUser === idUser && like.status === status)
      }

      return false
    })
  }

  const checkIfAlreadyVotedOnPost = (post, idPost, idUser, status) => {
    if (post.id == idPost && post.likes) {
      return post.likes.some(like => like.idUser == idUser && like.status == status)
    }

    return false
  }

  const removeDislikeOrLikeIfPresent = (comments, idComment, idUser, status) => {
    return comments.map((comment) => {
      if (comment.id === idComment && comment.likes) {
        const hasDisliked = comment.likes.some(like => like.idUser === idUser && like.status === status)
        if (hasDisliked) {
          const updatedLikes = comment.likes.filter(like => !(like.idUser === idUser && like.status === status))
          return { ...comment, likes: updatedLikes }
        }
      }

      return comment
    })
  }

  const removeDislikeOrLikeIfPresentInPost = (post, idPost, idUser, status) => {
    if (post.id == idPost) {
      const hasVote = post.likes.some(like => like.idUser == idUser && like.status == status)
      if (hasVote) {
        const updateLikes = post.likes.filter(like => !(like.idUser == idUser && like.status == status))
        return { ...post, likes: updateLikes }
      }
    }

    return post
  }

  const updateCommentLikes = (comments, idComment, data, userId) => {
    return comments.map(comment => {
      if (comment.id === idComment) {
        return {
          ...comment,
          likesCount: data.likesCount,
          likes: comment.likes.some(like => like.idUser === userId)
            ? comment.likes.map(like => (like.idUser === userId ? { ...like, status: data.status } : like))
            : [...comment.likes, { idUser: userId, idComment, status: data.status }],
        }
      }

      if (comment.children && comment.children.length > 0) {
        return {
          ...comment,
          children: updateCommentLikes(comment.children, idComment, data, userId),
        }
      }

      return comment
    })
  }

  const updatePostLikes = (post, idPost, data, userId) => {
    if (post.id == idPost) {
      return {
        ...post,
        likes: post.likes.some(like => like.idUser == userId)
          ? post.likes.map(like => (like.idUser == userId ? { ...like, status: data.status } : like)) : [...post.likes, { idUser: userId, idPost, status: data.status }]
      }
    }

    return post
  }

  const handleLike = async (idComment) => {
    try {
      const token = localStorage.getItem("token")
      if (!token) {
        setShowErrorModal(true)
        setErrorMessage("You must be logged in to like a comment.")
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

      const response = await fetch(`/api/Comments/like`, {
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
        const errorText = await response.text() // Get the response as text
        const errorData = JSON.parse(errorText) // Try to parse it as JSON
        setShowErrorModal(true)
        setErrorMessage(errorData.message)
      }
    } catch (error) {
      setShowErrorModal(true)
      setErrorMessage("An error occurred while processing your request.")
    }
  }

  const handleDislike = async (idComment) => {
    try {
      const token = localStorage.getItem("token")
      if (!token) {
        setShowErrorModal(true)
        setErrorMessage("You must be logged in to dislike a comment.")
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

      const response = await fetch(`/api/Comments/like`, {
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
        setShowErrorModal(true)
        setErrorMessage(errorData.message)
      }
    } catch (error) {
      setShowErrorModal(true)
      setErrorMessage("An error occurred while processing your request.")
    }
  }

  const handleEdit = async (comment, editedText) => {
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

  const handleDelete = async (commentId) => {
    setShowModal(false)

    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }
      const body = JSON.stringify({
        isDeleted: 1
      })

      const response = await fetch(`/api/Comments/comment/${commentId}`, {
        method: "DELETE",
        headers: {
          "Authorization": `Bearer ${token}`
        },
        body: body
      })

      if (response.ok) {
        const updatedComments = comments.map(comment =>
          comment.id === commentId ? { ...comment, isDeleted: 1 } : comment
        )
        setComments(updatedComments)

        const updatedChildComments = childComments.map(comment =>
          comment.id === commentId ? { ...comment, isDeleted: 1 } : comment
        )
        setChildComments(updatedChildComments)

        setCommentsNumber(commentsNumber - 1)
      }
    } catch (error) {
      console.log(error)
    }
  }

  const onLikePost = async (idPost) => {
    try {
      const token = localStorage.getItem("token")
      if (!token) {
        setShowErrorModal(true)
        setErrorMessage("You must be logged in to like a post.")
        return
      }

      const isAlreadyLiked = checkIfAlreadyVotedOnPost(post, idPost, currentUser.id, 1)

      if (isAlreadyLiked) {
        return
      }

      const body = JSON.stringify({
        IdUser: currentUser.id,
        IdPost: idPost,
        Status: 1
      })

      const response = await fetch(`/api/Posts/like`, {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: body
      })

      if (response.ok) {
        const data = await response.json()

        const updatedPost = removeDislikeOrLikeIfPresentInPost(post, idPost, currentUser.id, 2)
        setPost(updatedPost)
        const updatePostWithLikes = updatePostLikes(updatedPost, idPost, data, currentUser.id)
        setPost(updatePostWithLikes)
      } else {
        const errorText = await response.text()
        const errorData = JSON.parse(errorText)
        setShowErrorModal(true)
        setErrorMessage(errorData.message)
      }
    } catch (error) {
      setShowErrorModal(true)
      setErrorMessage("An error occurred while processing your request.")
    }
  }

  const onDislikePost = async (idPost) => {
    try {
      const token = localStorage.getItem("token")
      if (!token) {
        setShowErrorModal(true)
        setErrorMessage("You must be logged in to dislike a post.")
        return
      }

      const isAlreadyDisliked = checkIfAlreadyVotedOnPost(post, idPost, currentUser.id, 2)

      if (isAlreadyDisliked) {
        return
      }

      const body = JSON.stringify({
        IdUser: currentUser.id,
        IdPost: idPost,
        Status: 2
      })

      const response = await fetch(`/api/Posts/like`, {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: body
      })

      if (response.ok) {
        const data = await response.json()

        const updatedPost = removeDislikeOrLikeIfPresentInPost(post, idPost, currentUser.id, 1)
        setPost(updatedPost)
        const updatePostWithLikes = updatePostLikes(updatedPost, idPost, data, currentUser.id)
        setPost(updatePostWithLikes)
      } else {
        const errorText = await response.text()
        const errorData = JSON.parse(errorText)
        setShowErrorModal(true)
        setErrorMessage(errorData.message)
      }
    } catch (error) {
      setShowErrorModal(true)
      setErrorMessage("An error occurred while processing your request.")
    }
  }

  return (
    <div className='max-w-2xl mx-auto w-full p-3'>
      {currentUser ? (
        <div className='flex items-center gap-1 my-5 text-gray-500 text-sm'>
          <p>Signed in as:</p>
          <img src={currentUser.profilePicture.startsWith('http') ? currentUser.profilePicture : `../api/Users/images/${currentUser.profilePicture}`} alt='' className='w-10 object-cover rounded-full' />
          <Link to={'/dashboard?tab=profile'} className='text-cyan-600 hover:underline'>
            @{currentUser.username}
          </Link>
        </div>
      ) : (<div className='text-teal-500 my-5 flex gap-1'>You must be signed in to comment. <Link to={'/sign-in'} className='text-blue-500 hover:underline'>Sign In</Link></div>)}

      {currentUser && (
        <form className='border border-teal-500 rounded-md p-3' onSubmit={handleSubmit}>
          <Textarea placeholder='Leave a comment...' rows='3' maxLength='200' onChange={(e) => setComment(e.target.value)} value={comment} />
          <div className='flex justify-between items-center mt-5'>
            <p className='text-gray-500 text-sm'>{200 - comment.length} characters remaining</p>
            <Button outline gradientDuoTone='purpleToBlue' type='submit'>Submit</Button>
          </div>
        </form>
      )}

      <div className="text-sm my-5 flex items-center justify-between gap-1">
        <div className='flex'>
          <FaRegCommentDots className='text-2xl' />
          <span className='ml-2'>{commentsNumber == 1 ? commentsNumber + ' Comment' : commentsNumber + ' Comments'}</span>
        </div>
        <div className='flex ml-10'>
          <button type="button" onClick={() => onLikePost(idPost)} className={`text-gray-400 hover:text-blue-500 ml-6}`}>
            <FaThumbsUp className='text-xl' />
          </button>
          <span className='ml-2'>{post.likes && post.likes.filter((like) => like.status == 1).length}</span>

          <button type="button" onClick={() => onDislikePost(idPost)} className={`text-gray-400 hover:text-red-500 ml-6}`}>
            <FaThumbsDown className='ml-5 text-xl' />
          </button>
          <span className='ml-2'>{post.likes && post.likes.filter((like) => like.status == 2).length}</span>
        </div>
      </div>

      {showErrorModal && (
        <div className={`error-modal show`}>{errorMessage}</div>
      )}

      {comments.length === 0 ? (
        <p className="text-sm my-5">No comments yet!</p>
      ) : (
        <>
          {comments.map(comment => (
            <Comment key={comment.id}
              comment={comment}
              onLike={handleLike}
              onDislike={handleDislike}
              onAddChildComment={(e, idComment, childComment) => addChildComment(e, idComment, childComment)}
              childrenComments={childComments}
              onEdit={handleEdit}
              onDelete={(commentId) => {
                setShowModal(true)
                setCommentToDelete(commentId)
              }}
              setCommentToDelete={setCommentToDelete}
              setShowModal={setShowModal}
              setActiveReplyCommentId={setActiveReplyCommentId}
              activeReplyCommentId={activeReplyCommentId}
              comments={comments}
            />
          ))}
        </>
      )}

      <Modal show={showModal} onClose={() => setShowModal(false)} popup size='md'>
        <Modal.Header />
        <Modal.Body>
          <div className="text-center">
            <HiOutlineExclamationCircle className="h-14 w-14 text-gray-400 dark:text-gray-200 mb-4 mx-auto" />
            <h3 className="mb-5 text-lg text-gray-500 dark:text-gray-400">Are you sure you want to delete this comment?</h3>

            <div className="flex justify-center gap-4">
              <Button color="failure" onClick={() => handleDelete(commentToDelete)}>
                Yes, I&apos;m sure
              </Button>
              <Button color="gray" onClick={() => setShowModal(false)}>No, cancel</Button>
            </div>
          </div>
        </Modal.Body>
      </Modal>
    </div>
  )
}
