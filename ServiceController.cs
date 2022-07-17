using Microsoft.Extensions.DependencyInjection;
using Discord.WebSocket;
using Discord.Commands;

public class Initialize
{
    private readonly CommandService _commands;
    private readonly DiscordSocketClient _client;

    public Initialize(CommandService commands = null, DiscordSocketClient client = null)
    {
        this._commands = commands ?? new CommandService();
        this._client = client ?? new DiscordSocketClient();
    }
    
    public IServiceProvider BuildServiceProvider() => new ServiceCollection()
        .AddSingleton(_client)
        .AddSingleton(_commands)
        .AddSingleton<CommandHandler>()
        .BuildServiceProvider();
}
