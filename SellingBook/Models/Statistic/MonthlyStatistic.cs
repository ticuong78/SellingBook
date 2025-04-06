namespace SellingBook.Models.Statistic
{
    public class MonthlyStatistic
    {
        public int Year { get; set; }
        public int[] Months { get; set; }
        public int[] OrderCounts { get; set; }
        public decimal[] Revenues { get; set; }
    }
}
