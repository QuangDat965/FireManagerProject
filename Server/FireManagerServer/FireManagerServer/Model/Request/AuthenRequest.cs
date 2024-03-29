namespace FireManagerServer.Model.Request
{
    public class AuthenRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }

    }
    public class Register:AuthenRequest
    {
        public string? FullName { get; set; }
        public string? NumberPhone { get; set; }
    }
}
