namespace SellBot
{
    internal class Settings
    {
        public static readonly string DiscordToken = ""; // Discord Developer bot token
        public static readonly string DiscordNotificationCallback = ""; // Either custom https url to process it further or discord webhook for notifications only, this will trigger once the invoice status updates
        public static readonly string btcPayoutAddress = "";
        public static readonly string ltcPayoutAddress = "";
    }
}
