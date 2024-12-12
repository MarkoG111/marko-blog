import { Button, Label, Textarea } from "flowbite-react"
import { useEffect, useState } from "react"
import { useSelector } from "react-redux"
import { useError } from "../contexts/ErrorContext"
import { useSuccess } from "../contexts/SuccessContext"

export default function RequestAuthorForm() {
  const { loading } = useSelector((state) => state.user)
  const { currentUser } = useSelector(state => state.user)

  const [reason, setReason] = useState('')
  const [authorRequests, setAuthorRequests] = useState([])

  const { showError } = useError()
  const { showSuccess } = useSuccess()

  const handleSubmit = async (e) => {
    e.preventDefault()

    try {
      const token = localStorage.getItem("token")
      if (!token) {
        showError("Token not found")
        return
      }

      const body = JSON.stringify({
        IdUser: currentUser.id,
        Username: currentUser.username,
        ProfilePicture: currentUser.profilePicture,
        Status: 1,
        Reason: reason
      })

      const response = await fetch(`/authorrequests`, {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: body
      });

      if (response.ok) {
        const data = await response.json()

        setAuthorRequests([...authorRequests, data])
        setReason('')
        showSuccess('Successfully submited request')
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

  return (
    <div className="max-w-lg mx-auto p-3 w-full">
      <h1 className="my-7 text-center font-semibold text-3xl">Submit an author request</h1>

      <form className="flex flex-col gap-4" onSubmit={handleSubmit}>
        <div className="">
          <div className="mb-2 block">
            <Label htmlFor="requestAuthorFormTag" value="Why do you want to become an author?" />
          </div>
          <Textarea id="requestAuthorFormTag" value={reason} onChange={(e) => setReason(e.target.value)} required rows={4} />
        </div>

        <Button type="submit" gradientDuoTone='purpleToBlue' outline disabled={loading}>{loading ? 'Loading...' : 'Submit Request'}</Button>
      </form>
    </div>
  )
}
