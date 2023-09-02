using Core.Security.Encryption.Helpers;
using Core.Security.Entities;
using Core.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Core.Security.JWT
{
    public class JwtHelper : ITokenHelper
    {
        private readonly TokenOptions _tokenOptions;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public JwtHelper(IConfiguration configuration, IOptions<TokenValidationParameters> tokenValidationParameters, IOptions<TokenOptions> tokenOptions)
        {

            _tokenOptions = tokenOptions.Value;
            _tokenValidationParameters = tokenValidationParameters.Value;

        }




        public AccessToken CreateToken(User user, IList<OperationClaim> operationClaims)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            JwtSecurityToken jwt = CreateJwtSecurityToken(user, _tokenOptions, operationClaims);

            string? token = handler.WriteToken(jwt);
            return new AccessToken() { Token = token, ExpireTime = jwt.ValidTo };

        }

        public RefreshToken CreateRefreshToken(User user, string ipAdress)
        {


            string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            RefreshToken refreshToken = new()
            {
                UserId = user.Id,
                Token = token,
                Created = DateTime.UtcNow,
                CreatedByIp = ipAdress,
                ExpireDate = DateTime.UtcNow.AddDays(_tokenOptions.RefreshTokenTTL),
            };

            return refreshToken;

        }

        public bool IsTokenExpired(string token)
        {
           if (string.IsNullOrEmpty(token))
                return true;

           JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
           handler.ValidateToken(token, _tokenValidationParameters, out SecurityToken validatedToken);
           return validatedToken.ValidTo < DateTime.UtcNow;
        }

        private JwtSecurityToken CreateJwtSecurityToken(User user, TokenOptions tokenOptions, IList<OperationClaim> operationClaims)
        {
            SecurityKey securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            SigningCredentials credentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            JwtSecurityToken jwt = new(_tokenOptions.Issuer, _tokenOptions.Audience,
                                                    CreateAndSetClaims(user, operationClaims),
                                                    DateTime.UtcNow, DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenExpiration), credentials);
            return jwt;
        }

        private IEnumerable<Claim> CreateAndSetClaims(User user, IList<OperationClaim> operationClaims)
        {
            List<Claim> claims = new List<Claim>();
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddRoles(operationClaims.Select(x => x.Name).ToArray());
            return claims;
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            return null;
        }


    }
}
