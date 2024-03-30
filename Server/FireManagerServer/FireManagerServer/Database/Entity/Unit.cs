using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FireManagerServer.Database.Entity
{
    public class Unit:BaseDate
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Desc { get; set; }
        [ForeignKey("ApartmentId")]
        public string? ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }
        public List<Module>? Modules { get; set; }
    }
}
