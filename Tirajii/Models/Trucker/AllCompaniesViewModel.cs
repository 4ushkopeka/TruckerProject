using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Tirajii.Data.Models;

namespace Tirajii.Models.Trucker
{
    public class AllCompaniesViewModel
    {
        public const int OffersPerPage = 4;

        public string? Category { get; set; }

        [Display(Name = "Search")]
        public string? SearchTerm { get; set; }

        public CompanySorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalCompanies { get; set; }

        public IEnumerable<CompanyCategory> Categories { get; set; } = new List<CompanyCategory>();
        public IEnumerable<Data.Models.Company> Companies { get; set; } = new List<Data.Models.Company>();
    }
}
