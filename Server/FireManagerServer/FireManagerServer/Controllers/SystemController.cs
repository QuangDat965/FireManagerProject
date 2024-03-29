using Microsoft.AspNetCore.Mvc;

namespace FireManagerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SystemController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public SystemController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [HttpGet,Route("id")]
        public async Task<string> GetIdSystem ()
        {
            return await Task.FromResult(configuration.GetValue<string>("SystemId"));
        }
    }
}
