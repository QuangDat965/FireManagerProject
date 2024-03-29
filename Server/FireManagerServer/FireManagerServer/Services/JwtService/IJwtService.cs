using FireManagerServer.Model;

namespace FireManagerServer.Service.JwtService
{
    public interface IJwtService
    {
       public string GenerateToken(Dictionary<string, string> claims, int expirationMinutes = 30);
       public  Dictionary<string,string> VerifyToken(string token);
        string GetId(string token);
    }
}
