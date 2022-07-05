using CsvHelper.Configuration.Attributes;

namespace BotStockQuote
{
    public class StockQuoteResponse
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public int Volume { get; set; }
        [Ignore]
        public string Room { get; set; }
        [Ignore]
        public string ErrorMessage { get; set; }
    }
}
