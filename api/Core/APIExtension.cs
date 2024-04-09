using System.Text;

using EFDataAccess;

using Application;

using Application.Commands.Post;
using Application.Commands.Category;
using Application.Commands.User;
using Application.Commands.Like;
using Application.Commands.Comment;

using Application.Queries;
using Application.Queries.Post;
using Application.Queries.Category;
using Application.Queries.User;
using Application.Queries.Comment;

using Implementation.Validators.Post;
using Implementation.Validators.Category;
using Implementation.Validators.User;
using Implementation.Validators.Like;
using Implementation.Validators.Comment;

using Implementation.Commands.Post;
using Implementation.Commands.Category;
using Implementation.Commands.User;
using Implementation.Commands.Comment;
using Implementation.Commands.Like;

using Implementation.Queries;
using Implementation.Queries.Post;
using Implementation.Queries.Category;
using Implementation.Queries.Comment;
using Implementation.Queries.User;

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

            // Queries
            services.AddTransient<IGetPostsQuery, EFGetPostsQuery>();
            services.AddTransient<IGetPostQuery, EFGetPostQuery>();

            services.AddTransient<IGetCategoriesQuery, EFGetCategoriesQuery>();
            services.AddTransient<IGetCategoryQuery, EFGetCategoryQuery>();

            services.AddTransient<IGetCommentQuery, EFGetCommentQuery>();
            services.AddTransient<IGetCommentsQuery, EFGetCommentsQuery>();

            services.AddTransient<IGetUserQuery, EFGetUserQuery>();
            services.AddTransient<IGetUsersQuery, EFGetUsersQuery>();

            services.AddTransient<IGetUseCaseLogsQuery, EFGetUseCaseLogsQuery>();

            // Validators
            services.AddTransient<RegisterUserValidator>();

            services.AddTransient<CreatePostValidator>();
            services.AddTransient<CreateCategoryValidator>();
            services.AddTransient<CreateCommentValidator>();

            services.AddTransient<LikeValidator>();

            services.AddTransient<UpdatePostValidator>();
            services.AddTransient<UpdateCategoryValidator>();
            services.AddTransient<UpdateCommentValidator>();
            services.AddTransient<UpdateUserValidator>();

            services.AddTransient<DeletePostValidator>();
            services.AddTransient<DeleteCategoryValidator>();
            services.AddTransient<DeleteCommentValidator>();

            services.AddTransient<CreateUserValidator>();
            services.AddTransient<UpdateUserValidator>();
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
            });
        }
    }
}