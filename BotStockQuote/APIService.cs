using BotStockQuote;
using CsvHelper;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/stockQuote", async (string code, string room)
    =>
{
    StockQuoteResponse record = new StockQuoteResponse();
    try
    {
        var url = $"https://stooq.com/q/l/?s={code}&f=sd2t2ohlcv&h&e=csv";
        var client = new HttpClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        TextReader reader = new StreamReader(await response.Content.ReadAsStreamAsync());
        var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
        record = csvReader.GetRecords<StockQuoteResponse>().FirstOrDefault();
    }
    catch (Exception)
    {
        record.ErrorMessage = $"We have a problem trying to get information for {code}";
    }
    record.Room = room;
    Sender.QueueMessage(record);
});

app.Run();
