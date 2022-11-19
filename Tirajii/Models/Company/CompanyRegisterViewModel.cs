using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tirajii.Data.Models;

namespace Tirajii.Models.Company
{
    public class CompanyRegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string? OwnerId { get; set; }

        [Required]
        [Url]
        public string Picture { get; set; }

        public ICollection<CompanyCategory> Categories { get; set; } = new List<CompanyCategory>();
    }
}
