import { useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { Button, Label, Spinner, TextInput } from 'flowbite-react'
import { useError } from '../contexts/ErrorContext'
import OAuth from '../components/OAuth'

export default function SignUp() {
  const [formData, setFormData] = useState({})
  const [loading, setLoading] = useState(false)
  const navigate = useNavigate()
  const { showError } = useError()

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.id]: e.target.value.trim() })
  }

  const handleSubmit = async (e) => {
    e.preventDefault()

    try {
      setLoading(true)

      const res = await fetch('/api/register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData),
      })

      setLoading(false)

      if (!res.ok) {
        const data = await res.json()

        const fieldOrder = ['FirstName', 'LastName', 'Email', 'Username', 'Password']
        const fieldDisplayNames = {
          FirstName: 'First Name',
          LastName: 'Last Name',
          Email: 'Email',
          Username: 'Username',
          Password: 'Password',
        }

        let validationErrors = []

        if (Array.isArray(data.errors)) {
          validationErrors = data.errors.map((error) => error.ErrorMessage)
        } else if (typeof data.errors === 'object') {
          validationErrors = Object.entries(data.errors)
            .sort(([a], [b]) => fieldOrder.indexOf(a) - fieldOrder.indexOf(b))
            .map(([fieldName]) => {
              const displayName = fieldDisplayNames[fieldName] || fieldName
              return `${displayName} is required`
            })
            .flat()
        } else {
          validationErrors = [data.message]
        }

        validationErrors.forEach(showError)

        return
      }

      navigate('/sign-in')
    } catch (error) {
      setLoading(false)
      showError('An unexpected error occurred. Please try again later.')
    }
  }

  return (
    <div className="min-h-screen mt-20">
      <div className="flex p-3 max-w-3xl mx-auto flex-col md:flex-row md:items-center gap-5">
        <div className="flex-1">
          <Link to="/" className="font-bold dark:text-white text-4xl">
            <span className="px-2 py-1 bg-gradient-to-r from-indigo-500 via-purple-500 to-pink-500 rounded-lg text-white">
              Marko&apos;s
            </span>
            Blog
          </Link>

          <p className="text-sm mt-5">
            This is a demo project. You can sign up with your email and password or with Google.
          </p>
        </div>

        <div className="flex-1">
          <form className="flex flex-col gap-4" onSubmit={handleSubmit}>
            <div>
              <Label value="First name" />
              <TextInput type="text" placeholder="Peter" id="firstName" onChange={handleChange} />
            </div>
            <div>
              <Label value="Last name" />
              <TextInput type="text" placeholder="Clyne" id="lastName" onChange={handleChange} />
            </div>
            <div>
              <Label value="Your username" />
              <TextInput type="text" placeholder="Username" id="username" onChange={handleChange} />
            </div>
            <div>
              <Label value="Your email" />
              <TextInput type="text" placeholder="name@company.com" id="email" onChange={handleChange} />
            </div>
            <div>
              <Label value="Your password" />
              <TextInput type="password" placeholder="Password" id="password" onChange={handleChange} />
            </div>

            <Button gradientDuoTone="purpleToPink" type="submit" disabled={loading}>
              {loading ? (
                <>
                  <Spinner size="sm" />
                  <span className="pl-3">Loading...</span>
                </>
              ) : (
                'Sign up'
              )}
            </Button>

            <OAuth />
          </form>

          <div className="flex gap-2 text-sm mt-5">
            <span>Have an account?</span>
            <Link to="/sign-in" className="text-blue-500">
              Sign In
            </Link>
          </div>
        </div>
      </div>
    </div>
  )
}
