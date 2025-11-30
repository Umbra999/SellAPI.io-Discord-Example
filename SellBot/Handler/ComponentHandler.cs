using System.Globalization;
using System.Text.Json;
using Discord.WebSocket;
using SellBot.Wrappers;
using static SellBot.SellAPI;

namespace SellBot.Handler
{
    internal class ComponentHandler
    {
        private readonly SellAPI _api;

        public ComponentHandler(SellAPI api)
        {
            _api = api;
        }

        public async Task OnReceiveMessage(SocketMessageComponent message)
        {
            switch (message.Data.CustomId)
            {
                case "buyfirst":
                {
                    Invoice invoice = await _api.CreateCryptoInvoice(PaymentMethod.bitcoin, Settings.DiscordNotificationCallback, 30,0, Settings.btcPayoutAddress);
                    if (invoice == null || invoice.details == null)
                    {
                        await message.RespondAsync(null, null, false, true, null, null, MessageHelper.FailedEmbed("Failed to create invoice"));
                    }
                    else
                    {
                        var element = (JsonElement)invoice.details;
                        var cryptoDetails = element.Deserialize<CryptoInvoiceDetails>();

                            string btcPrice = Utility.DecimalToString(cryptoDetails.price_in_currency);
                        DateTime expiration = Utility.UnixTimeStampToDateTime(invoice.expiration);

                        string response = $"Please sent **{btcPrice}** {invoice.method.ToString()} to **{cryptoDetails.payment_address}**. This invoice will expire at {expiration.ToString(CultureInfo.InvariantCulture)}";
                        await message.RespondAsync(null, null, false, true, null, null, MessageHelper.SuccessEmbed(response));
                    }

                }
                break;

                case "buysecond":
                {
                    Invoice invoice = await _api.CreateCryptoInvoice(PaymentMethod.litecoin, Settings.DiscordNotificationCallback, 50, 5, Settings.ltcPayoutAddress, $"This is a invoice for {message.User.Id}");
                    if (invoice == null || invoice.details == null)
                    {
                        await message.RespondAsync(null, null, false, true, null, null, MessageHelper.FailedEmbed("Failed to create invoice"));
                    }
                    else
                    {
                        var element = (JsonElement)invoice.details;
                        var cryptoDetails = element.Deserialize<CryptoInvoiceDetails>();

                        string btcPrice = Utility.DecimalToString(cryptoDetails.price_in_currency);
                        DateTime expiration = Utility.UnixTimeStampToDateTime(invoice.expiration);

                        string response = $"Please sent **{btcPrice}** {invoice.method.ToString()} to **{cryptoDetails.payment_address}**. This invoice will expire at {expiration.ToString(CultureInfo.InvariantCulture)}";
                        await message.RespondAsync(null, null, false, true, null, null, MessageHelper.SuccessEmbed(response));
                    }
                }
                break;
            }
        }
    }
}
