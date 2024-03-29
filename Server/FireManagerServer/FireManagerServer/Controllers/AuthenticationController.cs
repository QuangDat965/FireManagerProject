using FireManagerServer.Model.Request;
using FireManagerServer.Model.Response;
using FireManagerServer.Services.AuthenService;
using Microsoft.AspNetCore.Mvc;

namespace FireManagerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController:ControllerBase
    {
        private readonly IAuthenService authenService;

        public AuthenticationController(IAuthenService authenService)
        {
            this.authenService = authenService;
        }
        [HttpPost("login")]
        public async Task<ResponseModel<AuthenResponse>> Login ([FromBody] AuthenRequest request)
        {
            return await authenService.Login(request);
        }
        [HttpPost("register")]
        public async Task<ResponseModel<AuthenResponse>> Register([FromBody] Register request)
        {
            return await authenService.Register(request);
        }
    }
}
