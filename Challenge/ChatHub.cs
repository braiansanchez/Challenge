using Challenge.Bot;
using Microsoft.AspNetCore.SignalR;

namespace Challenge
{
    public class ChatHub : Hub
    {
        private readonly IBot _bot;

        public ChatHub(IBot bot)
        {
            _bot = bot;
        }

        public async Task SendMessage(string room, string user, string message)
        {
            await Clients.Group(room).SendAsync("ReceiveMessage", user, message);
            _bot.ReadCommand(message);
        }

        public async Task AddToGroup(string room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
        }

        public async Task BotResponse(string room)
        {
            //aqui deberia escuchar la queue de RabbitMQ
            await Clients.Group(room).SendAsync("BotResponse", "BOT", $"APPL.US quote is ${93.42} per share");
        }
    }
}
