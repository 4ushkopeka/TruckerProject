using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tirajii.Data.Models
{
    public class Trailer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(TrailerType))]
        public int TypeId { get; set; }

        public TrailerType Type { get; set; }

        [Required]
        public string Colour { get; set; }

        [ForeignKey(nameof(Truck))]
        public int? TruckId { get; set; }

        public Truck? Truck { get; set; }

        [Required]
        public bool HasAdvertisement { get; set; }
    }
}
