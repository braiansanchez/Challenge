namespace Challenge.Bot
{
    public class Bot : IBot
    {
        private const string command = "/stock=";
        public void ReadCommand(string message)
        {
            if (message.ToLower().StartsWith(command))
                CallAPI();
        }

        private async void CallAPI()
        {

        }

        private void QueueMessage(double price)
        {

        }
    }
}
