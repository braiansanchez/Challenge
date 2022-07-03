using BotStockQuote;
using CsvHelper;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/stockQuote", async (string code, string room)
    =>
{
    var url = $"https://stooq.com/q/l/?s={code}&f=sd2t2ohlcv&h&e=csv";
    var client = new HttpClient();
    var response = await client.GetAsync(url);
    response.EnsureSuccessStatusCode();
    TextReader reader = new StreamReader(await response.Content.ReadAsStreamAsync());
    var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
    var record = csvReader.GetRecords<StockQuoteResponse>().FirstOrDefault();
    Sender.QueueMessage(record, room);
});

app.Run();