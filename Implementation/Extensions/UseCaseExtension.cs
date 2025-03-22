using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using EFDataAccess;

namespace Implementation
{
    public static class UseCaseExtension
    {
        public static void UpdateUseCasesForRole(this User user, BlogContext context)
        {
            var currentUseCaseIds = context.UserUseCases.Where(uc => uc.IdUser == user.Id).Select(uc => uc.IdUseCase).ToHashSet();

            var newUseCases = user.IdRole switch
            {
                (int)RoleEnum.Admin => new HashSet<int>
                {
                    (int)UseCaseEnum.EFCreatePostCommand,
                    (int)UseCaseEnum.EFUpdatePostCommand,
                    (int)UseCaseEnum.EFDeletePostCommand,
                    (int)UseCaseEnum.EFGetOnePostQuery,
                    (int)UseCaseEnum.EFGetPostsQuery,
                    (int)UseCaseEnum.EFUpdatePersonalPostCommand,
                    (int)UseCaseEnum.EFDeletePersonalPostCommand,

                    (int)UseCaseEnum.EFCreateCommentCommand,
                    (int)UseCaseEnum.EFUpdateCommentCommand,
                    (int)UseCaseEnum.EFDeleteCommentCommand,
                    (int)UseCaseEnum.EFGetOneCommentQuery,
                    (int)UseCaseEnum.EFGetCommentsQuery,
                    (int)UseCaseEnum.EFUpdatePersonalCommentCommand,
                    (int)UseCaseEnum.EFDeletePersonalCommentCommand,

                    (int)UseCaseEnum.EFLikePostCommand,
                    (int)UseCaseEnum.EFLikeCommentCommand,

                    (int)UseCaseEnum.EFGetUseCaseLogQuery,

                    (int)UseCaseEnum.EFUpdateUserCommand,
                    (int)UseCaseEnum.EFDeleteUserCommand,
                    (int)UseCaseEnum.EFGetOneUserQuery,
                    (int)UseCaseEnum.EFGetUsersQuery,

                    (int)UseCaseEnum.EFCreateCategoryCommand,
                    (int)UseCaseEnum.EFUpdateCategoryCommand,
                    (int)UseCaseEnum.EFDeleteCategoryCommand,
                    (int)UseCaseEnum.EFGetOneCategoryQuery,
                    (int)UseCaseEnum.EFGetCategoriesQuery,

                    (int)UseCaseEnum.EFUpdateAuthorRequestCommand,
                    (int)UseCaseEnum.EFGetAuthorRequestsQuery,

                    (int)UseCaseEnum.EFFollowCommand,
                    (int)UseCaseEnum.EFUnfollowCommand,
                    (int)UseCaseEnum.EFCheckFollowStatusQuery,

                    (int)UseCaseEnum.EFCreateNotificationCommand,
                    (int)UseCaseEnum.EFGetNotificationsQuery,
                    (int)UseCaseEnum.EFMarkAllNotificationsAsReadCommand,

                    (int)UseCaseEnum.EFGetFollowersQuery,
                    (int)UseCaseEnum.EFGetFollowingsQuery
                },

                (int)RoleEnum.Author => new HashSet<int>
                {
                    (int)UseCaseEnum.EFCreatePostCommand,
                    (int)UseCaseEnum.EFGetOnePostQuery,
                    (int)UseCaseEnum.EFGetPostsQuery,
                    (int)UseCaseEnum.EFUpdatePersonalPostCommand,
                    (int)UseCaseEnum.EFDeletePersonalPostCommand,

                    (int)UseCaseEnum.EFCreateCommentCommand,
                    (int)UseCaseEnum.EFGetOneCommentQuery,
                    (int)UseCaseEnum.EFGetCommentsQuery,
                    (int)UseCaseEnum.EFUpdatePersonalCommentCommand,
                    (int)UseCaseEnum.EFDeletePersonalCommentCommand,

                    (int)UseCaseEnum.EFLikePostCommand,
                    (int)UseCaseEnum.EFLikeCommentCommand,
                    (int)UseCaseEnum.EFUnlikeCommentCommand,
                    (int)UseCaseEnum.EFUnlikePostCommand,

                    (int)UseCaseEnum.EFUpdateUserCommand,
                    (int)UseCaseEnum.EFGetOneUserQuery,
                    (int)UseCaseEnum.EFGetUsersQuery,

                    (int)UseCaseEnum.EFGetOneCategoryQuery,
                    (int)UseCaseEnum.EFGetCategoriesQuery,

                    (int)UseCaseEnum.EFUpdateAuthorRequestCommand,

                    (int)UseCaseEnum.EFFollowCommand,
                    (int)UseCaseEnum.EFUnfollowCommand,
                    (int)UseCaseEnum.EFCheckFollowStatusQuery,

                    (int)UseCaseEnum.EFCreateNotificationCommand,
                    (int)UseCaseEnum.EFGetNotificationsQuery,
                    (int)UseCaseEnum.EFMarkAllNotificationsAsReadCommand,

                    (int)UseCaseEnum.EFGetFollowersQuery,
                    (int)UseCaseEnum.EFGetFollowingsQuery
                },

                (int)RoleEnum.User => new HashSet<int>
                {
                    (int)UseCaseEnum.EFGetOnePostQuery,
                    (int)UseCaseEnum.EFGetPostsQuery,

                    (int)UseCaseEnum.EFCreateCommentCommand,
                    (int)UseCaseEnum.EFGetOneCommentQuery,
                    (int)UseCaseEnum.EFGetCommentsQuery,
                    (int)UseCaseEnum.EFUpdatePersonalCommentCommand,
                    (int)UseCaseEnum.EFDeletePersonalCommentCommand,

                    (int)UseCaseEnum.EFLikePostCommand,
                    (int)UseCaseEnum.EFLikeCommentCommand,
                    (int)UseCaseEnum.EFUnlikeCommentCommand,
                    (int)UseCaseEnum.EFUnlikePostCommand,

                    (int)UseCaseEnum.EFUpdateUserCommand,
                    (int)UseCaseEnum.EFGetOneUserQuery,
                    (int)UseCaseEnum.EFGetUsersQuery,

                    (int)UseCaseEnum.EFGetOneCategoryQuery,
                    (int)UseCaseEnum.EFGetCategoriesQuery,

                    (int)UseCaseEnum.EFCreateAuthorRequestCommand,

                    (int)UseCaseEnum.EFFollowCommand,
                    (int)UseCaseEnum.EFUnfollowCommand,
                    (int)UseCaseEnum.EFCheckFollowStatusQuery,

                    (int)UseCaseEnum.EFCreateNotificationCommand,
                    (int)UseCaseEnum.EFGetNotificationsQuery,
                    (int)UseCaseEnum.EFMarkAllNotificationsAsReadCommand,

                    (int)UseCaseEnum.EFGetFollowersQuery,
                    (int)UseCaseEnum.EFGetFollowingsQuery
                },
            };

            var useCasesToAdd = newUseCases.Where(useCaseId => !currentUseCaseIds.Contains(useCaseId)).Select(useCaseId => new UserUseCase { IdUser = user.Id, IdUseCase = useCaseId }).ToList();

            if (useCasesToAdd.Any())
            {
                context.UserUseCases.AddRange(useCasesToAdd);
            }

            var useCasesToRemove = context.UserUseCases.Where(uc => uc.IdUser == user.Id && !newUseCases.Contains(uc.IdUseCase)).ToList();

            if (useCasesToRemove.Any())
            {
                context.UserUseCases.RemoveRange(useCasesToRemove);
            }

            context.SaveChanges();
        }
    }
}
