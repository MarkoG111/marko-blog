export const getAvatarSrc = (profilePicture, imageError = false) => {
  if (!profilePicture) {
    return undefined
  }

  if (imageError) {
    return `/api/images/proxy?url=${encodeURIComponent(profilePicture)}`
  }

  if (profilePicture.startsWith("http")) {
    return profilePicture
  }

  return `/api/users/profile-image/${encodeURIComponent(profilePicture)}`;
}