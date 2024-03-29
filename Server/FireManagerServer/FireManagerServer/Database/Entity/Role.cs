using System.ComponentModel.DataAnnotations;

namespace FireManagerServer.Database.Entity
{
    public class Role:BaseDate
    {
        [Key]
        public string? Id { get; set; }
        public string? RoleName { get; set; }
        public List<UserEntity>? Users { get; set; }
    }
}
