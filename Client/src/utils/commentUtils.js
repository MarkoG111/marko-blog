export const checkIfAlreadyVoted = (comments, idComment, idUser, status) => {
  return comments.some(comment => {
    if (comment.id === idComment && comment.likes) {
      return comment.likes.some(like => like.idUser === idUser && like.status === status)
    }

    return false
  })
}

export const removeDislikeOrLikeIfPresent = (comments, idComment, idUser, status) => {
  return comments.map((comment) => {
    if (comment.id === idComment && comment.likes) {
      const hasDisliked = comment.likes.some(like => like.idUser === idUser && like.status === status)
      if (hasDisliked) {
        const updatedLikes = comment.likes.filter(like => !(like.idUser === idUser && like.status === status))
        return { ...comment, likes: updatedLikes }
      }
    }

    return comment
  })
}

export const updateCommentLikes = (comments, idComment, data, userId) => {
  return comments.map(comment => {
    if (comment.id === idComment) {
      return {
        ...comment,
        likesCount: data.likesCount,
        likes: comment.likes.some(like => like.idUser === userId)
          ? comment.likes.map(like => (like.idUser === userId ? { ...like, status: data.status } : like))
          : [...comment.likes, { idUser: userId, idComment, status: data.status }],
      }
    }

    if (comment.children && comment.children.length > 0) {
      return {
        ...comment,
        children: updateCommentLikes(comment.children, idComment, data, userId),
      }
    }

    return comment
  })
}

export const handleOptimisticUpdate = (comments, setComments, idComment, type) => {
  const updatedComments = comments.map(comment =>
    comment.id === idComment
      ? { ...comment, likesCount: comment.likesCount + (type === 'like' ? 1 : -1) }
      : comment
  )
  setComments(updatedComments)
}