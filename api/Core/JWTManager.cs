using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;

using EFDataAccess;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using Microsoft.IdentityModel.Tokens;

namespace API.Core
{
    public class JWTManager
    {
        private readonly BlogContext _context;
        private readonly string _issuer;
        private readonly string _secretKey;

        public JWTManager(BlogContext context, string issuer, string secretKey)
        {
            _context = context;
            _issuer = issuer;
            _secretKey = secretKey;
        }

        public string MakeToken(string username, string password)
        {
            var user = _context.Users.Include(u => u.UserUseCases).Include(u => u.Role)
                .FirstOrDefault(x => x.Username == username && x.Password == password && x.IsActive == true);

            if (user == null)
            {
                return null;
            }

            var actor = new JWTActor
            {
                Id = user.Id,
                AllowedUseCases = user.UserUseCases.Select(x => x.IdUseCase),
                Identity = user.Username,

                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                RoleName = user.Role.Name,
                ProfilePicture = user.ProfilePicture
            };

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String, _issuer),
                new Claim(JwtRegisteredClaimNames.Iss, "asp_api_project", ClaimValueTypes.String, _issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _issuer),
                new Claim("IdUser", actor.Id.ToString(), ClaimValueTypes.String, _issuer),
                new Claim("ActorData", JsonConvert.SerializeObject(actor), ClaimValueTypes.String, _issuer)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var dateTimeNow = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: _issuer,
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