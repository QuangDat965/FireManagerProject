using FireManagerServer.Database.Entity;

namespace FireManagerServer.Services.RoleService
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllRoles();
        Task<Role> GetById(string Id);
        Task<Role> GetByName(string Name);
        Task<bool> SetUserRole(string userId, string roleId);
        Task<Role> AddRole(Role role);
       

    }
}
