using System.ComponentModel.DataAnnotations;

namespace Tirajii.Data.Models
{
    public class TrailerType
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Trailer> Trailers { get; set; }
    }
}
