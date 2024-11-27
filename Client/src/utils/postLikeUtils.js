export const checkIfAlreadyVotedOnPost = (post, idPost, idUser, status) => {
  if (post.id == idPost && post.likes) {
    return post.likes.some(like => like.idUser == idUser && like.status == status)
  }

  return false
}

export const removeDislikeOrLikeIfPresentInPost = (post, idPost, idUser, status) => {
  if (post.id == idPost) {
    const hasVote = post.likes.some(like => like.idUser == idUser && like.status == status)
    if (hasVote) {
      const updateLikes = post.likes.filter(like => !(like.idUser == idUser && like.status == status))
      return { ...post, likes: updateLikes }
    }
  }

  return post
}

export const updatePostLikes = (post, idPost, data, userId) => {
  if (post.id == idPost) {
    return {
      ...post,
      likes: post.likes.some(like => like.idUser == userId)
        ? post.likes.map(like => (like.idUser == userId ? { ...like, status: data.status } : like)) : [...post.likes, { idUser: userId, idPost, status: data.status }]
    }
  }

  return post
}
