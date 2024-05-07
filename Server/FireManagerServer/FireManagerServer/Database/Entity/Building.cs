using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireManagerServer.Database.Entity
{
    public class Building:BaseDate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Desc { get; set; }
        public List<Apartment>? Apartments { get; set; }
        public string? UserId { get; set; }
        public UserEntity? User { get; set; }
    }
}
