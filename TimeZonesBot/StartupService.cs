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
        private readonly SettingsHandler settings;
        private string token;
        public StartupService(IServiceProvider _provider, DiscordSocketClient discord, CommandService commands){
            this.commands = commands;
            provider = _provider;
            this.discord = discord;
            settings = new SettingsHandler("settings.json");
        }
        public async Task startAsync() {

            if (settings.IsLoaded()) { token = settings.GetToken(); } else { Console.WriteLine("Settings file not loaded"); return; }
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
