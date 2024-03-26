using API.Core;
using Application;
using EFDataAccess;
using Implementation.Logging;

using Microsoft.OpenApi.Models;
using Application.Commands.Email;
using Implementation.Commands.Email;

using Newtonsoft.Json;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = new AppSettings();

            Configuration.Bind(appSettings);

            services.AddTransient<BlogContext>();

            services.LoadUseCases();

            services.AddTransient<IUseCaseLogger, EFDatabaseLogger>();

            services.AddHttpContextAccessor();
            services.AddApplicationActor();

            services.AddJWT(appSettings);

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

            services.AddTransient<IEmailSender, SMTPEmailSender>();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //  Proverava da li je aplikacija pokrenuta u razvojnom okruženju. Ako jeste, dodaje se middleware komponenta DeveloperExceptionPage, koja prikazuje detaljne informacije o izuzecima prilikom razvoja aplikacije.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Dodaje middleware za rutiranje, što omogućava aplikaciji da odredi koji kod će se izvršiti na osnovu dolaznog HTTP zahteva.
            app.UseRouting();
            // Dodaje middleware koji omogućava serveru da poslužuje statičke datoteke, poput HTML, CSS, JavaScript i slika.
            app.UseStaticFiles();

            // Dodaje middleware za autorizaciju, koji omogućava aplikaciji da proveri da li korisnik ima odgovarajuće dozvole za pristup određenom resursu.
            app.UseAuthorization();
            // Dodaje middleware za autentifikaciju, koji omogućava aplikaciji da autentifikuje korisnike na osnovu pristiglih kredencijala ili tokena.
            app.UseAuthentication();

            // Dodaje middleware za obrađivanje Cross-Origin Resource Sharing (CORS) zahteva. Ovaj middleware omogućava definisanje politika CORS-a koje određuju koje origin domene su dozvoljene da pristupaju resursima na serveru.
            app.UseCors(x =>
            {
                x.AllowAnyOrigin();
                x.AllowAnyMethod();
                x.AllowAnyHeader();
            });

            // Dodaje middleware za podršku Swagger-u, koji generiše dokumentaciju API-ja na osnovu definicija ruta i kontrolera u aplikaciji.
            app.UseSwagger();

            // Dodaje middleware koji generiše HTML interfejs za Swagger dokumentaciju, omogućavajući pregled API specifikacija putem web pregledača.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlogAPI v1");
            });

            // Dodaje middleware komponentu GlobalExceptionHandler koja obrađuje sve izuzetke koji nisu već obrađeni i pruža odgovarajući odgovor korisniku ili aplikaciji.
            app.UseMiddleware<GlobalExceptionHandler>();

            // Dodaje middleware za definisanje krajnjih tačaka (endpoints) aplikacije, tj. mapiranje HTTP zahteva na odgovarajuće akcije u kontrolerima.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}