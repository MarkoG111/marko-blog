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

using System.Security.Cryptography;

namespace api.Core
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

            // Queries
            services.AddTransient<IGetBlogsQuery, EFGetBlogsQuery>();
            services.AddTransient<IGetBlogQuery, EFGetBlogQuery>();
            services.AddTransient<IGetUseCaseLogsQuery, EFGetUseCaseLogsQuery>();
            services.AddTransient<IGetUserQuery, EFGetUserQuery>();

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

        private static byte[] GenerateRandomBytes(int length)
        {
            byte[] randomBytes = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        public static void AddJWT(this IServiceCollection services)
        {
            byte[] keyBytes = GenerateRandomBytes(32); // 32 bajta = 256 bita
            string secretKey = Convert.ToBase64String(keyBytes);

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