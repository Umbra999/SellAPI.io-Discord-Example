using Discord;
using Discord.WebSocket;
using SellBot.Wrappers;

namespace SellBot.Handler
{
    internal class CommandHandler
    {
        private readonly DiscordSocketClient _client;

        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;

            Task.Run(Initialize);
        }

        private async Task Initialize()
        {
            // Setup
            await _client.CreateGlobalApplicationCommandAsync(GenerateSlashCommand("setuppanel", "Setup the Panel", true).Build());
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
            }
        }

        private async Task SetupPanel(SocketSlashCommand command)
        {
            var builder = new ComponentBuilder().WithButton("Bitcoin Payment", "buyfirst").WithButton("Litecoin Payment", "buysecond");

            await command.Channel.SendMessageAsync(null, false, MessageHelper.CustomEmbed("Tickets", $"To create a new payment click one of the buttons below", Color.DarkGreen), null, null, null, builder.Build());

            await command.RespondAsync(null, null, false, true, null, null, MessageHelper.SuccessEmbed("Created Panel"));
        }
    }
}
