<<<<<<< HEAD
/* eslint-disable react/prop-types */

import { Button, Textarea } from 'flowbite-react'
import { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import { Link, useNavigate } from 'react-router-dom'
import Comment from './Comment'
=======
import { Button, Textarea } from 'flowbite-react'
import { useEffect, useState } from 'react'
import { useSelector } from 'react-redux'
import { Link } from 'react-router-dom'
>>>>>>> 302b558e8d1e73a251f80e54cd26e042048d1532

export default function CommentSection({ idPost }) {
  const { currentUser } = useSelector(state => state.user)
  const [comment, setComment] = useState('')
  const [comments, setComments] = useState([])
<<<<<<< HEAD
  const navigate = useNavigate()
=======
>>>>>>> 302b558e8d1e73a251f80e54cd26e042048d1532

  console.log(comments);

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

<<<<<<< HEAD
      const body = JSON.stringify({
        CommentText: comment,
        IdPost: idPost,
        IdUser: currentUser.id
      })

=======
>>>>>>> 302b558e8d1e73a251f80e54cd26e042048d1532
      const response = await fetch(`/api/Comments`, {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
<<<<<<< HEAD
        body: body
=======
        body: JSON.stringify({ Text: comment, IdPost: idPost, IdUser: currentUser.id })
>>>>>>> 302b558e8d1e73a251f80e54cd26e042048d1532
      })

      const data = await response.json()
      if (response.ok) {
        setComment('')
<<<<<<< HEAD
        setComments([data, ...comments])
=======
>>>>>>> 302b558e8d1e73a251f80e54cd26e042048d1532
      }
    } catch (error) {
      console.log(error)
    }
  }

  useEffect(() => {
    const getComments = async () => {
      try {
        const token = localStorage.getItem("token")
        if (!token) {
          throw new Error("Token not found")
        }

        const response = await fetch(`/api/Comments`, {
          method: "GET",
          headers: {
            "Authorization": `Bearer ${token}`,
            "Content-Type": "application/json"
          },
        })
        if (response.ok) {
          const data = await response.json()
<<<<<<< HEAD
          setComments(data.items)
=======
          setComments(data)
>>>>>>> 302b558e8d1e73a251f80e54cd26e042048d1532
        }
      } catch (error) {
        console.log(error)
      }
    }

    getComments()
  }, [idPost])

<<<<<<< HEAD


=======
>>>>>>> 302b558e8d1e73a251f80e54cd26e042048d1532
  return (
    <div className='max-w-2xl mx-auto w-full p-3'>
      {currentUser ? (
        <div className='flex items-center gap-1 my-5 text-gray-500 text-sm'>
          <p>Signed in as:</p>
<<<<<<< HEAD
          <img src={currentUser.profilePicture.startsWith('http') ? currentUser.profilePicture : `api/Users/images/${currentUser.profilePicture}`} alt='' className='w-10 object-cover rounded-full' />
=======
          <img src={`/api/Users/images/${currentUser.profilePicture}`} alt='' className='w-10 object-cover rounded-full' />
>>>>>>> 302b558e8d1e73a251f80e54cd26e042048d1532
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
<<<<<<< HEAD

      {comments.length === 0 ? (
        <p className="text-sm my-5">No comments yet!</p>
      ) : (
        <>
          <div className="text-sm my-5 flex items-center gap-1">
            <p>Comments</p>
            <div className="border border-gray-400 py-1 px-2 rounded-sm">
              <p>{comments.length}</p>
            </div>
          </div>

          {comments.map(comment => (
            <Comment key={comment.id} comment={comment} />
          ))}
        </>
      )}

=======
>>>>>>> 302b558e8d1e73a251f80e54cd26e042048d1532
    </div>
  )
}
