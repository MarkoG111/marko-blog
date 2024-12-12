export const handleApiError = async (response, showError) => {
  try {
    const errorText = await response.text()
    const errorData = JSON.parse(errorText)

    if (Array.isArray(errorData.errors)) {
      errorData.errors.forEach((err) => {
        showError(err.ErrorMessage)
      })
    } else if (typeof errorData.errors === 'object') {
      Object.entries(errorData.errors).forEach(([field, errors]) => {
        errors.forEach((err) => showError(err))
      })
    } else {
      const errorMessage = errorData.title || "An unknown error occurred"
      showError(errorMessage)
    }
  } catch (e) {
    showError("Error processing the response")
  }
}