using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using EFDataAccess;
using Domain;
using System.Security.Cryptography;
using System.Security.Claims;
using Newtonsoft.Json;
using API.Core;
using Implementation;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly BlogContext _context;

        public AuthController(BlogContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> GoogleLogin(UserLoginModel model)
        {
            var user = _context.Users.Include(u => u.UserUseCases).Include(u => u.Role)
                .FirstOrDefault(x => x.Email == model.Email && x.IsActive == true);

            if (user == null)
            {
                var nameParts = model.Name.Split(' ');
                var firstName = nameParts[0];
                var lastName = string.Join(" ", nameParts.Skip(1));

                var generatedPassword = GenerateRandomPassword();
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
                if (role == null)
                {
                    return null;
                }

                var newUser = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Username = GenerateUsername(model.Name),
                    Password = HashPassword(generatedPassword),
                    Email = model.Email,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IdRole = 3,
                    ProfilePicture = model.GooglePhotoUrl,
                    Role = role
                };

                newUser.AddDefaultUseCasesForRole();

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                var token = GenerateJwtToken(newUser);
                return Ok(new { token });
            }
            else
            {
                try
                {
                    var token = GenerateJwtToken(user);
                    return Ok(new { token });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal Server Error: {ex.Message}");
                }
            }
        }

        private static string GenerateUsername(string name) => $"{name.ToLower().Replace(" ", "")}{Guid.NewGuid().ToString("N").Substring(0, 4)}";

        private static string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            var data = new byte[16];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(data);

            return new string(data.Select(b => chars[b % chars.Length]).ToArray());
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private string GenerateJwtToken(User newUser)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MyVeryVeryVerySecretKeyThatIsVeryVeryVeryBadHidden"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var actor = new JWTActor
            {
                Id = newUser.Id,
                Identity = newUser.Username,
                AllowedUseCases = newUser.UserUseCases.Select(x => x.IdUseCase),
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                RoleName = newUser.Role.Name,
                ProfilePicture = newUser.ProfilePicture
            };

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String, "asp_api_project"),
                new Claim(JwtRegisteredClaimNames.Iss, "asp_api_project", ClaimValueTypes.String, "asp_api_project"),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, "asp_api_project"),
                new Claim("IdUser", newUser.Id.ToString(), ClaimValueTypes.String, "asp_api_project"),
                new Claim("ActorData", JsonConvert.SerializeObject(actor), ClaimValueTypes.String, "asp_api_project")
            };

            var dateTimeNow = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: "asp_api_project",
                audience: "Any",
                claims: claims,
                notBefore: dateTimeNow,
                expires: dateTimeNow.AddMinutes(120),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class UserLoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string GooglePhotoUrl { get; set; }
    }
}