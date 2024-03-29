using FireManagerServer.Model.Request;
using FireManagerServer.Model.Response;

namespace FireManagerServer.Services.AuthenService
{
    public interface IAuthenService
    {
        public Task<ResponseModel<AuthenResponse>> Login(AuthenRequest request);
        public Task<ResponseModel<AuthenResponse>> Register(Register request);
    }
}
