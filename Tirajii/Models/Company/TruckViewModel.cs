using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tirajii.Data.Models;

namespace Tirajii.Models.Company
{
    public class TruckViewModel
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

        public ICollection<TruckClass> Classes { get; set; } = new List<TruckClass>();

        public int? CompanyId { get; set; }
        
        [Required]
        public bool HasSpeakers { get; set; }

        [Required]
        public bool HasBluetooth { get; set; }
        
        [Required]
        public bool HasCDPlayer { get; set; }
        
        [Required]
        public bool HasParkTronic { get; set; }
        
        [Required]
        public bool HasInstaBrakes { get; set; }

        [Required]
        [Url]
        public string Picture { get; set; }
    }
}
