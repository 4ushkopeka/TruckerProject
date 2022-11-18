using System.ComponentModel.DataAnnotations;

namespace Tirajii.Data.Models
{
    public class OfferCategory
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    }
}
