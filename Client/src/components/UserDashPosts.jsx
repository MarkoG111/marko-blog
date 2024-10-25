import { Table, Pagination, Modal, Button } from "flowbite-react";
import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { Link } from "react-router-dom";
import { HiOutlineExclamationCircle } from 'react-icons/hi'

export default function UserDashPosts() {
  const { currentUser } = useSelector((state) => state.user)
  const [userPosts, setUserPosts] = useState('')
  const [currentPage, setCurrentPage] = useState(1)
  const [pageCount, setPageCount] = useState(1)
  const [showModal, setShowModal] = useState(false)
  const [postIdToDelete, setPostIdToDelete] = useState('')
  const [postDeleted, setPostDeleted] = useState(false)

  const [errorMessage, setErrorMessage] = useState('')
  const [showErrorModal, setShowErrorModal] = useState(false)

  const [successMessage, setSuccessMessage] = useState('')
  const [showSuccessModal, setShowSucessModal] = useState(false)

  useEffect(() => {
    if (showErrorModal) {
      setShowErrorModal(true)
    }
    if (showSuccessModal) {
      setShowSucessModal(true)
    }
    const timer = setTimeout(() => {
      setShowSucessModal(false)
      setShowErrorModal(false)
    }, 10000)

    return () => clearTimeout(timer)
  }, [showSuccessModal, showErrorModal])

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const repsonse = await fetch(`/api/Users/${currentUser.id}`, {
          method: "GET"
        })

        const data = await repsonse.json()

        if (repsonse.ok) {
          setUserPosts(data.userPosts)

          const token = localStorage.getItem("token")
          if (!token) {
            throw new Error("Token not found")
          }
        }
      } catch (error) {
        console.log(error)
      }
    }

    fetchUser()

  }, [currentUser.id])

  const onPageChange = (page) => setCurrentPage(page);

  const handleDeletePost = async () => {
    setShowModal(false)

    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const url = currentUser.id == postIdToDelete ? `/api/Posts/personal/${postIdToDelete}` : `/api/Posts/${postIdToDelete}`

      const response = await fetch(url, {
        method: "DELETE",
        headers: {
          "Authorization": `Bearer ${token}`
        },
      })

      if (!response.ok) {
        setShowErrorModal(true)
        setErrorMessage("Delete error.")
      } else {
        setUserPosts((prev) => prev.filter((post) => post.id !== postIdToDelete))
        setShowSucessModal(true)
        setSuccessMessage("You have successfully deleted post.")
        setPostDeleted(!postDeleted)
      }
    } catch (error) {
      setShowErrorModal(true)
      setErrorMessage("Delete error.")
    }
  }


  return <div className="table-container-scrollbar table-auto overflow-x-scroll md:mx-auto p-3 scrollbar scrollbar-track-slate-100 scrollbar-thumb-slate-300 dark:scrollbar-track-slate-700 dark:scrollbar-thumb-slate-500">
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
                  <Link to={`/post/${post.id}`}>
                    <img src={`api/Images/images/${post.imageName}`} alt={post.title} className="w-20 h-10 object-contain bg-gray-500" />
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
                  <span onClick={() => { setShowModal(true); setPostIdToDelete(post.id) }} className="font-medium text-red-500 hover:underline cursor-pointer">Delete</span>
                </Table.Cell>
                <Table.Cell>
                  <Link to={`/update-post/${post.id}`} className="text-teal-500 hover:underline">
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

    {showSuccessModal && (
      <div className="success-modal show">
        {successMessage}
      </div>
    )}

    {showErrorModal && (
      <div className="error-modal show">
        {errorMessage}
      </div>
    )}

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
