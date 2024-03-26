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

                var newUser = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Username = GenerateUsername(model.Name),
                    Email = model.Email,
                    Password = HashPassword(generatedPassword),
                    IdRole = 3,
                    ProfilePicture = model.GooglePhotoUrl,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

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

        private static string GenerateUsername(string name)
        {
            string[] nameParts = name.ToLower().Split(' ');
            string username = string.Join("", nameParts) + new Random().Next(1000, 9999);
            return username;
        }

        private static string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var passwordChars = new char[16];

            for (int i = 0; i < passwordChars.Length; i++)
            {
                passwordChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(passwordChars);
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

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String, "asp_api_project"),
                new Claim(JwtRegisteredClaimNames.Iss, "asp_api_project", ClaimValueTypes.String, "asp_api_project"),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, "asp_api_project"),
                new Claim("IdUser", newUser.Id.ToString(), ClaimValueTypes.String, "asp_api_project"),
                new Claim("ActorData", JsonConvert.SerializeObject(new {
                    newUser.Id,
                    newUser.FirstName,
                    newUser.LastName,
                    newUser.Username,
                    newUser.Email,
                    newUser.ProfilePicture
                }), ClaimValueTypes.String, "asp_api_project")
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
        public string Email { get; set; }
        public string Name { get; set; }
        public string GooglePhotoUrl { get; set; }
    }
}