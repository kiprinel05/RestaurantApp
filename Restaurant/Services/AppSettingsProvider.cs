using System.IO;
using System.Text.Json;
using Restaurant.Models;

namespace Restaurant.Services
{
    public class AppSettingsProvider
    {
        private readonly AppDiscountSettings _settings;
        public AppSettingsProvider()
        {
            var json = File.ReadAllText("appsettings.json");
            _settings = JsonSerializer.Deserialize<AppDiscountSettings>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        public AppDiscountSettings GetSettings() => _settings;
    }
} 