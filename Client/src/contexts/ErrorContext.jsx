import { createContext, useState, useContext } from 'react'
import PropTypes from 'prop-types'

const ErrorContext = createContext()

export const useError = () => useContext(ErrorContext)

export const ErrorProvider = ({ children }) => {
  const [errors, setErrors] = useState([])

  const showError = (message) => {
    const id = `${Date.now()}-${Math.random()}`
    setErrors((prevErrors) => [...prevErrors, { id, message }])

    setTimeout(() => {
      setErrors((prevErrors) => prevErrors.filter((error) => error.id !== id))
    }, 7000)
  }

  return (
    <ErrorContext.Provider value={{ showError }}>
      {children}

      {errors.map((error, index) => (
        <div
          key={error.id}
          className="fixed top-32 right-4 bg-red-600 text-white p-4 rounded shadow-lg z-50"
          style={{
            top: `${32 + index * 80}px`, 
            transform: 'translateY(0)', 
          }}
        >
          <p>{error.message}</p>
        </div>
      ))}
    </ErrorContext.Provider>
  )
}

ErrorProvider.propTypes = {
  children: PropTypes.node.isRequired,
}
