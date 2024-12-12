import { Button } from "flowbite-react"
import { useEffect, useState } from "react"
import { useParams } from "react-router-dom"
import { AiOutlineHeart } from "react-icons/ai";
import { useError } from "../contexts/ErrorContext";
import { handleApiError } from "../utils/handleApiUtils";

export default function UserCommentPage() {
  const { id } = useParams()
  const [comment, setComment] = useState(null)
  const [parentComment, setParentComment] = useState(null)

  const { showError } = useError()

  useEffect(() => {
    const fetchComment = async () => {
      const response = await fetch(`/comments/${id}`, {
        method: "GET"
      })

      if (response.ok) {
        const data = await response.json()
        setComment(data)

        if (data.idParent != null) {
          fetchParentComment(data.idParent);
        }
      } else {
        await handleApiError(response, showError)
      }
    }

    const fetchParentComment = async (idParent) => {
      const response = await fetch(`/comments/${idParent}`, {
        method: "GET"
      })

      if (response.ok) {
        const parentData = await response.json()
        setParentComment(parentData)
      } else {
        await handleApiError(response, showError)
      }
    }

    fetchComment()
  }, [id, showError])

  return (
    <main className="flex flex-col max-w-3xl mx-auto min-h-96 bg-slate-100 dark:bg-gray-800 my-12 rounded-2xl">
      {comment &&
        <>
          <div className="px-10 py-6">
            <p className="dark:text-gray-400 text-xl pb-3">Discussion on: <span className="text-3xl dark:text-white">{comment.postTitle}</span></p>
            <Button gradientDuoTone='purpleToPink'><a href={`/post/${comment.idPost}`}>View post</a></Button>
          </div>
          {parentComment &&
            <>
              <div className="border-t-red-400 border-t-2 px-10 py-6">
                <p className="dark:text-gray-400">Replies for: <a href={`/comment/${parentComment.id}`} className="dark:text-white">{parentComment.commentText}</a></p>
              </div>
            </>
          }
          <div className="border-t-red-400 border-t-2 px-10 pt-6">
            <div>

            </div>
            <div className="border-red-400 border-2 p-4 rounded-md">
              <div className="flex gap-x-2">
                <p>{comment.firstName} {comment.lastName}</p>
                <span>&#183;</span>
                <span>{new Date(comment.createdAt).toLocaleDateString()}</span>
              </div>
              <p className="pt-3">{comment.commentText}</p>
            </div>
          </div>

          <div className="flex px-10 pt-2 gap-2">
            <AiOutlineHeart size="22" />
            <p>{comment.likesCount == 1 ? comment.likesCount + ' like' : comment.likesCount + ' likes'}</p>
          </div>
        </>
      }
    </main>
  )
}
