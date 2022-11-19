using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tirajii.Data.Models;
using Tirajii.Models.Truck;

namespace Tirajii.Models.Company
{
    public class TruckOfferAddViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        [Range(0, 1000000000)]
        public decimal Cost { get; set; }

        [Required]
        public TruckForOfferViewModel Truck { get; set; }
    }
}
