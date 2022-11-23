using System.ComponentModel.DataAnnotations;
using Tirajii.Data.Models;

namespace Tirajii.Models.Trucker
{
    public class TruckerRegisterViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [StringLength(500, MinimumLength =0)]
        public string Description { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        [StringLength(20, MinimumLength = 6)]
        public string PhoneNumber { get; set; }

        [Url]
        public string? Picture { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public ICollection<TruckingCategory> TruckingCategories { get; set; } = new List<TruckingCategory>();
    }
}
