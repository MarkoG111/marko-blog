import { Button, Spinner } from "flowbite-react"
import { useEffect, useState } from "react"
import { Link, useParams } from "react-router-dom"
import CommentSection from "../components/CommentSection"

export default function PostPage() {
  const { id } = useParams()
  const [loading, setLoading] = useState(true)
  const [post, setPost] = useState(null)

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

      <CommentSection idPost={post.id} childrenComments={post.comments.filter(comment => comment.children.length > 0).flatMap(comment => comment.children)} />
    </main>
  )
}