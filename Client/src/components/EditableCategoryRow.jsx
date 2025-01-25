import { useState } from "react"
import { useError } from "../contexts/ErrorContext"
import { useSuccess } from "../contexts/SuccessContext"
import { handleApiError } from "../utils/handleApiUtils"
import { Table } from "flowbite-react"
import { Link } from "react-router-dom"
import PropTypes from "prop-types"
import { HiOutlineCheck, HiOutlineX } from "react-icons/hi"

export default function EditableCategoryRow({ category, onSave }) {
  const [isEditing, setIsEditing] = useState(false)
  const [newName, setNewName] = useState(category.name)

  const { showError } = useError()
  const { showSuccess } = useSuccess()

  const handleSave = async () => {
    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const response = await fetch(`/categories/${category.id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify({
          name: newName
        })
      })

      if (response.ok) {
        onSave(category.id, newName)
        setIsEditing(false)
        showSuccess('Category name updated successfully')
      } else {
        await handleApiError(response, showError)
      }

    } catch (error) {
      showError(error.message)
    }
  }

  return (
    <Table.Row className="bg-white dark:border-gray-700 dark:bg-gray-800">
      <Table.Cell>{new Date(category.createdAt).toLocaleDateString()}</Table.Cell>

      <Table.Cell>
        {isEditing ? (
          <div className="flex items-center">
            <input type="text" value={newName} onChange={(e) => setNewName(e.target.value)} className="p-2 border rounded-md w-full" />
            <button onClick={handleSave} className="ml-2 text-green-500 hover:underline"><HiOutlineCheck className="text-2xl" /></button>
            <button onClick={() => {
              setIsEditing(false)
              setNewName(category.name)
            }}
              className="ml-2 text-red-500 hover:underline"
            >
              <HiOutlineX className="text-2xl" />
            </button>
          </div>
        ) : (<span onClick={() => setIsEditing(true)} className="font-medium text-gray-900 dark:text-white cursor-pointer">{category.name}</span>)}
      </Table.Cell>

      <Table.Cell>
        <span onClick={() => { /* handle delete */ }} className="font-medium text-red-500 hover:underline cursor-pointer">Delete</span>
      </Table.Cell>

      <Table.Cell>
        <Link className="text-teal-500 hover:underline">
          <span onClick={() => setIsEditing(true)}>Change Name</span>
        </Link>
      </Table.Cell>
    </Table.Row>
  )
}

EditableCategoryRow.propTypes = {
  category: PropTypes.shape({
    id: PropTypes.oneOfType([PropTypes.string, PropTypes.number]).isRequired,
    name: PropTypes.string.isRequired,
    createdAt: PropTypes.string.isRequired,
  }).isRequired,
  onSave: PropTypes.func.isRequired,
};