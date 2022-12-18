using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Tirajii.Data.Models;

namespace Tirajii.Models.Trucker
{
    public class AllOffersViewModel
    {
        public const int OffersPerPage = 8;

        public string? Category { get; set; }

        [Display(Name = "Search")]
        public string? SearchTerm { get; set; }

        public OfferSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalOffers { get; set; }

        public IEnumerable<TruckingCategory> Categories { get; set; } = new List<TruckingCategory>();
        public IEnumerable<Offer> Offers { get; set; } = new List<Offer>();
    }
}
