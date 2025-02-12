using API.Core;
using Application;
using EFDataAccess;
using Application.Settings;
using Application.Commands.Email;
using Implementation.Logging;
using Implementation.Commands.Email;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.SignalR;
using DotNetEnv;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Env.Load();
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = new AppSettings();

            Configuration.Bind(appSettings);

            services.AddControllers();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddSentry(o =>
                {
                    o.Dsn = "https://42856e7ceca42c96759e8d360f357474@o4508383067504640.ingest.de.sentry.io/4508383079235664";
                    o.TracesSampleRate = 1.0;
                });
            });

            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            services.AddTransient<BlogContext>();

            services.LoadUseCases();

            services.AddTransient<IUseCaseLogger, EFDatabaseLogger>();

            services.AddHttpContextAccessor();
            services.AddApplicationActor();

            services.AddJWT(appSettings);

            services.AddSignalR();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BlogAPI", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            services.Configure<EmailSettings>(options =>
            {
                options.SmtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER");
                options.SmtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587");
                options.SenderEmail = Environment.GetEnvironmentVariable("SENDER_EMAIL");
                options.SenderPassword = Environment.GetEnvironmentVariable("SENDER_PASSWORD");

            });
            services.AddTransient<IEmailSender, SMTPEmailSender>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Checks if the application is running in a development environment. If it is, adds the DeveloperExceptionPage middleware, which displays detailed information about exceptions during application development.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Adds middleware for routing, which enables the application to determine which code to execute based on the incoming HTTP request.
            app.UseRouting();

            // Adds middleware that allows the server to serve static files, such as HTML, CSS, JavaScript, and images.
            app.UseStaticFiles();

            // Adds middleware for authentication, which allows the application to authenticate users based on incoming credentials or tokens.
            app.UseAuthentication();

            // Adds middleware for authorization, which allows the application to check if the user has the necessary permissions to access a particular resource.
            app.UseAuthorization();

            app.UseDefaultFiles();

            // Adds middleware for handling Cross-Origin Resource Sharing (CORS) requests. This middleware allows defining CORS policies that determine which origin domains are allowed to access resources on the server.
            app.UseCors(x =>
            {
                x.AllowAnyMethod();
                x.AllowAnyHeader();
                x.AllowCredentials();
                x.AllowAnyMethod();
            });

            // Adds middleware for Swagger support, which generates API documentation based on route and controller definitions in the application.
            app.UseSwagger();

            // Adds middleware that generates an HTML interface for the Swagger documentation, allowing the API specifications to be viewed in a web browser.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlogAPI v1");
            });

            // Adds the GlobalExceptionHandler middleware component, which handles all exceptions that have not been processed yet and provides an appropriate response to the user or application.
            app.UseMiddleware<GlobalExceptionHandler>();

            app.UseSentryTracing();

            // Adds middleware for defining endpoints in the application, i.e., mapping HTTP requests to corresponding actions in controllers.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/notificationsHub");
            });
        }
    }
}