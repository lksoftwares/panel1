using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

public class TokenValidator
{
    public bool ValidateToken(
        string token,
        string issuer,
        string audience,
        ICollection<SecurityKey> signingKeys,
        out JwtSecurityToken jwt
    )
    {
        if (string.IsNullOrEmpty(token))
        {
            // Token is null or empty, cannot proceed with validation
            jwt = null;
            return false;
        }

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKeys = signingKeys,
            ValidateLifetime = true
        };

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            jwt = (JwtSecurityToken)validatedToken;

            return true;
        }
        catch (SecurityTokenValidationException)
        {
            jwt = null;
            return false;
        }
    }
}
