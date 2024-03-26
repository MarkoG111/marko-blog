using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Domain;

namespace Implementation
{
    public static class UseCaseExtension
    {
        public static void AddDefaultUseCasesForRole(this User user)
        {
            var list = new HashSet<UserUseCase>();

            if (user.IdRole == (int)RoleEnum.Admin)
            {
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFCreateBlogCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFUpdateBlogCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFDeleteBlogCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetOneBlogQuery });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetBlogsQuery });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFCreateCommentCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFUpdateCommentCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFDeleteCommentCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetOneCommentQuery });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetUseCaseLogQuery });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFCreateUserCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFUpdateUserCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFDeleteUserCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetOneUserQuery });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetUsersQuery });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFCreateCategoryCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFUpdateCategoryCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFDeleteCategoryCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetOneCategoryQuery });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetCategoriesQuery });
            }

            else if (user.IdRole == (int)RoleEnum.Author)
            {
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFCreateBlogCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFUpdatePersonalBlogCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFDeletePersonalBlogCommand });
                
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetOneBlogQuery });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetBlogsQuery });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetOneCategoryQuery });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetCategoriesQuery });
            }

            else if (user.IdRole == (int)RoleEnum.User)
            {
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetOneBlogQuery });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetBlogsQuery });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFCreateCommentCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFUpdatePersonalCommentCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFDeletePersonalCommentCommand });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFLikeBlog });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetOneCategoryQuery });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetCategoriesQuery });
            }

            user.UserUseCases = list;
        }
    }
}