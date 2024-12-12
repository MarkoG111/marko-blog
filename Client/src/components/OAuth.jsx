import { Button } from "flowbite-react"
import { AiFillGoogleCircle } from "react-icons/ai"
import { GoogleAuthProvider, signInWithPopup, getAuth } from "firebase/auth"
import { app } from "../firebase"
import { useDispatch } from "react-redux"
import { signInSuccess } from "../redux/user/userSlice"
import { useNavigate } from "react-router-dom"
import { jwtDecode } from 'jwt-decode'
import { useError } from "../contexts/ErrorContext"

export default function OAuth() {
  const auth = getAuth(app)

  const dispatch = useDispatch()

  const navigate = useNavigate()

  const { showError } = useError()

  const handleGoogleClick = async () => {
    const provider = new GoogleAuthProvider()

    provider.setCustomParameters({ prompt: 'select_account' })

    try {
      const resultsFromGoogle = await signInWithPopup(auth, provider)

      const response = await fetch('/auth', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          name: resultsFromGoogle.user.displayName,
          email: resultsFromGoogle.user.email,
          googlePhotoUrl: resultsFromGoogle.user.photoURL
        })
      })

      if (response.ok) {
        const { token } = await response.json()

        const decodedToken = jwtDecode(token)

        const userProfile = decodedToken.ActorData

        localStorage.setItem('token', token)

        dispatch(signInSuccess(userProfile))

        navigate('/')
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
    <Button type="button" gradientDuoTone='pinkToOrange' outline onClick={handleGoogleClick}>
      <AiFillGoogleCircle className='w-6 h-6 mr-2' />
      Continue with Google
    </Button>
  )
}