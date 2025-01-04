export const handleApiError = async (response, showError) => {
  try {
    const errorText = await response.text()
    const errorData = JSON.parse(errorText)

    const errors = []

    if (Array.isArray(errorData.errors)) {
      errorData.errors.forEach((err) => {
        errors.push(err.ErrorMessage)
        showError(err.ErrorMessage)
      });
    } else if (typeof errorData.errors === 'object') {
      Object.entries(errorData.errors).forEach(([field, fieldErrors]) => {
        fieldErrors.forEach((err) => {
          errors.push(err)
          showError(err)
        });
      });
    } else {
      const errorMessage = errorData.title || "Check your credentials."
      errors.push(errorMessage)
      showError(errorMessage)
    }

    return errors;
  } catch (e) {
    showError("Error processing the response")
    return ["Error processing the response"]
  }
};
