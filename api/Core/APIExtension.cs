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

using Implementation.Commands;
using Implementation.Commands.Blog;
using Implementation.Commands.Category;
using Implementation.Queries;
using Implementation.Queries.Blog;
using Implementation.Queries.Image;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Implementation.Validators.Blog;
using Implementation.Validators.Category;


namespace api.Core
{
    public static class APIExtension
    {
        public static void LoadUseCases(this IServiceCollection services)
        {
            services.AddTransient<UseCaseExecutor>();

            // Commands
            services.AddTransient<ICreateBlogCommand, EFCreateBlogCommand>();
            services.AddTransient<ICreateCategoryCommand, EFCreateCategoryCommand>();

            // Queries
            services.AddTransient<IGetBlogsQuery, EFGetBlogsQuery>();
            services.AddTransient<IGetBlogQuery, EFGetBlogQuery>();
            services.AddTransient<IGetUseCaseLogsQuery, EFGetUseCaseLogsQuery>();
            services.AddTransient<IGetImagesQuery, EFGetImagesQuery>();

            // Validators
            services.AddTransient<CreateBlogValidator>();
            services.AddTransient<CreateCategoryValidator>();
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