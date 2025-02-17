namespace API.Core
{
    public class JWTSettings
    {
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
        public string JwtSecretKey { get; set; }
        public int TokenExpiryMinutes { get; set; }

        public JWTSettings()
        {
            JwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new ArgumentNullException(nameof(JwtIssuer), "JWT_ISSUER env variable is required");
            JwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new ArgumentNullException(nameof(JwtAudience), "JWT_AUDIENCE env variable is required");
            JwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? throw new ArgumentNullException(nameof(JwtSecretKey), "JWT_SECRET_KEY env variable is required");
            TokenExpiryMinutes = int.TryParse(Environment.GetEnvironmentVariable("JWT_EXPIRY_MINUTES"), out int expiry) ? expiry : 120;
        }
    }
}
