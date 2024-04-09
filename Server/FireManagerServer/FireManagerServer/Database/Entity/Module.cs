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
        [ForeignKey("UnitId")]
        public string? UnitId { get; set; }
        public Unit? Unit { get; set; }
        [ForeignKey("UserId")]
        public string? UserId { get; set; }
        public UserEntity? User { get; set; }


    }
}
