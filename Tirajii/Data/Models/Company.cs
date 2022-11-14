using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace Tirajii.Data.Models
{
    public class Company
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Creator { get; set; }

        public decimal Rating { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [ForeignKey(nameof(Owner))]
        public string OwnerId { get; set; }

        public User Owner { get; set; }

        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    }
}
