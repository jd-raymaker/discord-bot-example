using Discord.Commands;

public class Echo : ModuleBase<SocketCommandContext>
{
    [Command("say")]
    [Summary("Echoes a message")]
    [Alias("echo")]
    public Task SayAsync(string echo) => ReplyAsync(echo);
}
