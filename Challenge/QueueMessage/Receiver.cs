
using Challenge.Models;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Challenge.QueueMessage
{
    public class Receiver
    {
        public ConnectionFactory Factory { get; set; }
        public IConnection Connection { get; set; }
        public IModel Channel { get; set; }
        private readonly IHubContext<ChatHub.ChatHub> _hubContext;

        public Receiver(IHubContext<ChatHub.ChatHub> hubContext)
        {
            Factory = new ConnectionFactory() { HostName = "localhost" };
            Connection = Factory.CreateConnection();
            Channel = Connection.CreateModel();
            _hubContext = hubContext;
        }

        public void Register()
        {
            Channel.QueueDeclare(queue: "BotAnswer", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += async (model, ea) =>
            {
                var stockQuote = GetstockQuote(ea);
                if (stockQuote is null)
                    return;

                await _hubContext.Clients.Group(stockQuote.Room).SendAsync("ReceiveMessage", "BOT", GetPostMessage(stockQuote));
            };
            Channel.BasicConsume(queue: "BotAnswer", autoAck: true, consumer: consumer);
        }

        private StockQuoteResponse? GetstockQuote(BasicDeliverEventArgs ea)
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                return JsonSerializer.Deserialize<StockQuoteResponse>(message);
            }
            catch
            {
                return null;
            }
        }

        private string GetPostMessage(StockQuoteResponse stockQuote)
        {
            if (!string.IsNullOrEmpty(stockQuote.ErrorMessage))
                return $"{stockQuote.ErrorMessage}";

            return $"{stockQuote?.Symbol} quote is ${stockQuote?.Open} per share";
        }

        public void Unregister() => Connection.Close();
    }
}