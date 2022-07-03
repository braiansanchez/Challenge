namespace Challenge.Bot
{
    public class Bot : IBot
    {
        private const string Command = "/stock=";
        public void ReadCommand(string message)
        {
            if (IsValidCommand(message))
                CallAPI(message.Replace(Command, string.Empty));
        }

        private bool IsValidCommand(string command) 
            => command.ToLower().StartsWith(Command);

        private void CallAPI(string code)
        {
            var botServiceURL = $"https://localhost:7116/stockQuote?code={code}";
            var client  = new HttpClient();
            client.GetAsync(botServiceURL);
        }
    }
}
