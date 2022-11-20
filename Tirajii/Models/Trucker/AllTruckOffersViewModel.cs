using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Tirajii.Data.Models;

namespace Tirajii.Models.Trucker
{
    public class AllTruckOffersViewModel
    {
        public const int OffersPerPage = 4;

        public string? Category { get; set; }

        [Display(Name = "Search")]
        public string? SearchTerm { get; set; }

        public TruckOfferSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalOffers { get; set; }

        public IEnumerable<TruckClass> Categories { get; set; } = new List<TruckClass>();
        public IEnumerable<TruckOffer> Offers { get; set; } = new List<TruckOffer>();
    }
}
