using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeZonesBotInfrastructure;

namespace TimeZonesBot.Modules
{
    public class General : ModuleBase
    {
        private readonly Servers _servers;
        private readonly TimeZones _zones;
        public General(Servers servers,TimeZones zones) {
            _servers = servers;
            _zones = zones;
        }

        [Command("prefix")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task Prefix(string prefix=null)
        {
            if (prefix == null) {
                prefix = await _servers.GetGuildPrefix(Context.Guild.Id) ?? "!";
                await Context.Channel.SendMessageAsync($"Prefix of bot: {prefix}");
                return;
            }
            if (prefix.Length > 10)
            {
                await Context.Channel.SendMessageAsync("Prefix is too long");
                return;
            }
            await Context.Channel.SendMessageAsync($"Changed prefix of bot to: {prefix}");
            await _servers.ModifyGuildPrefix(Context.Guild.Id,prefix);
        }

        [Command("help")]
        public async Task Help(){
            string commands = @"help - gives you list of commands

prefix <new prefix> - let you set different commands prefix for this server(require administrator rights)

set <time zone> [user name] - sets time zone for you on this discord server (or you can do it for another user if have administrator rights)

time [user name] - returns current time and time zone of user on this server
";
            var builder = new EmbedBuilder()
           .WithDescription(commands)
           .WithTitle("List of commands")
           .WithColor(new Color(41, 128, 185))
           .WithCurrentTimestamp();

            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);
        }

    }
}
