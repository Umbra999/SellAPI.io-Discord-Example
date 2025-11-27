namespace SellBot
{
    internal class Entry
    {
        private static DiscordBot _bot;
        private static SellAPI _api;

        public static void Main()
        {
            _api = new SellAPI();
            _bot = new DiscordBot(_api);

            Thread.Sleep(-1);
        }
    }
}
