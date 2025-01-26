import { useState } from "react"
import { useError } from "../contexts/ErrorContext"
import { useSuccess } from "../contexts/SuccessContext"
import { handleApiError } from "../utils/handleApiUtils"
import { Button, TextInput } from "flowbite-react"

export default function CreateCategory() {
  const { showError } = useError()
  const { showSuccess } = useSuccess()

  const handleSubmit = async (e) => {
    e.preventDefault()

    const postData = {
      Name: e.target.elements.name.value,
    }

    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const response = await fetch('/categories', {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: JSON.stringify(postData)
      })

      if (response.ok) {
        showSuccess("You have successfully added a category")

        e.target.elements.name.value = ''
      } else {
        await handleApiError(response, showError)
      }
    } catch (error) {
      showError(error.message)
    }
  }

  return (
    <div className="p-3 max-w-3xl mx-auto min-h-screen">
      <h1 className="text-center text-3xl my-7 font-semibold">Create Category</h1>

      <form className="flex flex-col gap-4" onSubmit={handleSubmit}>
        <div className="flex flex-col gap-4 sm:flex-row justify-between">
          <TextInput type="text" placeholder="Category Name" required id="name" className="flex-1" />
        </div>

        <Button type="submit" gradientDuoTone="purpleToPink" className="mt-4">Publish</Button>
      </form>
    </div>
  )
}
