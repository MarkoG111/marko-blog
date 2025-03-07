using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EFDataAccess;
using Domain;
using Implementation;
using BCrypt.Net;
using Application.DataTransfer.Auth;

namespace API.Services
{
    public class OAuthService
    {
        private readonly BlogContext _context;
        private readonly JWTService _jwtService;

        public OAuthService(BlogContext context, JWTService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<string?> AuthenticateUser(OAuthUserDto requestDto)
        {
            var user = await _context.Users
                .Include(u => u.UserUseCases)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(x => x.Email == requestDto.Email && x.IsActive);

            if (user != null)
            {
                return _jwtService.GenerateToken(_jwtService.GenerateClaims(user));
            }

            return await RegisterUser(requestDto);
        }

        private async Task<string?> RegisterUser(OAuthUserDto requestDto)
        {
            var nameParts = requestDto.Name.Split(' ');
            var firstName = nameParts[0];
            var lastName = string.Join(" ", nameParts.Skip(1));

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            if (role == null)
            {
                throw new Exception("User role does not exist in the database.");
            }

            var newUser = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Username = GenerateUsername(requestDto.Name),
                Password = BCrypt.Net.BCrypt.HashPassword(GenerateSimplePassword()),
                Email = requestDto.Email,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IdRole = role.Id,
                ProfilePicture = requestDto.GooglePhotoUrl,
                Role = role
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            newUser.UpdateUseCasesForRole(_context);
            await _context.SaveChangesAsync();

            return _jwtService.GenerateToken(_jwtService.GenerateClaims(newUser));
        }

        private static string GenerateUsername(string name) => $"{name.ToLower().Replace(" ", "")}{Guid.NewGuid().ToString("N").Substring(0, 6)}";

        private static string GenerateSimplePassword()
        {
            return $"User{new Random().Next(1000, 9999)}";
        }
    }
}