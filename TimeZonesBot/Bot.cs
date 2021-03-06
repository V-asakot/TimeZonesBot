using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeZonesBotInfrastructure;


namespace TimeZonesBot
{
    public class Bot
    {
        
        public static async Task asyncRun(string[] args) {
           var bot = new Bot();
            await bot.asyncRun();
        }

        public async Task asyncRun()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            var provider = services.BuildServiceProvider();
            provider.GetRequiredService<CommandHandler>();

            await provider.GetRequiredService<StartupService>().startAsync();
            await Task.Delay(-1);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = Discord.LogSeverity.Verbose,
                MessageCacheSize = 1000
            })).AddSingleton(new CommandService(new CommandServiceConfig
            {
                LogLevel = Discord.LogSeverity.Verbose,
                DefaultRunMode = RunMode.Async,
                CaseSensitiveCommands = false

            })).AddSingleton<CommandHandler>().AddSingleton<StartupService>().AddDbContext<TimeZonesBotContext>().AddSingleton<Servers>().AddSingleton<TimeZones>();
           

        }
    }
}
