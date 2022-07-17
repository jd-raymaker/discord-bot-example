using Discord;
using Discord.Commands;
using Discord.WebSocket;

class Program {

    static Task Main() => new Program().MainAsync();

    private readonly DiscordSocketClient _client;
    private readonly CommandHandler _command;

    private Program()
    {
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Info
        });

        var cmdConfig = new CommandServiceConfig
        {
            LogLevel = LogSeverity.Info,
            CaseSensitiveCommands = false,
            IgnoreExtraArgs = true
        };

        _command = new CommandHandler(_client, new CommandService(cmdConfig));

        _client.Log += Log;
    }

    private async Task MainAsync() {

        var token = File.ReadAllText("token.txt");

        await _command.InstallCommandsAsync();
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
	}

    private Task Log(LogMessage message) {

        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}
