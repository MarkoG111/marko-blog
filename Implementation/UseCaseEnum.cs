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

        EFCreateBlogCommand = 2,
        EFUpdateBlogCommand = 3,
        EFDeleteBlogCommand = 4,
        EFGetOneBlogQuery = 5,
        EFGetBlogsQuery = 6,
        EFUpdatePersonalBlogCommand = 7,
        EFDeletePersonalBlogCommand = 8,

        EFCreateCommentCommand = 9,
        EFUpdateCommentCommand = 10,
        EFDeleteCommentCommand = 11,
        EFGetOneCommentQuery = 12,
        EFUpdatePersonalCommentCommand = 14,
        EFDeletePersonalCommentCommand = 15,

        EFLikeBlog = 16,

        EFGetUseCaseLogQuery = 17,

        EFCreateUserCommand = 18,
        EFUpdateUserCommand = 19,
        EFDeleteUserCommand = 20,
        EFGetOneUserQuery = 21,
        EFGetUsersQuery = 22,

        EFCreateCategoryCommand = 23,
        EFUpdateCategoryCommand = 24,
        EFDeleteCategoryCommand = 25,
        EFGetOneCategoryQuery = 26,
        EFGetCategoriesQuery = 27
    }
}