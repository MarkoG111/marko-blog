using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using EFDataAccess;
using Domain;
using Implementation;
using API.Core;
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
                Password = BCrypt.Net.BCrypt.HashPassword(GenerateRandomPassword()),
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

        private static string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var data = new byte[16];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(data);

            return new string(data.Select(b => chars[b % chars.Length]).ToArray());
        }
    }
}