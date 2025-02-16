import { useEffect, useState } from "react"
import { useSelector } from "react-redux"
import { useParams } from "react-router-dom"
import { Button, Checkbox, FileInput, TextInput } from "flowbite-react"
import { useError } from "../contexts/ErrorContext"
import { useSuccess } from "../contexts/SuccessContext"
import { handleApiError } from "../utils/handleApiUtils"
import ReactQuill from 'react-quill'
import 'react-quill/dist/quill.snow.css'

export default function UpdatePost() {
  const [selectedCategories, setSelectedCategories] = useState([])
  const [categories, setCategories] = useState([])

  const [imageFile, setImageFile] = useState(null)
  const [imagePreview, setImagePreview] = useState(null)
  const [content, setEditContent] = useState('')
  const [editData, setEditData] = useState({})

  const { currentUser } = useSelector((state) => state.user)

  const { postId } = useParams()

  const { showError } = useError()
  const { showSuccess } = useSuccess()

  useEffect(() => {
    try {
      const fetchPost = async () => {
        const token = localStorage.getItem("token")
        if (!token) {
          throw new Error("Token not found")
        }

        try {
          const response = await fetch(`/api/posts/${postId}`, {
            method: "GET",
            headers: {
              "Authorization": `Bearer ${token}`
            }
          })

          if (response.ok) {
            const data = await response.json()

            setEditData(data)
          } else {
            await handleApiError(response, showError)
          }
        } catch (error) {
          showError(error.message)
        }
      }

      fetchPost()
    } catch (error) {
      showError(error.message)
    }
  }, [postId, showError])

  const handleContentEditPostChange = (value) => {
    setEditContent(value)
  }

  const handleCategoryChange = (idCategory) => {
    setSelectedCategories(prevCategories => {
      if (prevCategories.includes(idCategory)) {
        return prevCategories.filter(id => id !== idCategory)
      } else {
        return [...prevCategories, idCategory]
      }
    })
  }

  useEffect(() => {
    const fetchCategoriesForUpdatePost = async () => {
      try {
        const token = localStorage.getItem("token")
        if (!token) {
          throw new Error("Token not found")
        }

        const queryParams = new URLSearchParams({
          getAll: true
        })

        const response = await fetch(`/api/categories?${queryParams}`, {
          method: "GET",
          headers: {
            "Authorization": `Bearer ${token}`
          }
        })

        if (response.ok) {
          const data = await response.json()

          setCategories(data.items)
        } else {
          await handleApiError(response, showError)
        }
      } catch (error) {
        showError(error.message)
      }
    }

    fetchCategoriesForUpdatePost()
  }, [showError])

  useEffect(() => {
    if (editData.categories && editData.categories.length > 0) {
      const initialSelectedCategories = editData.categories.map(category => category.id)
      setSelectedCategories(initialSelectedCategories)
    }
  }, [editData])

  const handleUploadImage = async () => {
    if (!imageFile) {
      showError("Please select an image")
      return
    }

    const formData = new FormData()
    formData.append("Image", imageFile)

    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const response = await fetch(`/images`, {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`
        },
        body: formData
      })

      if (response.ok) {
        const imageUrl = await response.json()
        
        setImagePreview(imageUrl)
      } else {
        await handleApiError(response, showError)
      }
    } catch (error) {
      showError("Image upload failed")
    }
  }

  const handleSubmit = async (e) => {
    e.preventDefault()

    const postData = {
      Title: e.target.elements.title.value,
      Content: content ? content : editData.content,
      IdImage: imagePreview?.id ? imagePreview?.id : editData.idImage,
      CategoryIds: selectedCategories
    }

    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const url = currentUser.id == editData.idUser ? `/posts/${editData.id}/personal` : `/posts/${editData.id}`

      const response = await fetch(url, {
        method: "PUT",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: JSON.stringify(postData)
      })

      if (response.ok) {
        if (response.status == 204) {
          showSuccess('Post updated successfully')
        }
      } else {
        await handleApiError(response, showError)
      }
    } catch (error) {
      showError('Cannot update post')
    }
  }

  return (
    <div className="p-3 max-w-3xl mx-auto min-h-screen">
      <h1 className="text-center text-3xl my-7 font-semibold">Update post</h1>

      <form className="flex flex-col gap-4" onSubmit={handleSubmit}>
        <div className="flex flex-col gap-4 sm:flex-row justify-between">
          <TextInput type="text" placeholder="Title" required id="title" className="flex-1" defaultValue={editData.title || ''} />
        </div>

        <div className="flex flex-wrap gap-x-9 mb-6 gap-y-3">
          <h3 className="font-semibold w-full mt-5">Choose Categories</h3>
          {categories.map((category, index) => (
            <div key={index} className="flex items-center">
              <Checkbox
                checked={selectedCategories.includes(category.id)}
                onChange={() => handleCategoryChange(category.id)}
              />
              <label className="ml-2">{category.name}</label>
            </div>
          ))}
        </div>

        <div className="flex gap-4 items-center justify-between border-4 border-teal-500 border-dotted p-3 mb-6 flex-wrap rounded-xl">
          <FileInput type="file" accept="image/*" onChange={(e) => setImageFile(e.target.files[0])} />
          <Button type="button" gradientDuoTone="purpleToBlue" size="sm" outline onClick={handleUploadImage}>Upload Image</Button>

          {editData.imageName && !imagePreview && (
            <div>
              <img src={`/images/${editData.imageName}`} alt="Uploaded" className="max-w-full h-auto mb-4" />
            </div>
          )}

          {imagePreview && (
            <div>
              <img src={`/images/${imagePreview.imagePath}`} alt="Uploaded" className="max-w-full h-auto mb-4" />
            </div>
          )}
        </div>

        <ReactQuill
          theme="snow"
          placeholder="Write something..."
          id="contentEdit"
          className="h-72 mb-12"
          value={content ? content : editData.content}
          onChange={handleContentEditPostChange}
          required
        />

        <Button type="submit" gradientDuoTone="purpleToPink">Update post</Button>
      </form>
    </div>
  )
}
