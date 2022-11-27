using System.ComponentModel.DataAnnotations;

namespace Tirajii.Models.Truck
{
    public class TruckUpgradeViewModel
    {
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
    }
}
