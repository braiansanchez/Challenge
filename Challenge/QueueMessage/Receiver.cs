
using Challenge.Models;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Challenge.QueueMessage
{
    public class Receiver : Hub
    {
        public ConnectionFactory factory { get; set; }
        public IConnection connection { get; set; }
        public IModel channel { get; set; }
        private readonly IHubContext<ChatHub.ChatHub> _hubContext;

        public Receiver(IHubContext<ChatHub.ChatHub> hubContext)
        {
            factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            _hubContext = hubContext;
        }

        public void Register()
        {
            channel.QueueDeclare(queue: "BotAnswer", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var stockQuote = GetstockQuote(message);
                await _hubContext.Clients.Group(stockQuote?.Room).SendAsync("ReceiveMessage", "BOT", $"{stockQuote?.Symbol} quote is ${stockQuote?.Open} per share");
                Console.WriteLine(" [x] Received {0}", message);
            };
            channel.BasicConsume(queue: "BotAnswer", autoAck: true, consumer: consumer);
        }

        private StockQuoteResponse? GetstockQuote(string message)
        {
            try
            {
                return JsonSerializer.Deserialize<StockQuoteResponse>(message);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void Unregister()
        {
            this.connection.Close();
        }
    }
}