export const getAvatarSrc = (profilePicture, imageError = false) => {
  if (!profilePicture) {
    return undefined
  }

  if (imageError) {
    return `/images/proxy?url=${encodeURIComponent(profilePicture)}`
  }

  if (profilePicture.startsWith("http")) {
    return profilePicture
  }

  return `/users/images/${profilePicture}`
}