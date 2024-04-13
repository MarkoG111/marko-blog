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

        EFCreateUserCommand = 19,
        EFUpdateUserCommand = 20,
        EFDeleteUserCommand = 21,
        EFGetOneUserQuery = 22,
        EFGetUsersQuery = 23,

        EFCreateCategoryCommand = 24,
        EFUpdateCategoryCommand = 25,
        EFDeleteCategoryCommand = 26,
        EFGetOneCategoryQuery = 27,
        EFGetCategoriesQuery = 28
    }
}