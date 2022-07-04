using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace BotStockQuote
{
    public static class Sender
    {
        public static void QueueMessage(StockQuoteResponse? stockQuoteResponse)
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.QueueDeclare(queue: "BotAnswer",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.BasicPublish(exchange: "",
                                            routingKey: "BotAnswer",
                                            basicProperties: null,
                                            body: GetBody(stockQuoteResponse));

                if (stockQuoteResponse is null)
                    Console.WriteLine("Empty stockQuoteResponse object");
                else
                    Console.WriteLine($" [x] Sent code: {stockQuoteResponse?.Symbol}, price {stockQuoteResponse?.Open}, for room {stockQuoteResponse?.Room}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception" + ex.Message);
            }
            
        }

        private static byte[] GetBody(StockQuoteResponse? stockQuoteResponse)
        {
            if (stockQuoteResponse is null)
                return new List<byte>().ToArray();

            var jsonBody = JsonSerializer.Serialize(stockQuoteResponse);
            return Encoding.UTF8.GetBytes(jsonBody);
        }
    }
}
