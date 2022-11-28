using System.ComponentModel.DataAnnotations;

namespace Tirajii.Models.Truck
{
    public class TruckUpgradeViewModel
    {
        public Dictionary<string, bool> Upgrades { get; set; } = new Dictionary<string, bool>();

        public Dictionary<string, bool> Upgraded { get; set; } = new Dictionary<string, bool>();

        public decimal Cost { get; set; }

    }
}
