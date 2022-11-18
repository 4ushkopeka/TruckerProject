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
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(500, MinimumLength =0)]
        public string Description { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 6)]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Url]
        [Display(Name = "Profile Picture")]
        public string? ProfilePicture { get; set; }

        [Required]
        public TrailerType TruckingCategory { get; set; }
    }
}
