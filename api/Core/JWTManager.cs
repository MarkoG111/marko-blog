using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EFDataAccess;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;


namespace api.Core
{
    public class JWTManager
    {
        private readonly BlogContext _context;

        public JWTManager(BlogContext context)
        {
            _context = context;
        }

        private static byte[] GenerateRandomBytes(int length)
        {
            byte[] randomBytes = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        public string MakeToken(string username, string password)
        {
            var user = _context.Users.Include(u => u.UserUseCases)
                .FirstOrDefault(x => x.Username == username && x.Password == password && x.IsActive == true);

            if (user == null)
            {
                return null;
            }

            byte[] keyBytes = GenerateRandomBytes(32); // 32 bajta = 256 bita
            string secretKey = Convert.ToBase64String(keyBytes);

            var actor = new JWTActor
            {
                Id = user.Id,
                AllowedUseCases = user.UserUseCases.Select(x => x.IdUseCase),
                Identity = user.Username
            };

            var issuer = "asp_api";
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String, issuer),
                new Claim(JwtRegisteredClaimNames.Iss, "asp_api", ClaimValueTypes.String, issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, issuer),
                new Claim("UserId", actor.Id.ToString(), ClaimValueTypes.String, issuer),
                new Claim("ActorData", JsonConvert.SerializeObject(actor), ClaimValueTypes.String, issuer)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var dateTimeNow = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: "Any",
                claims: claims,
                notBefore: dateTimeNow,
                expires: dateTimeNow.AddMinutes(120),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}