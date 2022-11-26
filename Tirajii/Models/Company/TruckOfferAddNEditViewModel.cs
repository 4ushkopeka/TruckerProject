using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tirajii.Data.Models;

namespace Tirajii.Models.Company
{
    public class TruckOfferAddNEditViewModel
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }

        public int? CompanyId { get; set; }

        [Required]
        [Range(0, 1000000000)]
        public decimal Cost { get; set; }

        [Required]
        public int TruckId { get; set; }

        public int? truckOfferId { get; set; }
    }
}
