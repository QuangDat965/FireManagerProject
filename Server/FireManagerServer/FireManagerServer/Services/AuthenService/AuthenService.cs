using FireManagerServer.Common;
using FireManagerServer.Database;
using FireManagerServer.Database.Entity;
using FireManagerServer.Model.Request;
using FireManagerServer.Model.Response;
using FireManagerServer.Service.JwtService;
using FireManagerServer.Services.RoleService;
using Microsoft.EntityFrameworkCore;

namespace FireManagerServer.Services.AuthenService
{
    public class AuthenService : IAuthenService
    {
        private readonly FireDbContext dbContext;
        private readonly IRoleService roleService;
        private readonly IJwtService jwtService;

        public AuthenService(FireDbContext dbContext, IJwtService jwtService,IRoleService roleService)
        {
            this.dbContext = dbContext;
            this.roleService = roleService;
            this.jwtService = jwtService;
        }
        public async Task<ResponseModel<AuthenResponse>> Login(AuthenRequest request)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(p=>p.Email==request.Email);
            if (user == null)
            {
                return new ResponseModel<AuthenResponse>()
                {
                    Code = -1,
                    Message = "Email không tồn tại"
                };
            }
            bool isvalidPassword = user.PassWord == request.Password;
            if(!isvalidPassword)
            {
                return new ResponseModel<AuthenResponse>() { Code=-2,Message="Sai mật khẩu"};
            }
            var claims = GetClaims(user);
            var role = await roleService.GetById(user.RoleId);
            return new ResponseModel<AuthenResponse>()
            {
                Code = 0,
                Message = "Sucess",
                Data = new AuthenResponse()
                {
                    Token = jwtService.GenerateToken(claims),
                    Role= role.RoleName
                }
            };


        }
        private Dictionary<string, string> GetClaims(UserEntity user)
        {
            var claims = new Dictionary<string, string>();
            claims.Add("UserId", user.UserId);
            claims.Add("Email", user.Email);
            return claims;

        }

        public async Task<ResponseModel<AuthenResponse>> Register(Register request)
        {
            var checkExitEmail = await dbContext.Users.FirstOrDefaultAsync(p=>p.Email==request.Email);
            if(checkExitEmail!=null)
            {
                return new ResponseModel<AuthenResponse>()
                {
                    Code = -1,
                    Message = "Email đã tồn tại"
                };
            }
            var role = await roleService.GetByName(RoleEnum.USER.ToString());
            if(role==null)
            {
                role = await roleService.AddRole(new Role()
                {
                    Id = Guid.NewGuid().ToString(),
                    RoleName = RoleEnum.USER.ToString()
                });
            }
            var user = new UserEntity()
            {
                UserId = Guid.NewGuid().ToString(),
                Email = request.Email,
                PhoneNumber = request.NumberPhone,
                PassWord = request.Password,
                FullName = request.FullName,
                RoleId = role.Id,
            };
            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return new ResponseModel<AuthenResponse>()
            {
                Code = 0,
                Message = "Success",
                Data = new AuthenResponse()
                {
                    Token =  jwtService.GenerateToken(GetClaims(user))
                }
            };
        }
    }
}
