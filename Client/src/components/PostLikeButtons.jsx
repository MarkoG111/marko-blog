import { FaThumbsUp, FaThumbsDown, FaRegCommentDots } from 'react-icons/fa'
import PropTypes from 'prop-types'

const PostLikeButtons = ({ post, idPost, onLikePost, onDislikePost, commentsNumber }) => {
  return (
    <div className="text-sm mb-8 mt-12 flex items-center justify-between gap-1">
      <div className="flex">
        <FaRegCommentDots className="text-2xl" />
        <span className="ml-2">
          {commentsNumber === 1 ? `${commentsNumber} Comment` : `${commentsNumber} Comments`}
        </span>
      </div>
      <div className="flex ml-10">
        <button
          type="button"
          onClick={() => onLikePost(idPost)}
          className="text-gray-400 hover:text-blue-500 ml-6"
        >
          <FaThumbsUp className="text-xl" />
        </button>
        <span className="ml-2">{post.likes && post.likes.filter((like) => like.status === 1).length}</span>

        <button
          type="button"
          onClick={() => onDislikePost(idPost)}
          className="text-gray-400 hover:text-red-500 ml-2"
        >
          <FaThumbsDown className="ml-5 text-xl" />
        </button>
        <span className="ml-2">{post.likes && post.likes.filter((like) => like.status === 2).length}</span>
      </div>
    </div>
  )
}

PostLikeButtons.propTypes = {
  post: PropTypes.shape({
    likes: PropTypes.arrayOf(
      PropTypes.shape({
        idUser: PropTypes.number.isRequired,
        status: PropTypes.number.isRequired,
      })
    ).isRequired,
  }).isRequired,
  idPost: PropTypes.number.isRequired,
  onLikePost: PropTypes.func.isRequired,
  onDislikePost: PropTypes.func.isRequired,
  commentsNumber: PropTypes.number.isRequired,
}

export default PostLikeButtons