import { Button, Spinner } from "flowbite-react"
import { useEffect, useState } from "react"
import { Link, useParams } from "react-router-dom"
import CommentSection from "../components/CommentSection"
import PostLikeButtons from "../components/PostLikeButtons"
import { useSelector } from "react-redux"

import {
  removeDislikeOrLikeIfPresentInPost,
  checkIfAlreadyVotedOnPost,
  updatePostLikes

} from '../utils/postLikeUtils'

export default function PostPage() {
  const { id } = useParams()

  const [loading, setLoading] = useState(true)
  const [post, setPost] = useState(null)
  const [errorMessage, setErrorMessage] = useState('')
  const [showErrorModal, setShowErrorModal] = useState(false)

  const { currentUser } = useSelector(state => state.user)

  useEffect(() => {
    const fethcPost = async () => {
      try {
        setLoading(true)

        const response = await fetch(`/api/Posts/${id}`, {
          method: "GET"
        })

        if (!response.ok) {
          setLoading(true)
          return
        } else {
          const data = await response.json()
          setPost(data)
          setLoading(false)
        }
      } catch (error) {
        setLoading(true)
      }
    }

    fethcPost()
  }, [id])

  const handleError = (message) => {
    setErrorMessage(message)
    setShowErrorModal(true)
    setTimeout(() => setShowErrorModal(false), 10000)
  }

  const onLikePost = async (idPost) => {
    try {
      const token = localStorage.getItem("token")
      if (!token) {
        handleError("You must be logged in to like a post.")
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
        handleError(errorData.message)
      }
    } catch (error) {
      handleError("An error occurred while processing your request.")
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
        handleError(errorData.message)
      }
    } catch (error) {
      handleError("An error occurred while processing your request.")
    }
  }


  if (loading) return (
    <div className="flex flex-col justify-center text-center min-h-screen">
      <Spinner size="xl" />
    </div>
  )

  return (
    <main className="p-3 flex flex-col max-w-6xl mx-auto min-h-screen bg-slate-100 dark:bg-gray-800 my-12 rounded-2xl">
      <h1 className="text-3xl mt-2 p-3 text-center font-serif max-w-2xl mx-auto lg:text-4xl">{post && post.title}</h1>

      <div className="flex justify-center">
        {post && post.categories.map((category) => (
          <Link to={`/category/${category.id}`} className="self-center mt-5 ml-6" key={category.id}>
            <Button color="gray" className="p-3" pill size="s">{category.name}</Button>
          </Link>
        ))}
      </div>

      <div className="flex justify-center mt-6">
        <p className="text-xl">Author: {post.username}</p>
      </div>

      <img src={post && `/api/Images/images/${post.imageName}`} alt={post && post.title} className="mt-10 p-3 max-h-[300px] w-full object-contain" />

      <div className="flex justify-between p-3 border-b border-slate-500 mx-auto w-full max-w-2xl">
        <span>{post && new Date(post.dateCreated).toLocaleDateString()}</span>
        <span className="italic">{post && (post.content.length / 1000).toFixed(0)} mins read</span>
      </div>

      <div className="p-3 max-w-2xl mx-auto w-full post-content" dangerouslySetInnerHTML={{ __html: post && post.content }}>

      </div>

      <div className="px-4 max-w-2xl mx-auto w-full">
        {/* Use the PostLikeButtons component here */}
        <PostLikeButtons
          post={post}
          idPost={post.id}
          onLikePost={onLikePost}
          onDislikePost={onDislikePost}
          commentsNumber={post.comments.length}
        />
      </div>


      <CommentSection idPost={post.id} childrenComments={post.comments.filter(comment => comment.children.length > 0).flatMap(comment => comment.children)} />
    </main>
  )
}