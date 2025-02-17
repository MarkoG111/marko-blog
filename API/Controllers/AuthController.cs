using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.EntityFrameworkCore;
using EFDataAccess;
using Domain;
using System.Security.Cryptography;
using Newtonsoft.Json;
using API.Core;
using Implementation;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly BlogContext _context;
        private readonly JWTService _jwtService;

        public AuthController(BlogContext context, JWTService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
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

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                newUser.UpdateUseCasesForRole(_context);
                await _context.SaveChangesAsync();

                var token = _jwtService.GenerateToken(_jwtService.GenerateClaims(newUser));
                return Ok(new { token });
            }
            else
            {
                try
                {
                    var token = _jwtService.GenerateToken(_jwtService.GenerateClaims(user));
                    return Ok(new { token });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal Server Error: {ex.Message}");
                }
            }
        }

        private static string GenerateUsername(string name) => $"{name.ToLower().Replace(" ", "")}{Guid.NewGuid().ToString("N").Substring(0, 6)}";

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
    }

    public class UserLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string GooglePhotoUrl { get; set; }
    }
}