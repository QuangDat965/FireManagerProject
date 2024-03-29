using FireManagerServer.Common;
using FireManagerServer.Model.Request;
using FireManagerServer.Model.Response;
using FireManagerServer.Service.JwtService;
using FireManagerServer.Services.AuthenService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Unicode;
using uPLibrary.Networking.M2Mqtt;

namespace FireManagerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {

        private readonly IJwtService jwtService;
        private readonly IAuthenService authenticationService;
        private readonly IConfiguration configuration;

        public TestController(IJwtService jwtService, IAuthenService authenticationService,
            IConfiguration configuration)
        {
            this.jwtService= jwtService;
            this.authenticationService = authenticationService;
            this.configuration = configuration;
        }
        [HttpGet( "generateToken")]
        public IActionResult Test()
        {
           var dic = new Dictionary<string, string>();
            dic.Add("id","ghusnduw");
            dic.Add("role","admin");
            return Ok(jwtService.GenerateToken(dic));
        }
        [HttpPost("verify")]
        [VerifyToken]
        public IActionResult TestV(string token)
        {
           
            return Ok(jwtService.VerifyToken(token));
        }
        [HttpPost("login")]
        public async Task<ResponseModel<AuthenResponse>> Login([FromBody] AuthenRequest request)
        {
            return await authenticationService.Login(request);
        }
        [HttpPost("register")]
        public async Task<ResponseModel<AuthenResponse>> Register([FromBody] Register request)
        {
            return await authenticationService.Register(request);
        }
        [HttpGet("mockdata")]
        public async Task MockData()
        {
            MqttClient client = new MqttClient("broker.emqx.io");
            client.Connect(Guid.NewGuid().ToString());
            string systemid = configuration.GetValue<string>("SystemId").ToString();
            var gaspush = $"{systemid}/ESP32-1/D2/R/Gas";
            var tempaturepush = $"{systemid}/ESP32-1/D3/R/Tempature";
            var window = $"{systemid}/ESP32-1/D4/W/Window";

            var gaspush2 = $"{systemid}/ESP32-2/D2/R/Gas";
            var tempaturepush2 = $"{systemid}/ESP32-2/D3/R/Tempature";
            var window2 = $"{systemid}/ESP32-2/D4/W/Window";
            var task = new Task(()=>
            {
                while (true)
                {
                    var gaspay = new Random().Next(1023,2000);
                    var temppay = new Random().Next(30,40);
                    client.Publish(gaspush,Encoding.UTF8.GetBytes(gaspay.ToString()));
                    client.Publish(tempaturepush,Encoding.UTF8.GetBytes(temppay.ToString()));
                    client.Publish(window,Encoding.UTF8.GetBytes(0.ToString()));

                    client.Publish(gaspush2, Encoding.UTF8.GetBytes(gaspay.ToString()));
                    client.Publish(tempaturepush2, Encoding.UTF8.GetBytes(temppay.ToString()));
                    client.Publish(window2, Encoding.UTF8.GetBytes(0.ToString()));
                    Thread.Sleep(5000);
                }
                
            });
            task.Start();

        }
    }
}
