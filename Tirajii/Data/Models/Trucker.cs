using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tirajii.Data.Models
{
    public class Trucker
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 0)]
        public string Description { get; set; }

        [Required]
        public string Name { get; set; }

        [ForeignKey(nameof(User))]
        [Required]
        public string UserId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [StringLength(20, MinimumLength = 6)]
        [Phone]
        public string PhoneNumber { get; set; }

        public User User { get; set; }

        [Url]
        public string? ProfilePicture { get; set; }

        [ForeignKey(nameof(Truck))]
        public int? TruckId { get; set; }

        public Truck? Truck { get; set; }

        public int Experience { get; set; }

        public int Level { get; set; }

        public ICollection<CompanyRatings> CompaniesRated { get; set; } = new List<CompanyRatings>();

        [ForeignKey(nameof(Category))]
        [Required]
        public int CategoryId { get; set; }

        public TruckingCategory Category { get; set; }

        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    }
}
