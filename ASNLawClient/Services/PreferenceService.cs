using System;
using System.Threading.Tasks;

namespace ASNLawClient.Client.Services
{
    public interface IPreferenceService
    {
        string Get(string key, string defaultValue);
        void Set(string key, string value);
        Task<string> GetAsync(string key, string defaultValue);
        Task SetAsync(string key, string value);
    }

    public class PreferenceService : IPreferenceService
    {
        private readonly Dictionary<string, string> _preferences = new();

        public string Get(string key, string defaultValue)
        {
            try
            {
                return _preferences.TryGetValue(key, out var value) ? value : defaultValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting preference {key}: {ex.Message}");
                return defaultValue;
            }
        }

        public void Set(string key, string value)
        {
            try
            {
                _preferences[key] = value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting preference {key}: {ex.Message}");
            }
        }

        public Task<string> GetAsync(string key, string defaultValue)
        {
            return Task.FromResult(Get(key, defaultValue));
        }

        public Task SetAsync(string key, string value)
        {
            Set(key, value);
            return Task.CompletedTask;
        }
    }
}