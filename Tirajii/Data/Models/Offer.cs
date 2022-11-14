using System.ComponentModel.DataAnnotations.Schema;

namespace Tirajii.Data.Models
{
    public class Offer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime DueDate { get; set; }

        [ForeignKey(nameof(Company))]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        [ForeignKey(nameof(Trucker))]
        public int? TruckerId { get; set; }

        public Trucker Trucker { get; set; }

        public decimal Payment { get; set; }

        public bool IsTaken { get; set; }
    }
}
