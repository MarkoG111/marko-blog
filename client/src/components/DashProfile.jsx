import { Button, TextInput } from "flowbite-react"
import { useEffect, useRef, useState } from "react"
import { useSelector } from "react-redux"

export default function DashProfile() {
  const { currentUser } = useSelector((state) => state.user)

  const [imageFile, setImageFile] = useState(null)
  const [imageFileUrl, setImageFileUrl] = useState(null)

  const filePickerRef = useRef()

  const handleImagechange = (e) => {
    const file = e.target.files[0]

    if (file) {
      setImageFile(file)
      setImageFileUrl(URL.createObjectURL(file))
    }
  }

  const handleFormSubmit = async (e) => {
    e.preventDefault()

    const reader = new FileReader();
    reader.readAsDataURL(imageFile);

    reader.onloadend = async () => {
      const base64data = reader.result;

      const requestData = {
        Id: currentUser.id,
        FirstName: e.target.elements.FirstName.value,
        LastName: e.target.elements.LastName.value,
        Username: e.target.elements.Username.value,
        Email: e.target.elements.Email.value,
        Password: e.target.elements.Password.value,
        ProfilePicture: base64data
      };
      
      try {
        const token = localStorage.getItem("token");
        if (!token) {
          throw new Error("Token not found");
        }

        const response = await fetch(`/api/Users/${currentUser.id}`, {
          method: "PUT",
          headers: {
            "Authorization": `Bearer ${token}`,
            "Content-Type": "application/json"
          },
          body: JSON.stringify(requestData)
        });

        if (response.ok) {
          console.log(response, requestData);
        } else {
          console.log('Failed update profile picture');
        }
      } catch (error) {
        console.log('Error updating profile ', error);
      }
    }
  }

  useEffect(() => {
    return () => {
      if (imageFileUrl) {
        URL.revokeObjectURL(imageFileUrl);
      }
    };
  }, [imageFileUrl, imageFile])

  return (
    <div className="max-w-lg mx-auto p-3 w-full">
      <h1 className="my-7 text-center font-semibold text-3xl">Profile</h1>

      <form className="flex flex-col gap-4" onSubmit={handleFormSubmit}>
        <input type="file" accept="image/*" onChange={handleImagechange} ref={filePickerRef} hidden />

        <div className="w-32 h-32 self-center cursor-pointer shadow-md overflow-hidden rounded-full" onClick={() => filePickerRef.current.click()}>
          <img src={imageFileUrl || currentUser.profilePicture} alt="user" className="rounded-full w-full h-full border-8 border-[lightgray] object-cover" />
        </div>

        <TextInput type="text" id="FirstName" placeholder="First name" defaultValue={currentUser.firstName} />
        <TextInput type="text" id="LastName" placeholder="Last name" defaultValue={currentUser.lastName} />
        <TextInput type="text" id="Username" placeholder="Username" defaultValue={currentUser.username} />
        <TextInput type="email" id="Email" placeholder="Email" defaultValue={currentUser.email} />
        <TextInput type="password" id="Password" placeholder="Password" defaultValue={currentUser.password} />

        <Button type="submit" gradientDuoTone='purpleToBlue' outline>Update</Button>
      </form>

      <div className="text-red-500 flex justify-between mt-5">
        <span className="cursor-pointer">Delete Account</span>
        <span className="cursor-pointer">Sign Out</span>
      </div>
    </div>
  )
}
