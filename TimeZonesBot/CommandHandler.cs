using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeZonesBotInfrastructure;

namespace TimeZonesBot
{
    public class CommandHandler
    {
        public static IServiceProvider provider;
        public static DiscordSocketClient discord;
        public static CommandService commands;
        public static Servers servers;

        public CommandHandler(IServiceProvider _provider, DiscordSocketClient _discord, CommandService _commands,Servers _servers) {
            commands = _commands;
            provider = _provider;
            discord = _discord;
            servers = _servers;
            foreach (CommandInfo a in commands.Commands)
            {
                Console.WriteLine(a);
            }

            discord.Ready += OnReady;
            discord.MessageReceived += OnMessageReceived;
            commands.CommandExecuted += OnCommandExecuted;
        }

        private async Task OnCommandExecuted(Optional<CommandInfo>  command, ICommandContext context, IResult result)
        {
            if (command.IsSpecified && !result.IsSuccess) await context.Channel.SendMessageAsync($"Error: {result}");
        }

        private async Task OnMessageReceived(SocketMessage arg)
        {
            var msg = arg as SocketUserMessage;
            if (msg.Author.IsBot) return;
            var context = new SocketCommandContext(discord,msg);
            int pos = 0;
            string prefix = await servers.GetGuildPrefix((msg.Channel as SocketGuildChannel).Guild.Id) ?? "!";
            if (msg.HasStringPrefix(prefix, ref pos) || msg.HasMentionPrefix(discord.CurrentUser, ref pos)) {
                var result = await commands.ExecuteAsync(context,pos,provider);
                /*
                if (!result.IsSuccess)
                {
                    var reason = result.Error;
                    await context.Channel.SendMessageAsync($"Error occured: {reason} \n");
                    Console.WriteLine(reason);
                }*/
            }
        }

        private Task OnReady()
        {
            Console.WriteLine($"Connected as {discord.CurrentUser.Username}#{discord.CurrentUser.Discriminator}");
            return Task.CompletedTask;
        }
    }
}
