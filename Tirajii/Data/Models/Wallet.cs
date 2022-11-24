using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tirajii.Data.Models
{
    public class Wallet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        [Column(TypeName = "decimal(9,2)")]
        public decimal Balance { get; set; }
    }
}
