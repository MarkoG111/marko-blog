import { useEffect, useState } from "react"
import { useSelector } from "react-redux"
import { Table, Pagination } from "flowbite-react"
import { useError } from "../contexts/ErrorContext"
import { handleApiError } from "../utils/handleApiUtils"
import EditableCategoryRow from "./EditableCategoryRow"

export default function DashCategories() {
  const { currentUser } = useSelector((state) => state.user)
  const [categories, setCategories] = useState([])
  const [currentPage, setCurrentPage] = useState(1)
  const [pageCount, setPageCount] = useState(1)

  const { showError } = useError()

  useEffect(() => {
    const fetchAdminCategories = async () => {
      try {
        const queryParams = new URLSearchParams({
          perPage: 12,
          page: currentPage
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
        
        if (response.ok) {
          const data = await response.json()
          
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
  }, [currentPage, showError])

  const onPageChange = (page) => setCurrentPage(page)

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
                  onDelete={(id) => setCategories((prev) => prev.filter((cat) => cat.id !== id))}
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
  </div>
}
