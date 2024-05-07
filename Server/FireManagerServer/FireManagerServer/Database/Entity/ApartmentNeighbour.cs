using System.ComponentModel.DataAnnotations;

namespace FireManagerServer.Database.Entity
{
    public class ApartmentNeighbour
    {
        public string? ApartmentId { get; set; }
        public Apartment? Apartment1 { get; set; }

        public string? NeighbourId { get; set; }
        public Apartment? Apartment2 { get; set; }

    }
}
