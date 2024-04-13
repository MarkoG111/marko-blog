import { Alert, Button, Checkbox, FileInput, TextInput } from "flowbite-react";
import { useEffect, useState } from "react";
import ReactQuill from 'react-quill';
import 'react-quill/dist/quill.snow.css';

export default function CreatePost() {
  const [selectedCategories, setSelectedCategories] = useState([])
  const [categories, setCategories] = useState([])

  const [imageFile, setImageFile] = useState(null)
  const [imageUploadError, setImageUploadError] = useState(null)
  const [imagePreview, setImagePreview] = useState(null);
  const [content, setContent] = useState('')

  const [errorMessages, setErrorMessage] = useState([]);

  const handleContentChange = (value) => {
    setContent(value);
  };

  const handleCategoryChange = (IdCategory) => {
    setSelectedCategories(prevCategories => {
      if (prevCategories.includes(IdCategory)) {
        return prevCategories.filter(id => id !== IdCategory);
      } else {
        return [...prevCategories, IdCategory];
      }
    });
  };

  useEffect(() => {
    fetch('/api/Categories')
      .then(response => response.json())
      .then(data => setCategories(data.items))
      .catch(error => console.error('Error fetching categories: ', error))
  }, [])

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
      Content: content,
      IdImage: imagePreview.id,
      PostCategories: selectedCategories.map(IdCategory => ({ IdPost: 0, IdCategory: IdCategory }))
    };

    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const response = await fetch('api/Posts', {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: JSON.stringify(postData)
      })

      if (!response.ok) {
        const data = await response.json();
        let validationErrors = [];

        validationErrors = data.errors.map(error => {
          return `${error.ErrorMessage}`;
        });

        setErrorMessage(validationErrors);
      }

      const insertPostId = await response.json();
      postData.PostCategories.forEach(postCategory => {
<<<<<<< HEAD
        postCategory.idPost = insertPostId;
=======
        postCategory.IdPost = insertPostId;
>>>>>>> 302b558e8d1e73a251f80e54cd26e042048d1532
      });

      console.log(postData);
    } catch (error) {
      console.log(error)
    }
  }

  return (
    <div className="p-3 max-w-3xl mx-auto min-h-screen">
      <h1 className="text-center text-3xl my-7 font-semibold">Create a post</h1>

      <form className="flex flex-col gap-4" onSubmit={handleSubmit}>
        <div className="flex flex-col gap-4 sm:flex-row justify-between">
          <TextInput type="text" placeholder="Title" required id="title" className="flex-1" />
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

          {imagePreview && (
            <div>
              <img src={`api/Images/images/${imagePreview.imagePath}`} alt="Uploaded" className="max-w-full h-auto mb-4" />
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
          id="content"
          className="h-72 mb-12"
          value={content}
          onChange={handleContentChange}
          required
        />

        <Button type="submit" gradientDuoTone="purpleToPink">Publish</Button>

        {errorMessages && errorMessages.length > 0 && (
          <Alert className='mt-5' color='failure'>
            {errorMessages.map((error, index) => (
              <div key={index}>{error}</div>
            ))}
          </Alert>
        )}
      </form>
    </div>
  )
<<<<<<< HEAD
}``
=======
}
>>>>>>> 302b558e8d1e73a251f80e54cd26e042048d1532
