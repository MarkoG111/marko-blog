using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application;

using Application.Commands.Blog;
using Application.Commands.Category;
using Application.Commands.User;
using Application.Commands.Like;
using Application.Commands.Comment;

using Application.Queries;
using Application.Queries.Blog;
using Application.Queries.Category;
using Application.Queries.User;
using Application.Queries.Comment;

using Implementation.Validators.Blog;
using Implementation.Validators.Category;
using Implementation.Validators.User;
using Implementation.Validators.Like;
using Implementation.Validators.Comment;

using Implementation.Commands.Blog;
using Implementation.Commands.Category;
using Implementation.Commands.User;
using Implementation.Commands.Comment;
using Implementation.Commands.Like;

using Implementation.Queries;
using Implementation.Queries.Blog;
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

            services.AddTransient<ICreateBlogCommand, EFCreateBlogCommand>();
            services.AddTransient<IUpdateBlogCommand, EFUpdateBlogCommand>();
            services.AddTransient<IDeleteBlogCommand, EFDeleteBlogCommand>();
            services.AddTransient<IUpdatePersonalBlogCommand, EFUpdatePersonalBlogCommand>();
            services.AddTransient<IDeletePersonalBlogCommand, EFDeletePersonalBlogCommand>();

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

            services.AddTransient<ILikeBlogCommand, EFLikeBlogCommand>();

            // Queries
            services.AddTransient<IGetBlogsQuery, EFGetBlogsQuery>();
            services.AddTransient<IGetBlogQuery, EFGetBlogQuery>();

            services.AddTransient<IGetCategoriesQuery, EFGetCategoriesQuery>();
            services.AddTransient<IGetCategoryQuery, EFGetCategoryQuery>();

            services.AddTransient<IGetCommentQuery, EFGetCommentQuery>();

            services.AddTransient<IGetUserQuery, EFGetUserQuery>();

            services.AddTransient<IGetUseCaseLogsQuery, EFGetUseCaseLogsQuery>();

            // Validators
            services.AddTransient<RegisterUserValidator>();

            services.AddTransient<CreateBlogValidator>();
            services.AddTransient<CreateCategoryValidator>();
            services.AddTransient<CreateCommentValidator>();
            services.AddTransient<CreateUserValidator>();

            services.AddTransient<LikeValidator>();

            services.AddTransient<UpdateBlogValidator>();
            services.AddTransient<UpdateCategoryValidator>();
            services.AddTransient<UpdateCommentValidator>();
            services.AddTransient<UpdateUserValidator>();

            services.AddTransient<DeleteBlogValidator>();
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

        public static void AddJWT(this IServiceCollection services)
        {
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
                    ValidIssuer = "asp_api",
                    ValidateIssuer = true,
                    ValidAudience = "Any",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsMyVerySecretKey")),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}