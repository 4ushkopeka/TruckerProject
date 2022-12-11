using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tirajii.Data.Models
{
    public class TruckOffer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey(nameof(Company))]
        [Required]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        [Required]
        [Range(0, 1000000000)]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Cost { get; set; }

        [Required]
        [ForeignKey(nameof(Truck))]
        public int TruckId { get; set; }

        public Truck Truck { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        [Required]
        public bool IsBought { get; set; }
    }
}
