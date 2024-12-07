import Comment from './Comment'
import PropTypes from 'prop-types'

export default function ChildComment({
  childComments,
  idParentComment,
  onDeleteComment,
  onLikeComment,
  onDislikeComment,
  onEditComment,
  onAddChildComment,
  activeReplyIdComment,
  setActiveReplyIdComment,
  comments,
  isFirstReply,
  isSmallScreen,
}) {
  return (
    <>
      {childComments
        .filter((child) => child.idParent === idParentComment)
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
              onDeleteComment={onDeleteComment}
              onLikeComment={onLikeComment}
              onDislikeComment={onDislikeComment}
              onEditComment={onEditComment}
              onAddChildComment={onAddChildComment}
              childrenComments={childComments}
              activeReplyIdComment={activeReplyIdComment}
              setActiveReplyIdComment={setActiveReplyIdComment}
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
  idParentComment: PropTypes.number.isRequired,
  onDeleteComment: PropTypes.func.isRequired,
  onLikeComment: PropTypes.func.isRequired,
  onDislikeComment: PropTypes.func.isRequired,
  onEditComment: PropTypes.func.isRequired,
  onAddChildComment: PropTypes.func.isRequired,
  activeReplyIdComment: PropTypes.number,
  setActiveReplyIdComment: PropTypes.func.isRequired,
  comments: PropTypes.arrayOf(PropTypes.object).isRequired,
  isFirstReply: PropTypes.bool.isRequired,
  isSmallScreen: PropTypes.bool.isRequired,
}