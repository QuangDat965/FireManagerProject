using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireManagerServer.Database.Entity
{
    public class Apartment:BaseDate
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Desc { get; set; }
        List<Unit>? Rooms { get; set; }
        [ForeignKey("UserId")]
        public string? UserId { get; set; }
        public UserEntity? User { get; set; }
    }
}
