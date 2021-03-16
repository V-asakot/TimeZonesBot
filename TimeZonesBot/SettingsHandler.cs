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
        private string _token;
        public string Token { get => _token; set => _token = value; }

    }
    class SettingsHandler
    {
        private Settings _settings;
        public SettingsHandler(string filePath) {
            _settings = File.Exists("settings.json") ? JsonConvert.DeserializeObject<Settings>(File.ReadAllText("settings.json")) : null;
            if (_settings == null) Console.WriteLine("Settings was not loaded");
        }

        public string GetToken()
        {

            if (_settings == null) return "Token not loaded";
            return _settings.Token;
        }

        public bool IsLoaded() {
            return _settings != null;
        }
         
    }
}
