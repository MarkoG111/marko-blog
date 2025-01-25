using System;

namespace Implementation
{
    public enum RoleEnum
    {
        Admin = 1,
        Author = 2,
        User = 3
    }

    public enum UseCaseEnum
    {
        EFRegisterUserCommand = 1,

        EFCreatePostCommand = 2,
        EFUpdatePostCommand = 3,
        EFDeletePostCommand = 4,
        EFGetOnePostQuery = 5,
        EFGetPostsQuery = 6,
        EFUpdatePersonalPostCommand = 7,
        EFDeletePersonalPostCommand = 8,

        EFCreateCommentCommand = 9,
        EFUpdateCommentCommand = 10,
        EFDeleteCommentCommand = 11,
        EFGetOneCommentQuery = 12,
        EFGetCommentsQuery = 13,
        EFUpdatePersonalCommentCommand = 14,
        EFDeletePersonalCommentCommand = 15,

        EFLikePost = 16,
        EFLikeComment = 17,

        EFGetUseCaseLogQuery = 18,

        EFUpdateUserCommand = 19,
        EFDeleteUserCommand = 20,
        EFGetOneUserQuery = 21,
        EFGetUsersQuery = 22,

        EFCreateCategoryCommand = 23,
        EFUpdateCategoryCommand = 24,
        EFDeleteCategoryCommand = 25,
        EFGetOneCategoryQuery = 26,
        EFGetCategoriesQuery = 27,

        EFCreateAuthorRequestCommand = 28,
        EFUpdateAuthorRequestCommand = 29,
        EFGetAuthorRequestsQuery = 30,

        EFFollowCommand = 31,
        EFUnfollowCommand = 32,
        EFCheckFollowStatusQuery = 33,

        EFCreateNotificationCommand = 34,
        EFGetNotificationsQuery = 35,

        EFMarkAllNotificationsAsReadCommand = 36,
        
        EFGetFollowersQuery = 37,
        EFGetFollowingsQuery = 38,
    }
}