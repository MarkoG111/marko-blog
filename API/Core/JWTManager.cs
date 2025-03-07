using Domain;
using EFDataAccess;
using Microsoft.EntityFrameworkCore;
using API.Services;
using Application.Exceptions;

namespace API.Core
{
    public class JWTManager
    {
        private readonly BlogContext _context;
        private readonly JWTService _jwtService;

        public JWTManager(BlogContext context, JWTService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public string MakeToken(string username, string password)
        {
            var user = FetchUser(username, password);

            var claims = _jwtService.GenerateClaims(user);

            return _jwtService.GenerateToken(claims);
        }

        private User FetchUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new AuthenticationException("Username and password are required.");
            }

            var user = _context.Users
                .Include(u => u.UserUseCases)
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Username == username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                throw new AuthenticationException("Invalid username or password.");
            }

            return user;
        }
    }
}