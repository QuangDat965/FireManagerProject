
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FireManagerServer.Service.JwtService;
using Microsoft.IdentityModel.Tokens;

public class JwtService:IJwtService
{
    private readonly IConfiguration configuration;

    public JwtService(IConfiguration configuration)
    {
      this.configuration = configuration;
    }

    public string GenerateToken(Dictionary<string, string> claims, int expirationMinutes = 1000)
    {
        var secretKey = configuration.GetValue<string>("ScretKey");
        // Tạo danh sách claims từ dữ liệu đầu vào
        var identityClaims = new List<Claim>();
        foreach (var claim in claims)
        {
            identityClaims.Add(new Claim(claim.Key, claim.Value));
        }

        // Tạo symmetric security key từ secret key
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));

        // Tạo signing credentials
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Tạo token
        var token = new JwtSecurityToken(
            claims: identityClaims,
            expires: DateTime.UtcNow.AddDays(expirationMinutes),
            signingCredentials: creds
        );

        // Chuyển đổi token thành chuỗi JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }

    public string GetId(string token)
    {
        var claims = VerifyToken(token);
        claims.TryGetValue("UserId", out var id);
        return id;
    }

    public Dictionary<string,string> VerifyToken(string token)
    {
        try
        {
            var secretKey = configuration.GetValue<string>("ScretKey");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience= false,
                ValidateIssuer=false,
                IssuerSigningKey = key,
            };

            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            var claims = new Dictionary<string, string>();
            foreach (var claim in principal.Claims)
            {
                claims.Add(claim.Type, claim.Value);
            }

            return claims;
            // Lấy thông tin từ claims
        }
        catch (Exception)
        {
            // Xác thực thất bại
            return null;
        }
    }
    
}

