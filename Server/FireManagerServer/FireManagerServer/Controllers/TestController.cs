using FireManagerServer.Common;
using FireManagerServer.Database;
using FireManagerServer.Model.Request;
using FireManagerServer.Model.Response;
using FireManagerServer.Service.JwtService;
using FireManagerServer.Services.ApartmentService;
using FireManagerServer.Services.AuthenService;
using FireManagerServer.Services.ModuleServices;
using FireManagerServer.Services.RuleServiceServices;
using FireManagerServer.Services.UnitServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRuleService _ruleService;
        private readonly FireDbContext _dbContext;
        private readonly IModuleService _moduleService;
        private readonly IApartmentService _apartmentService;
        private readonly IBuildingService _buildingService;
        private readonly Dictionary<string,string> _cache;

        public TestController(IJwtService jwtService, IAuthenService authenticationService,
            IRuleService ruleService,
            FireDbContext dbContext,
            IModuleService moduleService,
            IApartmentService apartmentService,
            IBuildingService buildingService,
            Dictionary<string, string> dic,
            IConfiguration configuration)
        {
            this.jwtService = jwtService;
            this.authenticationService = authenticationService;
            this.configuration = configuration;
            _ruleService = ruleService;
            _dbContext = dbContext;
            _moduleService = moduleService;
            _apartmentService = apartmentService;
            _buildingService = buildingService;
            _cache = dic;
        }
        [HttpGet("generateToken")]
        public IActionResult Test()
        {
            var dic = new Dictionary<string, string>();
            dic.Add("id", "ghusnduw");
            dic.Add("role", "admin");
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
        [HttpGet("getdic")]
        public async Task<IActionResult> GetDic()
        {
            var value = _cache.ToList();
            return Ok(value);
        }
        [HttpPost("mockrule")]
        public async Task<IActionResult> MockRule(int numberModule, string name)
        {
            //building create
            int buildingCount = 1;
            var apm = new ApartmentRequest() {
                Name = name,
                UserId = Constance.SystemAccount,
                Id = Guid.NewGuid().ToString(),
            };
            await _buildingService.Add(apm);
            //apartment create
            int numA = 1;
           
            for(int i=0; i< numberModule; i++)
            {
                var unitrq = new UnitRequest()
                {
                    ApartmentId = apm.Id,
                    Name =$"{i+1}",
                    
                };
                await _apartmentService.Add(unitrq);
            }
           

            // add module to apartment
            var allapartments = await _dbContext.Apartments.Where(x=>x.BuldingId == apm.Id).ToListAsync();
            var allModules = await _dbContext.Modules.ToListAsync();
            for(int i=0;i<allapartments.Count;i++)
            {
                await _moduleService.AddToRoom(allapartments[i].Id, allModules[i].Id);
            }
            // create rule for module
            int rs = 0;
            // Tải tất cả các thiết bị từ cơ sở dữ liệu vào bộ nhớ
            var allDevices = await _dbContext.Devices.ToListAsync();

            // Thực hiện GroupBy và ToDictionary trên dữ liệu trong bộ nhớ
            var devicesGroupedByModule = allDevices
                .GroupBy(x => x.ModuleId)
                .ToDictionary(g => g.Key, g => g.ToList());
            foreach (var module in allModules)
            {
                if (devicesGroupedByModule.TryGetValue(module.Id, out var devices))
                {
                    var ruleAdd = new RuleAddDto();
                    ruleAdd.ModuleId = module.Id;
                    ruleAdd.isFire = true;
                    ruleAdd.isActive = true;
                    ruleAdd.Desc = module.Id;
                    ruleAdd.TopicThreshholds = new();
                    foreach (var device in devices)
                    {
                        var th = new TopicThreshHoldDto();
                        th.DeviceId = device.Id;
                        th.ThreshHold =device.Type ==DeviceType.R? 40:1;
                        th.TypeCompare = TypeCompare.Bigger;
                        ruleAdd.TopicThreshholds.Add(th);
                    }
                    var change = await _ruleService.Create(ruleAdd);
                    if(change==true)
                    {
                        rs++;
                    }
                }
            }
            return Ok(rs);
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
            var task = new Task(() =>
            {
                while (true)
                {
                    var gaspay = new Random().Next(1023, 2000);
                    var temppay = new Random().Next(30, 40);
                    client.Publish(gaspush, Encoding.UTF8.GetBytes(gaspay.ToString()));
                    client.Publish(tempaturepush, Encoding.UTF8.GetBytes(temppay.ToString()));
                    client.Publish(window, Encoding.UTF8.GetBytes(0.ToString()));

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
