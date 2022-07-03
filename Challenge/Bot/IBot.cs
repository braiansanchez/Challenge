namespace Challenge.Bot
{
    public interface IBot
    {
        void ReadCommand(string message, string room);
    }
}
