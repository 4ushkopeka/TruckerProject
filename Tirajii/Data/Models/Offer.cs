using Humanizer;
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
        public string Name { get; set; }

        [Required]
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public TruckingCategory Category { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [ForeignKey(nameof(Company))]
        [Required]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        [ForeignKey(nameof(Trucker))]
        public int? TruckerId { get; set; }

        public Trucker? Trucker { get; set; }

        [Required]
        [Range(0,20000)]
        [Column(TypeName = "decimal(15,2)")]
        public decimal Payment { get; set; }

        [Required]
        public bool IsTaken { get; set; }

        [Required]
        public int ExpAmount => new Random().Next(2,6);

        [Required]
        public bool IsCompleted { get; set; }

        public bool IsApproved { get; set; }

    }
}
