import { useEffect, useState } from "react"
import { useSelector } from "react-redux"
import { HiOutlineExclamationCircle } from 'react-icons/hi'
import { Table, Pagination, Modal, Button } from "flowbite-react"
import { useError } from "../contexts/ErrorContext"
import { useSuccess } from "../contexts/SuccessContext"
import { handleApiError } from "../utils/handleApiUtils"
import EditableCategoryRow from "./EditableCategoryRow"

export default function DashCategories() {
  const { currentUser } = useSelector((state) => state.user)
  const [categories, setCategories] = useState([])
  const [currentPage, setCurrentPage] = useState(1)
  const [pageCount, setPageCount] = useState(1)
  const [showModal, setShowModal] = useState(false)
  const [categoryIdToDelete, setCategoryIdToDelete] = useState('')
  const [categoryDeleted, setCategoryDeleted] = useState(false)

  const { showError } = useError()
  const { showSuccess } = useSuccess()


  useEffect(() => {
    const fetchAdminCategories = async () => {
      try {
        const queryParams = new URLSearchParams({
          perPage: 12,
          page: currentPage,
          includeCreatedAt: true
        })

        const token = localStorage.getItem("token")
        if (!token) {
          throw new Error("Token not found")
        }

        const response = await fetch(`/categories?${queryParams}`, {
          method: "GET",
          headers: {
            "Authorization": `Bearer ${localStorage.getItem("token")}`
          }
        })
        const data = await response.json()

        if (response.ok) {
          setCategories(data.items)
          setPageCount(data.pageCount)
        } else {
          await handleApiError(response, showError)
        }
      } catch (error) {
        showError(error.message)
      }
    }

    fetchAdminCategories()
  }, [currentPage, categoryDeleted, showError])

  const onPageChange = (page) => setCurrentPage(page)

  const handleDeleteCategory = async () => {
    setShowModal(false)

    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const response = await fetch(`/categories/${categoryIdToDelete}`, {
        method: "DELETE",
        headers: {
          "Authorization": `Bearer ${token}`
        },
      })

      if (response.ok) {
        setCategories((prev) => prev.filter((category) => category.id !== categoryIdToDelete))
        showSuccess("You have successfully deleted a category")
        setCategoryDeleted(!categoryDeleted)
      } else {
        await handleApiError(response, showError)
      }
    } catch (error) {
      showError(error.message)
    }
  }

  const handleSaveCategoryName = (id, updatedName) => {
    setCategories((prev) => prev.map((category) => category.id === id ? { ...category, name: updatedName } : category))
  }

  return <div className="table-container-scrollbar table-auto overflow-x-scroll md:mx-auto p-3 scrollbar scrollbar-track-slate-100 scrollbar-thumb-slate-300 dark:scrollbar-track-slate-700 dark:scrollbar-thumb-slate-500">
    {currentUser.roleName === 'Admin' && categories.length > 0 ? (
      <>
        <Table hoverable className="shadow-md my-8">
          <Table.Head>
            <Table.HeadCell>Date updated</Table.HeadCell>
            <Table.HeadCell>Category name</Table.HeadCell>
            <Table.HeadCell>Delete</Table.HeadCell>
            <Table.HeadCell>
              <span>Edit</span>
            </Table.HeadCell>
          </Table.Head>

          {categories.length === 0 ? (
            <p>No categories found</p>
          ) : (
            <Table.Body>
              {categories.map((category) => (
                <EditableCategoryRow
                  key={category.id}
                  category={category}
                  onSave={handleSaveCategoryName}
                />
              ))}
            </Table.Body>
          )}
        </Table>

        <Pagination
          currentPage={currentPage}
          onPageChange={onPageChange}
          totalPages={pageCount}
          className="pb-6"
        />
      </>
    ) : (<p>You have no categories</p>)
    }

    <Modal show={showModal} onClose={() => setShowModal(false)} popup size='md'>
      <Modal.Header />
      <Modal.Body>
        <div className="text-center">
          <HiOutlineExclamationCircle className="h-14 w-14 text-gray-400 dark:text-gray-200 mb-4 mx-auto" />
          <h3 className="mb-5 text-lg text-gray-500 dark:text-gray-400">Are you sure you want to delete this category?</h3>

          <div className="flex justify-center gap-4">
            <Button color="failure" onClick={handleDeleteCategory}>
              Yes, I&apos;m sure
            </Button>
            <Button color="gray" onClick={() => setShowModal(false)}>No, cancel</Button>
          </div>
        </div>
      </Modal.Body>
    </Modal>
  </div >
}
