using EFDataAccess;
using Microsoft.EntityFrameworkCore;
using Domain;

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

            if (user == null)
            {
                return null;
            }

            var claims = _jwtService.GenerateClaims(user);
            
            return _jwtService.GenerateToken(claims);
        }

        public User FetchUser(string username, string password)
        {
            return _context.Users.Include(u => u.UserUseCases).Include(u => u.Role).FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}