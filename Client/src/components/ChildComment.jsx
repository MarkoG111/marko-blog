import Comment from './Comment'
import PropTypes from 'prop-types'

export default function ChildComment({
  childComments,
  parentCommentId,
  onDelete,
  onLike,
  onDislike,
  onEdit,
  onAddChildComment,
  activeReplyCommentId,
  setActiveReplyCommentId,
  comments,
  isFirstReply,
  isSmallScreen,
}) {
  return (
    <>
      {childComments
        .filter((child) => child.idParent === parentCommentId)
        .map((child) => (
          <div
            key={child.id}
            style={{
              paddingLeft: isFirstReply ? (isSmallScreen ? '25px' : '60px') : '0',
              paddingBottom: '10px',
            }}
          >
            <Comment
              comment={child}
              onDelete={onDelete}
              onLike={onLike}
              onDislike={onDislike}
              onEdit={onEdit}
              onAddChildComment={onAddChildComment}
              childrenComments={childComments}
              activeReplyCommentId={activeReplyCommentId}
              setActiveReplyCommentId={setActiveReplyCommentId}
              comments={comments}
            />
          </div>
        ))}
    </>
  )
}

ChildComment.propTypes = {
  childComments: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.number.isRequired,
      idParent: PropTypes.number.isRequired,
    })
  ).isRequired,
  parentCommentId: PropTypes.number.isRequired,
  onDelete: PropTypes.func.isRequired,
  onLike: PropTypes.func.isRequired,
  onDislike: PropTypes.func.isRequired,
  onEdit: PropTypes.func.isRequired,
  onAddChildComment: PropTypes.func.isRequired,
  activeReplyCommentId: PropTypes.number,
  setActiveReplyCommentId: PropTypes.func.isRequired,
  comments: PropTypes.arrayOf(PropTypes.object).isRequired,
  isFirstReply: PropTypes.bool.isRequired,
  isSmallScreen: PropTypes.bool.isRequired,
}