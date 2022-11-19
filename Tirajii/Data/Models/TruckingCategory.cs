using System.ComponentModel.DataAnnotations;

namespace Tirajii.Data.Models
{
    public class TruckingCategory
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
        public ICollection<Trucker> Truckers { get; set; } = new List<Trucker>();
    }
}
