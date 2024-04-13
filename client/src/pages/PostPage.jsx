import { Button, Spinner } from "flowbite-react"
import { useEffect, useState } from "react"
import { Link, useParams } from "react-router-dom"
import CommentSection from "../components/CommentSection"
<<<<<<< HEAD

=======
import Comment from "../components/Comment"
>>>>>>> 302b558e8d1e73a251f80e54cd26e042048d1532

export default function PostPage() {
  const { id } = useParams()
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState(false)
  const [post, setPost] = useState(null)

<<<<<<< HEAD
=======
  console.log(post)

>>>>>>> 302b558e8d1e73a251f80e54cd26e042048d1532
  useEffect(() => {
    const fethcPost = async () => {
      try {
        setLoading(true)
        const response = await fetch(`/api/Posts/${id}`)
        const data = await response.json()
        if (!response.ok) {
          setError(true)
          setLoading(false)
          return
        } else {
          setPost(data)
          setLoading(false)
          setError(false)
        }
      } catch (error) {
        setError(true)
        setLoading(false)
      }
    }

    fethcPost()
  }, [id])

  if (loading) return (
    <div className="flex justify-center min-h-screen">
      <Spinner size="xl" />
    </div>
  )

  return (
    <main className="p-3 flex flex-col max-w-6xl mx-auto min-h-screen">
      <h1 className="text-3xl mt-10 p-3 text-center font-serif max-w-2xl mx-auto lg:text-4xl">{post && post.title}</h1>

      {post && post.categories.map((category) => (
        <Link to={`/search?category=${category.name}`} className="self-center mt-5" key={category.id}>
          <Button color="gray" className="p-3" pill size="s">{category.name}</Button>
        </Link>
      ))}

      <img src={post && `/api/Images/images/${post.imageName}`} alt={post && post.title} className="mt-10 p-3 max-h-[500px] w-full object-cover" />

      <div className="flex justify-between p-3 border-b border-slate-500 mx-auto w-full max-w-2xl">
        <span>{post && new Date(post.dateCreated).toLocaleDateString()}</span>
        <span className="italic">{post && (post.content.length / 1000).toFixed(0)} mins read</span>
      </div>

      <div className="p-3 max-w-2xl mx-auto w-full post-content" dangerouslySetInnerHTML={{ __html: post && post.content }}>

      </div>
<<<<<<< HEAD
      
      <CommentSection idPost={post.id} />
=======

      <CommentSection idPost={post.id} />

      {post.comments.length === 0 ? (
        <p className="text-sm my-5">No comments yet!</p>
      ) : (
        <>
          <div className="text-sm my-5 flex items-center gap-1">
            <p>Comments</p>
            <div className="border border-gray-400 py-1 px-2 rounded-sm">
              <p>{post.comments.length}</p>
            </div>
          </div>

          {post.comments.map(comment => (
            <Comment key={comment.id} comment={comment} />
          ))}
        </>
      )}


>>>>>>> 302b558e8d1e73a251f80e54cd26e042048d1532
    </main>
  )
}