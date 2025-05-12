using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ASNLawClient.Client.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace ASNLawClient.Client.Services
{
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly IPreferenceService _preferenceService;
        private readonly NavigationManager _navigationManager;
        private readonly ApiSettings _apiSettings;

        public AuthenticationService(
            HttpClient httpClient,
            IPreferenceService preferenceService,
            NavigationManager navigationManager,
            IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _preferenceService = preferenceService;
            _navigationManager = navigationManager;
            _apiSettings = apiSettings.Value;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            try
            {
                var loginRequest = new
                {
                    Email = email,
                    Password = password
                };

                var response = await _httpClient.PostAsJsonAsync("auth/login", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    var loginResult = await response.Content.ReadFromJsonAsync<LoginResult>();

                    if (loginResult != null && !string.IsNullOrEmpty(loginResult.Token))
                    {
                        // Save token and user info
                        _preferenceService.Set("AuthToken", loginResult.Token);
                        _preferenceService.Set("UserName", loginResult.UserName);
                        _preferenceService.Set("UserRole", loginResult.Role);

                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return false;
            }
        }

        public void Logout()
        {
            // Clear authentication data
            _preferenceService.Set("AuthToken", string.Empty);
            _preferenceService.Set("UserName", string.Empty);
            _preferenceService.Set("UserRole", string.Empty);

            // Redirect to login page
            _navigationManager.NavigateTo("/login");
        }

        public bool IsAuthenticated()
        {
            var token = _preferenceService.Get("AuthToken", string.Empty);
            return !string.IsNullOrEmpty(token);
        }

        public string GetUserName()
        {
            return _preferenceService.Get("UserName", string.Empty);
        }

        public string GetUserRole()
        {
            return _preferenceService.Get("UserRole", string.Empty);
        }

        private class LoginResult
        {
            public string Token { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
        }
    }
}