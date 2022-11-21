namespace Tirajii.Models.Company
{
    public class RatingViewModel
    {
        public decimal AverageRating { get; set; }

        public int Count1 { get; set; }

        public int Count2 { get; set; }

        public int Count3 { get; set; }

        public int Count4 { get; set; }

        public int Count5 { get; set; }

        public int Total => Count1 + Count2 + Count3 + Count4 + Count5;
    }
}
