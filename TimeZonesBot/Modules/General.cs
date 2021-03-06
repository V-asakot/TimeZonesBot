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

        [Command("info")]
        public async Task Info()
        {
            var builder = new EmbedBuilder()
            .WithThumbnailUrl(Context.User.GetAvatarUrl()??Context.User.GetDefaultAvatarUrl())
            .WithDescription("User Time")
            .WithColor(new Color(41,128,185))
            .WithCurrentTimestamp();

            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null,false,embed);
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

    }
}
