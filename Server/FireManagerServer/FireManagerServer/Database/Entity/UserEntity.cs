using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireManagerServer.Database.Entity
{
    public class UserEntity:BaseDate
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string PassWord { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Image { get; set; }
        public string? Adress { get; set; }
        public string RoleId { get; set; }
        public Role Role { get; set; }
        public List<Building> Buildings { get; set; }
        public List<Module> Modules { get; set; }
        public List<HistoryData> HistoryDatas { get; set; }
    }
}
