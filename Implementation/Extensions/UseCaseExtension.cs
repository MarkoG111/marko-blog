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
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFCreatePostCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFUpdatePostCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFDeletePostCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetOnePostQuery });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetPostsQuery });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFCreateCommentCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFUpdateCommentCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFDeleteCommentCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetOneCommentQuery });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetCommentsQuery });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFUpdatePersonalCommentCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFDeletePersonalCommentCommand });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetUseCaseLogQuery });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFCreateUserCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFUpdateUserCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFDeleteUserCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetOneUserQuery });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetUsersQuery });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFLikePost });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFLikeComment });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFCreateCategoryCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFUpdateCategoryCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFDeleteCategoryCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetOneCategoryQuery });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetCategoriesQuery });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFUpdateAuthorRequestCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetAuthorRequestsQuery });
            }

            else if (user.IdRole == (int)RoleEnum.Author)
            {
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFCreatePostCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFUpdatePersonalPostCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFDeletePersonalPostCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetOnePostQuery });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetPostsQuery });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetCommentsQuery });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFDeletePersonalCommentCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFUpdatePersonalCommentCommand });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFLikePost });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFLikeComment });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFUpdateUserCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetOneUserQuery });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetUsersQuery });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetOneCategoryQuery });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetCategoriesQuery });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFFollowCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFUnfollowCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFCheckFollowStatusQuery });

            }

            else if (user.IdRole == (int)RoleEnum.User)
            {
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetOnePostQuery });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetPostsQuery });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetOneCategoryQuery });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetCategoriesQuery });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFCreateCommentCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFUpdatePersonalCommentCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFDeletePersonalCommentCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetCommentsQuery });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetOneUserQuery });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFGetUsersQuery });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFLikePost });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFLikeComment });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFCreateAuthorRequestCommand });

                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFFollowCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFUnfollowCommand });
                list.Add(new UserUseCase { IdUseCase = (int)UseCaseEnum.EFCheckFollowStatusQuery });
            }

            user.UserUseCases = list;
        }
    }
}