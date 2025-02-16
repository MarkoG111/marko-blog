import { useEffect, useState } from "react"
import { useSelector } from "react-redux"
import { Link } from "react-router-dom"
import { Table, Pagination, Modal, Button } from "flowbite-react"
import { HiOutlineExclamationCircle } from 'react-icons/hi'
import { useError } from "../contexts/ErrorContext"
import { useSuccess } from "../contexts/SuccessContext"
import { handleApiError } from "../utils/handleApiUtils"

export default function UserDashPosts() {
  const { currentUser } = useSelector((state) => state.user)

  const [userPosts, setUserPosts] = useState('')
  const [currentPage, setCurrentPage] = useState(1)
  const [pageCount, setPageCount] = useState(1)
  const [showModal, setShowModal] = useState(false)
  const [idPostToDelete, setIdPostToDelete] = useState('')
  const [postDeleted, setPostDeleted] = useState(false)

  const { showError } = useError()
  const { showSuccess } = useSuccess()

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const repsonse = await fetch(`/api/users/${currentUser.id}`, {
          method: "GET"
        })

        const data = await repsonse.json()

        if (repsonse.ok) {
          setUserPosts(data.userPosts)

          const token = localStorage.getItem("token")
          if (!token) {
            showError("Token not found")
            return
          }
        } else {
          await handleApiError(repsonse, showError)
        }
      } catch (error) {
        showError(error.message)
      }
    }

    fetchUser()
  }, [currentUser.id, showError])

  const onPageChange = (page) => setCurrentPage(page)

  const handleDeletePost = async () => {
    setShowModal(false)

    try {
      const token = localStorage.getItem("token")
      if (!token) {
        showError("Token not found")
        return
      }

      const postToDelete = userPosts.find((post) => post.id === idPostToDelete)
      if (!postToDelete) {
        showError("Post not found")
        return
      }

      const { idUser } = postToDelete

      const url = currentUser.id === idUser ? `/api/posts/${idPostToDelete}/personal` : `/api/posts/${idPostToDelete}`

      const response = await fetch(url, {
        method: "DELETE",
        headers: {
          "Authorization": `Bearer ${token}`
        },
      })

      if (response.ok) {
        setUserPosts((prev) => prev.filter((post) => post.id !== idPostToDelete))
        showSuccess("You have successfully deleted a post")
        setPostDeleted(!postDeleted)
      } else {
        await handleApiError(response, showError)
      }
    } catch (error) {
      showError(error.message)
    }
  }

  return <div className="table-container-scrollbar table-auto overflow-x-scroll md:mx-auto p-3 scrollbar scrollbar-track-slate-100 scrollbar-thumb-slate-300 dark:scrollbar-track-slate-700 dark:scrollbar-thumb-slate-500">
    <h1 className='mb-4 font-bold text-xl text-center my-3'>
      Posts
    </h1>
    {currentUser.roleName === 'Author' && userPosts.length > 0 ? (
      <>
        <Table hoverable className="shadow-md my-8">
          <Table.Head>
            <Table.HeadCell>Date updated</Table.HeadCell>
            <Table.HeadCell>Post image</Table.HeadCell>
            <Table.HeadCell>Post title</Table.HeadCell>
            <Table.HeadCell>Category</Table.HeadCell>
            <Table.HeadCell>Delete</Table.HeadCell>
            <Table.HeadCell>
              <span>Edit</span>
            </Table.HeadCell>
          </Table.Head>

          {userPosts.map((post) => (
            <Table.Body key={post.id} className="divide-y">
              <Table.Row className="bg-white dark:border-gray-700 dark:bg-gray-800">
                <Table.Cell>{new Date(post.dateCreated).toLocaleDateString()}</Table.Cell>
                <Table.Cell>
                  <Link to={`/api/post/${post.id}`}>
                    <img src={`/api/images/${post.imageName}`} alt={post.title} className="w-20 h-10 object-contain bg-gray-500" />
                  </Link>
                </Table.Cell>
                <Table.Cell>
                  <Link to={`/post/${post.id}`} className="font-medium text-gray-900 dark:text-white">
                    {post.title}
                  </Link>
                </Table.Cell>
                <Table.Cell>
                  {post.categories.map((category, index) => (
                    <span key={category.id}>{category.name} {index !== post.categories.length - 1 && ', '} </span>
                  ))}
                </Table.Cell>
                <Table.Cell>
                  <span onClick={() => { setShowModal(true); setIdPostToDelete(post.id) }} className="font-medium text-red-500 hover:underline cursor-pointer">Delete</span>
                </Table.Cell>
                <Table.Cell>
                  <Link to={`/api/update-post/${post.id}`} className="text-teal-500 hover:underline">
                    <span>Edit</span>
                  </Link>
                </Table.Cell>
              </Table.Row>
            </Table.Body>
          ))}
        </Table>

        <Pagination
          currentPage={currentPage}
          onPageChange={onPageChange}
          totalPages={pageCount}
          className="pb-6"
        />
      </>
    ) : (<p>You have no posts</p>)
    }

    <Modal show={showModal} onClose={() => setShowModal(false)} popup size='md'>
      <Modal.Header />
      <Modal.Body>
        <div className="text-center">
          <HiOutlineExclamationCircle className="h-14 w-14 text-gray-400 dark:text-gray-200 mb-4 mx-auto" />
          <h3 className="mb-5 text-lg text-gray-500 dark:text-gray-400">Are you sure you want to delete this post?</h3>

          <div className="flex justify-center gap-4">
            <Button color="failure" onClick={handleDeletePost}>
              Yes, I&apos;m sure
            </Button>
            <Button color="gray" onClick={() => setShowModal(false)}>No, cancel</Button>
          </div>
        </div>
      </Modal.Body>
    </Modal>
  </div >
}
