using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TimeZonesBot
{
    class Settings {
        private string token;
        public string Token { get => token; set => token = value; }

    }
    class SettingsHandler
    {
        private Settings settings;
        public SettingsHandler(string filePath) {
            settings = File.Exists("settings.json") ? JsonConvert.DeserializeObject<Settings>(File.ReadAllText("settings.json")) : null;
            if (settings == null) Console.WriteLine("Settings was not loaded");
        }

        public string GetToken()
        {

            if (settings == null) return "Token not loaded";
            return settings.Token;
        }

        public bool IsLoaded() {
            return settings != null;
        }
         
    }
}
