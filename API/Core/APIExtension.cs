using System.Text;

using EFDataAccess;

using Application;

using Application.Commands.Post;
using Application.Commands.Category;
using Application.Commands.User;
using Application.Commands.Like;
using Application.Commands.Comment;
using Application.Commands.AuthorRequest;
using Application.Commands.Follow;
using Application.Commands.Notification;

using Application.Queries;
using Application.Queries.Post;
using Application.Queries.Category;
using Application.Queries.User;
using Application.Queries.Comment;
using Application.Queries.AuthorRequest;
using Application.Queries.Follow;
using Application.Queries.Notification;
using Application.Services;

using Implementation.Validators.Post;
using Implementation.Validators.Category;
using Implementation.Validators.User;
using Implementation.Validators.Like;
using Implementation.Validators.Comment;
using Implementation.Validators.AuthorRequest;

using Implementation.Commands.Post;
using Implementation.Commands.Category;
using Implementation.Commands.User;
using Implementation.Commands.Comment;
using Implementation.Commands.Like;
using Implementation.Commands.AuthorRequest;
using Implementation.Commands.Follow;
using Implementation.Commands.Notification;

using Implementation.Queries;
using Implementation.Queries.Post;
using Implementation.Queries.Category;
using Implementation.Queries.Comment;
using Implementation.Queries.User;
using Implementation.Queries.AuthorRequest;
using Implementation.Queries.Follow;
using Implementation.Queries.Notification;
using Implementation.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace API.Core
{
    public static class APIExtension
    {
        public static void LoadUseCases(this IServiceCollection services)
        {
            services.AddTransient<UseCaseExecutor>();

            services.AddTransient<INotificationHubService, SignalRNotificationHub>();

            services.AddTransient<INotificationService, NotificationService>();

            // Commands
            services.AddTransient<IRegisterUserCommand, EFRegisterUserCommand>();

            services.AddTransient<ICreatePostCommand, EFCreatePostCommand>();
            services.AddTransient<IUpdatePostCommand, EFUpdatePostCommand>();
            services.AddTransient<IDeletePostCommand, EFDeletePostCommand>();
            services.AddTransient<IUpdatePersonalPostCommand, EFUpdatePersonalPostCommand>();
            services.AddTransient<IDeletePersonalPostCommand, EFDeletePersonalPostCommand>();

            services.AddTransient<ICreateCategoryCommand, EFCreateCategoryCommand>();
            services.AddTransient<IUpdateCategoryCommand, EFUpdateCategoryCommand>();
            services.AddTransient<IDeleteCategoryCommand, EFDeleteCategoryCommand>();

            services.AddTransient<ICreateCommentCommand, EFCreateCommentCommand>();
            services.AddTransient<IUpdatePersonalCommentCommand, EFUpdatePersonalCommentCommand>();
            services.AddTransient<IDeleteCommentCommand, EFDeleteCommentCommand>();
            services.AddTransient<IDeletePersonalCommentCommand, EFDeletePersonalCommentCommand>();

            services.AddTransient<ICreateUserCommand, EFCreateUserCommand>();
            services.AddTransient<IUpdateUserCommand, EFUpdateUserCommand>();
            services.AddTransient<IDeleteUserCommand, EFDeleteUserCommand>();

            services.AddTransient<ILikePostCommand, EFLikePostCommand>();
            services.AddTransient<ILikeCommentCommand, EFLikeCommentCommand>();

            services.AddTransient<ICreateAuthorRequestCommand, EFCreateAuthorRequestCommand>();
            services.AddTransient<IUpdateAuthorRequestCommand, EFUpdateAuthorRequestCommand>();

            services.AddTransient<IFollowCommand, EFFollowCommand>();
            services.AddTransient<IUnfollowCommand, EFUnfollowCommand>();

            services.AddTransient<ICreateNotificationCommand, EFCreateNotificationCommand>();
            services.AddTransient<IMarkAllNotificationsAsReadCommand, EFMarkAllNotificationsAsReadCommand>();

            // Queries
            services.AddTransient<IGetPostsQuery, EFGetPostsQuery>();
            services.AddTransient<IGetPostQuery, EFGetPostQuery>();

            services.AddTransient<IGetCategoriesQuery, EFGetCategoriesQuery>();
            services.AddTransient<IGetCategoryQuery, EFGetCategoryQuery>();

            services.AddTransient<IGetCommentQuery, EFGetCommentQuery>();
            services.AddTransient<IGetCommentsQuery, EFGetCommentsQuery>();

            services.AddTransient<IGetUserQuery, EFGetUserQuery>();
            services.AddTransient<IGetUsersQuery, EFGetUsersQuery>();

            services.AddTransient<IGetAuthorRequestsQuery, EFGetAuthorRequestsQuery>();

            services.AddTransient<IGetUseCaseLogsQuery, EFGetUseCaseLogsQuery>();

            services.AddTransient<ICheckFollowStatusQuery, EFCheckFollowStatusQuery>();

            services.AddTransient<IGetNotificationsQuery, EFGetNotificationsQuery>();

            services.AddTransient<IGetFollowersQuery, EFGetFollowersQuery>();
            services.AddTransient<IGetFollowingQuery, EFGetFollowingsQuery>();

            // Validators
            services.AddTransient<RegisterUserValidator>();

            services.AddTransient<CreatePostValidator>();
            services.AddTransient<CreateCategoryValidator>();
            services.AddTransient<CreateCommentValidator>();
            services.AddTransient<CreateUserValidator>();

            services.AddTransient<LikePostValidator>();
            services.AddTransient<LikeCommentValidator>();

            services.AddTransient<UpdatePostValidator>();
            services.AddTransient<UpdateCategoryValidator>();
            services.AddTransient<UpdateCommentValidator>();
            services.AddTransient<UpdateUserValidator>();
            services.AddTransient<UpdateUserWithoutImageValidator>();

            services.AddTransient<DeletePostValidator>();
            services.AddTransient<DeleteCategoryValidator>();
            services.AddTransient<DeleteCommentValidator>();

            services.AddTransient<AuthorRequestValidator>();
        }

        public static void AddApplicationActor(this IServiceCollection services)
        {
            services.AddTransient<IApplicationActor>(x =>
            {
                var accessor = x.GetService<IHttpContextAccessor>();

                var user = accessor.HttpContext.User;

                if (user.FindFirst("ActorData") == null)
                {
                    return new AnonymousActor();
                }

                var actorString = user.FindFirst("ActorData").Value;

                var actor = JsonConvert.DeserializeObject<JWTActor>(actorString);

                return actor;
            });
        }

        public static void AddJWT(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddTransient<JWTManager>(x =>
            {
                var context = x.GetService<BlogContext>();
                return new JWTManager(context, appSettings.JwtIssuer, appSettings.JwtSecretKey);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = appSettings.JwtIssuer,
                    ValidateIssuer = true,
                    ValidAudience = "Any",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JwtSecretKey)),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                cfg.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // If SignalR is passed the token via query string, capture it
                        var accessToken = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}