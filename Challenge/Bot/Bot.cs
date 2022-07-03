namespace Challenge.Bot
{
    public class Bot : IBot
    {
        private const string Command = "/stock=";
        public void ReadCommand(string message, string room)
        {
            if (IsValidCommand(message))
                CallAPI(message.Replace(Command, string.Empty), room);
        }

        private bool IsValidCommand(string command) 
            => command.ToLower().StartsWith(Command);

        private void CallAPI(string code, string room)
        {
            var botServiceURL = $"https://localhost:7116/stockQuote?code={code}&room={room}";
            var client  = new HttpClient();
            client.GetAsync(botServiceURL);
        }
    }
}
