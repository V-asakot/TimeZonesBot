using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeZonesBot.Modules
{
    public class General : ModuleBase
    {
        [Command("set")]
        public async Task Reg(int timeZone)
        {
            await Context.Channel.SendMessageAsync($"{Context.User.Username} time zone set to: {timeZone} UTC");
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

        [Command("time")]
        public async Task Time(string name)
        {
         
        }

    }
}
