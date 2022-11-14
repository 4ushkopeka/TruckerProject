using System.ComponentModel.DataAnnotations.Schema;

namespace Tirajii.Data.Models
{
    public class Trucker
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public User User { get; set; }

        public int Experience { get; set; }

        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    }
}
