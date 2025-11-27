using Discord;
using Discord.WebSocket;
using SellBot.Handler;
using SellBot.Wrappers;

namespace SellBot
{
    internal class DiscordBot
    {
        private readonly DiscordSocketClient _client;
        private readonly SellAPI sellApi;

        private CommandHandler _commandHandler = null;
        private ComponentHandler _componentHandler = null;

        public DiscordBot(SellAPI api)
        {
            sellApi = api;

            DiscordSocketConfig config = new()
            {
                AlwaysDownloadUsers = true,
                LogLevel = LogSeverity.Warning,
                GatewayIntents = GatewayIntents.All,
            };

            _client = new DiscordSocketClient(config);
            _client.Log += Log;
            _client.Ready += Client_Ready;
            _client.SlashCommandExecuted += Client_SlashCommandExecuted;
            _client.ButtonExecuted += Client_ButtonExecuted;

            Task.Run(LoginAsync);
        }

        private async Task LoginAsync()
        {
            await _client.LoginAsync(TokenType.Bot, Settings.DiscordToken);
            await _client.StartAsync();
        }

        private async Task Client_Ready()
        {
            Logger.LogSuccess("Bot Ready");

            await _client.SetStatusAsync(UserStatus.DoNotDisturb);
            await _client.SetCustomStatusAsync("powered by sellapi.io");
            await _client.SetGameAsync("powered by sellapi.io", null, ActivityType.Watching);

            _commandHandler = new CommandHandler(_client);
            _componentHandler = new ComponentHandler(sellApi);
        }

        private async Task Client_SlashCommandExecuted(SocketSlashCommand command)
        {
            await _commandHandler.OnReceiveComamnd(command);
        }

        private async Task Client_ButtonExecuted(SocketMessageComponent component)
        {
            await _componentHandler.OnReceiveMessage(component);
        }

        private static Task Log(LogMessage msg)
        {
            Logger.LogDebug($"[DISCORD] {msg.ToString()}");

            return Task.CompletedTask;
        }
    }
}
