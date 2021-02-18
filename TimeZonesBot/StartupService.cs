using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TimeZonesBot.Modules;

namespace TimeZonesBot
{
   public class StartupService
    {
        public static IServiceProvider provider;
        private readonly DiscordSocketClient discord;
        private readonly CommandService commands;
        private string token = "xxx";
        public StartupService(IServiceProvider _provider, DiscordSocketClient discord, CommandService commands){
            this.commands = commands;
            provider = _provider;
            this.discord = discord;
        }
        public async Task startAsync() {
            if (string.IsNullOrEmpty(token)) {
                Console.WriteLine("Incorect token");
                return;
            }
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), provider);
            await discord.LoginAsync(Discord.TokenType.Bot,token);
            await discord.StartAsync();

            
        }
    }
}
