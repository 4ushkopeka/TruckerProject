using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tirajii.Data.Models
{
    public class Truck
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Colour { get; set; }

        [Required]
        [RegularExpression(@"[A-Z]{2} \d{4} [A-Z]{2}")]
        public string RegistrationNumber { get; set; }

        [StringLength(10, MinimumLength = 4)]
        public string? Name { get; set; }

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
        public bool IsForSale { get; set; }

        [ForeignKey(nameof(Owner))]
        public string? OwnerId { get; set; }

        public User? Owner { get; set; }

        [Required]
        [ForeignKey(nameof(Class))]
        public int ClassId { get; set; }

        public TruckClass Class { get; set; }

        public TruckOffer? TruckOffer { get; set; }

        [Required]
        [ForeignKey(nameof(Company))]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        [Required]
        [Url]
        public string Picture { get; set; }
    }
}
