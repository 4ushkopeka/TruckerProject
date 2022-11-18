using System.ComponentModel.DataAnnotations;

namespace Tirajii.Data.Models
{
    public class CompanyCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Company> Companies { get; set; } = new List<Company>();
    }
}
