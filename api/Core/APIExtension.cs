using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application;
using Application.Queries;
using Application.Queries.Blog;
using Application.Queries.Image;
using Application.Commands.Blog;
using Application.Commands.Category;
using Application.Commands.User;

using Implementation.Commands;
using Implementation.Commands.Blog;
using Implementation.Commands.Category;
using Implementation.Commands.User;

using Implementation.Queries;
using Implementation.Queries.Blog;
using Implementation.Queries.Image;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

using Implementation.Validators.Blog;
using Implementation.Validators.Category;
using Implementation.Validators.User;
using Application.Queries.User;
using Implementation.Queries.User;
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
            services.AddTransient<ICreateCategoryCommand, EFCreateCategoryCommand>();

            // Queries
            services.AddTransient<IGetBlogsQuery, EFGetBlogsQuery>();
            services.AddTransient<IGetBlogQuery, EFGetBlogQuery>();
            services.AddTransient<IGetUseCaseLogsQuery, EFGetUseCaseLogsQuery>();
            services.AddTransient<IGetImagesQuery, EFGetImagesQuery>();
            services.AddTransient<IGetUserQuery, EFGetUserQuery>();

            // Validators
            services.AddTransient<CreateBlogValidator>();
            services.AddTransient<CreateCategoryValidator>();
            services.AddTransient<RegisterUserValidator>();
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