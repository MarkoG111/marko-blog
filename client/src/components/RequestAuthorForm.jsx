import { Button, Label, Textarea } from "flowbite-react"
import { useEffect, useState } from "react"
import { useSelector } from "react-redux"

export default function RequestAuthorForm() {
  const { loading } = useSelector((state) => state.user)
  const { currentUser } = useSelector(state => state.user)

  const [errorMessage, setErrorMessage] = useState('')
  const [showErrorModal, setShowErrorModal] = useState(false)
  const [successMessage, setSuccessMessage] = useState('')
  const [showSuccessModal, setShowSuccessModal] = useState(false)

  const [reason, setReason] = useState('')
  const [authorRequests, setAuthorRequests] = useState([])

  useEffect(() => {
    if (showErrorModal) {
      setShowErrorModal(true)
    }
    if (showSuccessModal) {
      setShowSuccessModal(true)
    }
    const timer = setTimeout(() => {
      setShowErrorModal(false)
      setShowSuccessModal(false)
    }, 10000);

    return () => clearTimeout(timer);
  }, [showErrorModal, showSuccessModal])

  const handleSubmit = async (e) => {
    e.preventDefault()

    try {
      const token = localStorage.getItem("token")
      if (!token) {
        throw new Error("Token not found")
      }

      const body = JSON.stringify({
        IdUser: currentUser.id,
        Username: currentUser.username,
        ProfilePicture: currentUser.profilePicture,
        Status: 1,
        Reason: reason
      })

      const response = await fetch(`/api/AuthorRequests`, {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: body
      });

      if (response.ok) {
        const data = await response.json()
        console.log(data)
        setAuthorRequests([...authorRequests, data])
        setReason('')
        setShowSuccessModal(true)
        setSuccessMessage('Successfully submited request.')
      } else {
        const errorText = await response.text()
        const errorData = JSON.parse(errorText)
        setShowErrorModal(true)
        setErrorMessage(errorData.message)
      }
    } catch (error) {
      setErrorMessage("An error occurred while processing your request.")
      setShowErrorModal(true)
    }
  }

  return (
    <div className="max-w-lg mx-auto p-3 w-full">
      <h1 className="my-7 text-center font-semibold text-3xl">Submit an author request</h1>

      {showErrorModal && (
        <div className={`error-modal show`}>{errorMessage}</div>
      )}

      {showSuccessModal && (
        <div className={`success-modal show`}>{successMessage}</div>
      )}

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
