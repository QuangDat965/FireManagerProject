namespace FireManagerServer.Common
{
    public static class CommomFuncition
    {
        public static string GetTokenBear(HttpContext context)
        {
           string tokenBear =  context.Request.Headers["Authorization"].ToString();
            var token = tokenBear.Substring("Bearer ".Length);
            return token;
        }
    }
}
