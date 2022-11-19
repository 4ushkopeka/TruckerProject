using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tirajii.Data.Models;

namespace Tirajii.Models.Truck
{
    public class TruckForOfferViewModel
    {
        [Required]
        [RegularExpression(@"[A-Z]{2} \d{4} [A-Z]{2}")]
        public string RegistrationNumber { get; set; }

        [Required]
        public string Colour { get; set; }

        [StringLength(10, MinimumLength = 4)]
        public string? Name { get; set; }

        [Required]
        public int ClassId { get; set; }

        [Required]
        public int? CompanyId { get; set; }
    }
}
