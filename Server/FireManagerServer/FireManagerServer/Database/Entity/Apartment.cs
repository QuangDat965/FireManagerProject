using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireManagerServer.Database.Entity
{
    public class Apartment:BaseDate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Desc { get; set; }
        public string? BuldingId { get; set; }
        public Building? Building { get; set; }
        public List<Module>? Modules { get; set; }
        public bool IsFire { get; set; }
        public List<ApartmentNeighbour>? ApartmentNeighbours { get; set; }
        public List<ApartmentNeighbour>? ApartmentNeighbours2 { get; set; }
    }
}
