using Challenge.Bot;
using Microsoft.AspNetCore.SignalR;

namespace Challenge.ChatHub
{
    public class ChatHub : Hub
    {
        private readonly IBot _service;

        public ChatHub(IBot bot)
        {
            _service = bot;
        }

        public async Task SendMessage(string room, string user, string message)
        {
            await Clients.Group(room).SendAsync("ReceiveMessage", user, message);
            _service.ReadCommand(message, room);
        }

        public async Task AddToGroup(string room)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, room);
        }

        public async Task BotResponse(string room, string body)
        {
            //await Clients.Group(room).SendAsync("BotResponse", "BOT", $"APPL.US quote is ${93.42} per share");
            await Clients.Group(room).SendAsync("BotResponse", "BOT", body);
        }
    }
}
