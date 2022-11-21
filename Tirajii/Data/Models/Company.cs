using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace Tirajii.Data.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, 5)]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Rating { get; set; }

        [ForeignKey(nameof(Category))]
        [Required]
        public int CategoryId { get; set; }

        public CompanyCategory Category { get; set; }

        public ICollection<CompanyRatings> CompanyRatings { get; set; } = new List<CompanyRatings>();

        [Required]
        [ForeignKey(nameof(Owner))]
        public string OwnerId { get; set; }

        public User Owner { get; set; }

        [Required]
        [Url]
        public string Picture { get; set; }

        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    }
}
