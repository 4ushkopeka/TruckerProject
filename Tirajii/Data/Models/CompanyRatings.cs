using System.ComponentModel.DataAnnotations;

namespace Tirajii.Data.Models
{
    public class CompanyRatings
    {
        [Required]
        public int RaterId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int Rating { get; set; }

        public Trucker Rater { get; set; }

        public Company Company { get; set; }
    }
}
