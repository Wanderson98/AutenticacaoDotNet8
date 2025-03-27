using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebApi.Token
{
    public class TokenJwtBuilder
    {
        private SecurityKey securityKey = null;
        private string subjetc = "";
        private string issuer = "";
        private string audience = "";
        private Dictionary<string, string> claims = new Dictionary<string, string>();
        private int expiryInMinutes = 5;


        public TokenJwtBuilder AddSecurityKey(SecurityKey securityKey)
        {
            this.securityKey = securityKey;
            return this;
        }
        public TokenJwtBuilder AddSubject(string subjetc)
        {
            this.subjetc = subjetc;
            return this;
        }
        public TokenJwtBuilder AddIssuer(string issuer)
        {
            this.issuer = issuer;
            return this;
        }
        public TokenJwtBuilder AddAudience(string audience)
        {
            this.audience = audience;
            return this;
        }

        public TokenJwtBuilder AddClaims(Dictionary<string, string> claims)
        {
            this.claims = claims;
            return this;
        }
        public TokenJwtBuilder AddExpiry(int expiryInMinutes)
        {
            this.expiryInMinutes = expiryInMinutes;
            return this;
        }

        private void EnsureArguments()

        
        {
            if (this.securityKey == null)
                throw new ArgumentNullException("Security Key");
            if (string.IsNullOrWhiteSpace(this.issuer))
                throw new ArgumentNullException("Issuer");
            if (string.IsNullOrWhiteSpace(this.audience))
                throw new ArgumentNullException("Audience");
            if (string.IsNullOrWhiteSpace(this.subjetc))
                throw new ArgumentNullException("Subject");

        }


        public TokenJWT Builder()
        {
            EnsureArguments();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, this.subjetc),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }.Union(this.claims.Select(item => new Claim(item.Key, item.Value)));

            var token = new JwtSecurityToken(
                issuer: this.issuer,
                audience: this.audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                signingCredentials: new SigningCredentials(this.securityKey, SecurityAlgorithms.HmacSha256)
                );

            return new TokenJWT(token);
        }
    }
}
