namespace Tirajii.Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Company> Companies { get; set; } = new List<Company>();
    }
}
