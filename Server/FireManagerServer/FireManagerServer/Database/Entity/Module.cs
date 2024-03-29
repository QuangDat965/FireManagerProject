using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireManagerServer.Database.Entity
{
    public class Module:BaseDate
    {
        [Key]
        public string Id { get; set; }
        public string ModuleName { get; set; }
        public bool? Status { get; set; } = false;
        public string? Desc { get; set; }
        [ForeignKey("RoomId")]
        public string? RoomId { get; set; }
        public Unit? Room { get; set; }
        [ForeignKey("RoomId")]
        public string? UserId { get; set; }
        public UserEntity? User { get; set; }


    }
}
