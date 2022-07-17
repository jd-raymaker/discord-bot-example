using System.Reflection;
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

public class CommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;
    private readonly IServiceProvider _services;
    private readonly Discord.Rest.BaseDiscordClient _baseClient;

    public CommandHandler(DiscordSocketClient client, CommandService commands)
    {
        this._client = client;
        this._commands = commands;
    }

    public async Task InstallCommandsAsync()
    {
        // Hook the MessageReceived event into our command handler
        _client.MessageReceived += HandleCommandAsync;

        // Discover all of the command modules in the entry assembly and load them.
        await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: _services);
    }

    private async Task HandleCommandAsync(SocketMessage messageParam) {

        // Don't process the command if it was a system message
        var message = messageParam as SocketUserMessage;
        if (message == null) return;

        // Create a number to track where the prefix ends and the command begins
        int argPos = 0;

        // Determine if the message is a command based on the prefix and make sure no bots trigger commands
        if (!(message.HasCharPrefix('!', ref argPos) || 
            message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
            message.Author.IsBot)
            return;

        // Create a WebSocket-based command context based on the message
        var context = new SocketCommandContext(_client, message);

        // Execute the command with the command context we just
        // created, along with the service provider for precondition checks.
        await _commands.ExecuteAsync(
            context: context, 
            argPos: argPos,
            services: null);
    }
}
