import { Table, Pagination, Modal, Button } from "flowbite-react";
import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { Link } from "react-router-dom";
import { HiOutlineExclamationCircle } from 'react-icons/hi'
import { useError } from "../contexts/ErrorContext";

export default function DashComments() {
  const { currentUser } = useSelector((state) => state.user)
  const [comments, setComments] = useState([])
  const [currentPage, setCurrentPage] = useState(1)
  const [pageCount, setPageCount] = useState(1)
  const [showModal, setShowModal] = useState(false)
  const [commentIdToDelete, setCommentIdToDelete] = useState('')

  const [successMessage, setSuccessMessage] = useState('')
  const [showSuccessModal, setShowSucessModal] = useState(false)

  const { showError } = useError()

  useEffect(() => {
    if (showSuccessModal) {
      setShowSucessModal(true)
    }
    const timer = setTimeout(() => {
      setShowSucessModal(false)
    }, 10000)

    return () => clearTimeout(timer)
  }, [showSuccessModal])

  useEffect(() => {
    const fetchComments = async () => {
      try {
        const token = localStorage.getItem("token")
        if (!token) {
          throw new Error("Token not found")
        }

        const response = await fetch(`/api/Comments?page=${currentPage}`, {
          method: "GET",
          headers: {
            "Authorization": `Bearer ${token}`
          }
        })

        const data = await response.json()

        if (response.ok) {
          setComments(data.items)
          setPageCount(data.pageCount)
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

    fetchComments()
  }, [currentPage, showError])

  const onPageChange = (page) => setCurrentPage(page);

  const handleDeleteComment = async () => {
    setShowModal(false)

    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const response = await fetch(`api/Comments/${commentIdToDelete}`, {
        method: "DELETE",
        headers: {
          "Authorization": `Bearer ${token}`
        },
      })

      if (!response.ok) {
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
      } else {
        setComments((prev) => prev.filter((comment) => comment.id !== commentIdToDelete))
        setShowModal(false)

        setShowSucessModal(true)
        setSuccessMessage("You have successfully deleted comment.")
      }
    } catch (error) {
      showError(error.message);
    }
  }

  return <div className="table-container-scrollbar table-auto overflow-x-scroll md:mx-auto p-3 scrollbar scrollbar-track-slate-100 scrollbar-thumb-slate-300 dark:scrollbar-track-slate-700 dark:scrollbar-thumb-slate-500">
    {currentUser.roleName === 'Admin' && comments.length > 0 ? (
      <>
        <Table hoverable className="shadow-md my-8">
          <Table.Head>
            <Table.HeadCell>Date created</Table.HeadCell>
            <Table.HeadCell>Comment content</Table.HeadCell>
            <Table.HeadCell>Number of likes</Table.HeadCell>
            <Table.HeadCell>Post title</Table.HeadCell>
            <Table.HeadCell>User</Table.HeadCell>
            <Table.HeadCell>Delete</Table.HeadCell>
          </Table.Head>

          {comments.map((comment) => (
            <Table.Body key={comment.id} className="divide-y">
              <Table.Row className="bg-white dark:border-gray-700 dark:bg-gray-800">
                <Table.Cell>
                  {new Date(comment.createdAt).toLocaleDateString()}
                </Table.Cell>
                <Table.Cell>
                  {comment.commentText.length < 35 ? comment.commentText : comment.commentText.substr(0, 35) + ' ...'}
                </Table.Cell>
                <Table.Cell>
                  {comment.likesCount}
                </Table.Cell>
                <Table.Cell>
                  <Link to={`/post/${comment.idPost}`}>
                    {comment.postTitle}
                  </Link>
                </Table.Cell>
                <Table.Cell>
                  {comment.username}
                </Table.Cell>
                <Table.Cell>
                  <span onClick={() => { setShowModal(true); setCommentIdToDelete(comment.id) }} className="font-medium text-red-500 hover:underline cursor-pointer">Delete</span>
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
    ) : (<p>No comments</p>)
    }

    {showSuccessModal && (
      <div className="success-modal show">
        {successMessage}
      </div>
    )}

    <Modal show={showModal} onClose={() => setShowModal(false)} popup size='md'>
      <Modal.Header />
      <Modal.Body>
        <div className="text-center">
          <HiOutlineExclamationCircle className="h-14 w-14 text-gray-400 dark:text-gray-200 mb-4 mx-auto" />
          <h3 className="mb-5 text-lg text-gray-500 dark:text-gray-400">Are you sure you want to delete this comment?</h3>

          <div className="flex justify-center gap-4">
            <Button color="failure" onClick={handleDeleteComment}>
              Yes, I&apos;m sure
            </Button>
            <Button color="gray" onClick={() => setShowModal(false)}>No, cancel</Button>
          </div>
        </div>
      </Modal.Body>
    </Modal>
  </div>
}
