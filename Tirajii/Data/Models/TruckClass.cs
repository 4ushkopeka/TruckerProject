using System.ComponentModel.DataAnnotations;

namespace Tirajii.Data.Models
{
    public class TruckClass
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Truck> Trucks { get; set; } = new List<Truck>();

    }
}
