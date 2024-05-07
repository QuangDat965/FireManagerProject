using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireManagerServer.Database.Entity
{
    public class Module:BaseDate
    {
        public string Id { get; set; }
        public string ModuleName { get; set; }
        public bool? Status { get; set; } = false;
        public string? Desc { get; set; }
        public string? ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }
        public string? UserId { get; set; }
        public UserEntity? User { get; set; }
        public List<DeviceEntity>? Devices { get; set; }


    }
}
