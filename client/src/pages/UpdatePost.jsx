import { Alert, Button, Checkbox, FileInput, TextInput } from "flowbite-react";
import { useEffect, useState } from "react";
import ReactQuill from 'react-quill';
import 'react-quill/dist/quill.snow.css';
import { useSelector } from "react-redux";
import { useParams } from "react-router-dom";

export default function UpdatePost() {
  const [selectedCategories, setSelectedCategories] = useState([])
  const [categories, setCategories] = useState([])

  const [imageFile, setImageFile] = useState(null)
  const [imageUploadError, setImageUploadError] = useState(null)
  const [imagePreview, setImagePreview] = useState(null)
  const [content, setEditContent] = useState('')
  const [editData, setEditData] = useState({})

  const [errorMessage, setErrorMessage] = useState('')
  const [errorMessages, setErrorMessages] = useState([])
  const [showErrorModal, setShowErrorModal] = useState(false)
  const [showErrorsModal, setShowErrorsModal] = useState(false)
  const [successMessage, setSuccessMessage] = useState('')
  const [showSuccessModal, setShowSucessModal] = useState(false)

  const { currentUser } = useSelector((state) => state.user)

  const { postId } = useParams()

  useEffect(() => {
    if (showErrorModal) {
      setShowErrorModal(true)
    }
    if (showErrorsModal) {
      setShowErrorsModal(true)
    }
    if (showSuccessModal) {
      setShowSucessModal(true)
    }
    const timer = setTimeout(() => {
      setShowErrorModal(false)
      setShowErrorsModal(false)
      setShowSucessModal(false)
    }, 10000)

    return () => clearTimeout(timer)
  }, [showErrorModal, showErrorsModal, showSuccessModal])

  useEffect(() => {
    try {
      const fetchPost = async () => {
        const token = localStorage.getItem("token")
        if (!token) {
          throw new Error("Token not found")
        }

        try {
          const response = await fetch(`/api/Posts/${postId}`, {
            method: "GET",
            headers: {
              "Authorization": `Bearer ${token}`
            }
          });

          const data = await response.json()

          if (!response.ok) {
            throw new Error();
          } else {
            setEditData(data);
          }
        } catch (error) {
          console.log(error)
        }
      }
      fetchPost()
    } catch (error) {
      console.log(error)
    }
  }, [postId])

  const handleContentEditPostChange = (value) => {
    setEditContent(value);
  };

  const handleCategoryChange = (idCategory) => {
    setSelectedCategories(prevCategories => {
      if (prevCategories.includes(idCategory)) {
        return prevCategories.filter(id => id !== idCategory)
      } else {
        return [...prevCategories, idCategory]
      }
    });
  };

  useEffect(() => {
    const fetchCategoriesForUpdatePost = async () => {
      try {
        const token = localStorage.getItem("token")
        if (!token) {
          throw new Error("Token not found")
        }

        const response = await fetch(`/api/Categories`, {
          method: "GET",
          headers: {
            "Authorization": `Bearer ${token}`
          }
        })

        if (response.ok) {
          const data = await response.json()
          setCategories(data.items)
        }
      } catch (error) {
        console.log(error)
      }
    }

    fetchCategoriesForUpdatePost()
  }, [])

  useEffect(() => {
    if (editData.categories && editData.categories.length > 0) {
      const initialSelectedCategories = editData.categories.map(category => category.id);
      setSelectedCategories(initialSelectedCategories);
    }
  }, [editData]);

  const handleUploadImage = async () => {
    if (!imageFile) {
      setImageUploadError("Please select an image")
      return
    }

    const formData = new FormData()
    formData.append("Image", imageFile)

    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const response = await fetch(`/api/Images`, {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`
        },
        body: formData
      });

      const imageUrl = await response.json()
      setImagePreview(imageUrl)
    } catch (error) {
      setImageUploadError("Image upload failed")
    }
  }

  const handleSubmit = async (e) => {
    e.preventDefault()

    const postData = {
      Title: e.target.elements.title.value,
      Content: content ? content : editData.content,
      IdImage: imagePreview?.id ? imagePreview?.id : editData.idImage,
      PostCategories: selectedCategories
    }

    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const url = currentUser.id == postId ? `/api/Posts/personal/${editData.id}` : `/api/Posts/${editData.id}`

      const response = await fetch(url, {
        method: "PUT",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: JSON.stringify(postData)
      })

      if (response.status == 204) {
        setShowSucessModal(true)
        setSuccessMessage("You have successfully updated post.")
      } else {
        const data = await response.json()
        if (!response.ok) {
          const errorMessages = data.errors.map(error => error.ErrorMessage)
          setShowErrorsModal(true)
          setErrorMessages(errorMessages)
        }
      }
    } catch (error) {
      setShowErrorModal(true)
      setErrorMessage('Cannot update post.')
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
              <img src={`/api/Images/images/${editData.imageName}`} alt="Uploaded" className="max-w-full h-auto mb-4" />
            </div>
          )}

          {imagePreview && (
            <div>
              <img src={`/api/Images/images/${imagePreview.imagePath}`} alt="Uploaded" className="max-w-full h-auto mb-4" />
            </div>
          )}
        </div>

        {imageUploadError && (
          <Alert color="failure">
            {imageUploadError}
          </Alert>
        )}

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

        {showErrorsModal && (
          <div className="error-modals show">
            {errorMessages.map((message, index) => (
              <div key={index} className="error-in-list">
                {message}
              </div>
            ))}
          </div>
        )}
      </form>
    </div>
  )
}
