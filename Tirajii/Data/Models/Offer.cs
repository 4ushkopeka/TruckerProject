using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tirajii.Data.Models
{
    public class Offer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public OfferCategory Category { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [ForeignKey(nameof(Company))]
        [Required]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        [ForeignKey(nameof(Trucker))]
        public int? TruckerId { get; set; }

        public Trucker Trucker { get; set; }

        [Required]
        [Range(0,10000)]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Payment { get; set; }

        [Required]
        public bool IsTaken { get; set; }

        [Required]
        public bool IsCompleted { get; set; }
    }
}
