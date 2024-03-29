using FireManagerServer.Database;
using FireManagerServer.Database.Entity;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace FireManagerServer.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly FireDbContext dbContext;

        public RoleService(FireDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Role> AddRole(Role role)
        {
            role.Id = Guid.NewGuid().ToString();
            dbContext.Add(role);
            dbContext.SaveChanges();
            return await Task.FromResult(role);
        }

        public async Task<List<Role>> GetAllRoles()
        {
            return await dbContext.Roles.ToListAsync();
        }

        public async Task<Role> GetById(string Id)
        {
            return await dbContext.Roles.FirstOrDefaultAsync(p=>p.Id==Id);
        }

        public async Task<Role> GetByName(string Name)
        {
            return await dbContext.Roles.FirstOrDefaultAsync(p => p.RoleName == Name);
        }

        public async Task<bool> SetUserRole(string userId, string roleId)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(p=>p.UserId==userId);
            user.RoleId = roleId;
            dbContext.Update(user);
            return await dbContext.SaveChangesAsync()>0;
        }
    }
}
