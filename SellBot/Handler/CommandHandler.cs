using Discord;
using Discord.WebSocket;
using SellBot.Wrappers;

namespace SellBot.Handler
{
    internal class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly SellAPI _api;

        public CommandHandler(DiscordSocketClient client, SellAPI api)
        {
            _client = client;
            _api = api;

            Task.Run(Initialize);
        }

        private async Task Initialize()
        {
            // Setup
            await _client.CreateGlobalApplicationCommandAsync(GenerateSlashCommand("setuppanel", "Setup the Panel", true).Build());

            var checkCmd = GenerateSlashCommand("checkinvoice", "Get information about a invoice status", true);
            checkCmd.AddOption("invoiceid", ApplicationCommandOptionType.String, "Invoice id to check", true);
            await _client.CreateGlobalApplicationCommandAsync(checkCmd.Build());
        }

        private static SlashCommandBuilder GenerateSlashCommand(string name, string description, bool admin = false)
        {
            SlashCommandBuilder builder = new();
            builder.WithName(name);
            builder.WithDescription(description);
            builder.WithContextTypes(InteractionContextType.Guild);
            builder.WithNsfw(false);
            if (admin) builder.WithDefaultMemberPermissions(GuildPermission.Administrator);

            return builder;
        }

        public async Task OnReceiveComamnd(SocketSlashCommand command)
        {
            switch (command.CommandName)
            {
                case "setuppanel":
                    await SetupPanel(command);
                    break;

                case "checkinvoice":
                    await CheckInvoice(command);
                    break;
            }
        }

        private async Task SetupPanel(SocketSlashCommand command)
        {
            var builder = new ComponentBuilder().WithButton("Bitcoin Payment", "buyfirst").WithButton("Litecoin Payment", "buysecond");

            await command.Channel.SendMessageAsync(null, false, MessageHelper.CustomEmbed("Tickets", $"To create a new payment click one of the buttons below", Color.DarkGreen), null, null, null, builder.Build());

            await command.RespondAsync(null, null, false, true, null, null, MessageHelper.SuccessEmbed("Created Panel"));
        }

        private async Task CheckInvoice(SocketSlashCommand command)
        {
            var id = (string)command.Data.Options.ToArray()[0].Value;

            SellAPI.Invoice invoice = await _api.GetInvoice(id);
            if (invoice == null)
            {
                await command.RespondAsync(null, null, false, true, null, null, MessageHelper.FailedEmbed("Invoice not found"));
            }
            else
            {
                await command.RespondAsync(null, null, false, true, null, null, MessageHelper.SuccessEmbed($"Invocie status: {invoice.status.ToString()}"));
            }
        }
    }
}
