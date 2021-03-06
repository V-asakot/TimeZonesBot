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

        [Command("set")]
        public async Task Reg(string timeZone)
        {
                int time;
                if (!int.TryParse(timeZone, out time)) { await Context.Channel.SendMessageAsync("Wrong UTC format"); return; }
                if (time < -12 || time > 14) {await Context.Channel.SendMessageAsync("UTC ranging from UTC−12:00 to UTC+14:00"); return; }
                await _zones.ModifyUserTimeZone(Context.Guild.Id, Context.User.Id,time);
                await Context.Channel.SendMessageAsync($"{Context.User.Username}#{Context.User.Discriminator} time zone set to: {time} UTC"); 

        }

        [Command("set")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        public async Task Reg(string timeZone, string name)
        {
            int time;
            if (!int.TryParse(timeZone, out time)) { await Context.Channel.SendMessageAsync("Wrong UTC format"); return; }
            if (time < -12 || time > 14) { await Context.Channel.SendMessageAsync("UTC ranging from UTC−12:00 to UTC+14:00"); return; }
            IUser user;
            int discriminator = -1, index = -1;
            string[] args = name.Split('#');
            if (args.Length == 2 && int.TryParse(args[1], out discriminator)) name = args[0];
            IReadOnlyCollection<IUser> users = await Context.Channel.GetUsersAsync().FirstOrDefaultAsync(x => { for (int i = 0; i < x.Count; i++) if (x.ElementAt(i).Username == name && (discriminator == -1 || discriminator == x.ElementAt(i).DiscriminatorValue)) { index = i; return true; } return false; });
            if (users == null || index == -1) { await Context.Channel.SendMessageAsync($"No such user at chanel"); return; }
            user = users.ElementAt(index);
            
            await _zones.ModifyUserTimeZone(Context.Guild.Id, user.Id, time);
            await Context.Channel.SendMessageAsync($"{user.Username}#{user.Discriminator} time zone set to: {time} UTC");
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

        [Command("time")]
        public async Task Time(string name = null) {
            IUser user;
            if (name == null) { 
                user = Context.User; 
            }
            else
            {
                int discriminator = -1, index = -1;
                string[] args = name.Split('#');
                if (args.Length == 2 && int.TryParse(args[1], out discriminator)) name = args[0];
                IReadOnlyCollection<IUser> users = await Context.Channel.GetUsersAsync().FirstOrDefaultAsync(x => { for (int i = 0; i < x.Count; i++) if (x.ElementAt(i).Username == name && (discriminator == -1 || discriminator == x.ElementAt(i).DiscriminatorValue)) { index = i; return true; } return false; });
                if (users == null || index == -1) { await Context.Channel.SendMessageAsync($"No such user at chanel"); return; }
                user = users.ElementAt(index);
            }
            int time = await _zones.GetUserTimeZone(Context.Guild.Id,user.Id);
            if(time == int.MinValue) { await Context.Channel.SendMessageAsync($"User timezone not set: {user.Username}"); return; }

            var builder = new EmbedBuilder()
            .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
            .WithDescription($"{user.Username}#{user.Discriminator} time is {DateTime.UtcNow.AddHours(time).ToString("HH:mm")} \n with {time} UTC time zone")
            .WithColor(new Color(41, 128, 185))
            .WithCurrentTimestamp();

            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);
           
        }

    }
}
