namespace SellingBook.Models.Statistic
{
    public class DailyStatistic
    {
        public string[] Dates { get; set; }
        public int[] OrderCounts { get; set; }
        public decimal[] Revenues { get; set; }
    }
}
