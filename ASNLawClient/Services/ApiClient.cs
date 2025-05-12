using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ASNLawClient.Client.Models;
using ASNLawClient.Client.Options;
using ASNLawClient.Client.Services;
using Microsoft.Extensions.Options;

namespace ASNLawClient.Client.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IPreferenceService _preferenceService;
        private readonly ApiSettings _apiSettings;
        private readonly ToastService _toastService;
        private bool _hasShownConnectionError = false;

        public ApiClient(
            HttpClient httpClient,
            IPreferenceService preferenceService,
            IOptions<ApiSettings> apiSettings,
            ToastService toastService)
        {
            _httpClient = httpClient;
            _preferenceService = preferenceService;
            _apiSettings = apiSettings.Value;
            _toastService = toastService;
        }

        // Document Methods
        public async Task<List<Models.Document>> GetDocumentsAsync()
        {
            try
            {
                return await GetAsync<List<Models.Document>>("documents");
            }
            catch (Exception ex)
            {
                HandleApiException("Error getting documents", ex);
                return new List<Models.Document>();
            }
        }

        public async Task<Models.Document?> GetDocumentAsync(string id)
        {
            try
            {
                return await GetAsync<Models.Document>($"documents/{id}");
            }
            catch (Exception ex)
            {
                HandleApiException("Error getting document", ex);
                return null;
            }
        }

        public async Task<List<Models.Document>> GetRecentDocumentsAsync(int count)
        {
            try
            {
                return await GetAsync<List<Models.Document>>($"documents/recent?count={count}");
            }
            catch (Exception ex)
            {
                HandleApiException("Error getting recent documents", ex);
                return new List<Models.Document>();
            }
        }

        // Annotation Methods
        public async Task<List<Annotation>> GetAnnotationsAsync(string documentId, int pageNumber)
        {
            try
            {
                return await GetAsync<List<Annotation>>($"annotations?documentId={documentId}&pageNumber={pageNumber}");
            }
            catch (Exception ex)
            {
                HandleApiException("Error getting annotations", ex);
                return new List<Annotation>();
            }
        }

        public async Task<List<Annotation>> GetAnnotationsForDocumentAsync(string documentId)
        {
            try
            {
                return await GetAsync<List<Annotation>>($"annotations/document/{documentId}");
            }
            catch (Exception ex)
            {
                HandleApiException("Error getting annotations for document", ex);
                return new List<Annotation>();
            }
        }

        public async Task<Annotation> CreateAnnotationAsync(Annotation annotation)
        {
            try
            {
                return await PostAsync<Annotation>("annotations", annotation);
            }
            catch (Exception ex)
            {
                HandleApiException("Error creating annotation", ex);
                throw; // Rethrow to allow the calling code to handle it
            }
        }

        public async Task<bool> DeleteAnnotationAsync(string annotationId)
        {
            try
            {
                return await DeleteAsync($"annotations/{annotationId}");
            }
            catch (Exception ex)
            {
                HandleApiException("Error deleting annotation", ex);
                return false;
            }
        }

        // Search Methods
        public async Task<List<SearchResult>> SearchAsync(string query, FilterOptions filters)
        {
            try
            {
                // Build query parameters
                var queryParams = new List<string>
                {
                    $"query={Uri.EscapeDataString(query)}"
                };

                // Add filter parameters if they exist
                if (filters != null)
                {
                    if (!string.IsNullOrEmpty(filters.Jurisdiction))
                    {
                        queryParams.Add($"jurisdiction={Uri.EscapeDataString(filters.Jurisdiction)}");
                    }

                    if (!string.IsNullOrEmpty(filters.DocumentType))
                    {
                        queryParams.Add($"documentType={Uri.EscapeDataString(filters.DocumentType)}");
                    }

                    if (!string.IsNullOrEmpty(filters.DateRange))
                    {
                        queryParams.Add($"dateRange={Uri.EscapeDataString(filters.DateRange)}");
                    }
                }

                // Join query parameters with &
                var queryString = string.Join("&", queryParams);

                return await GetAsync<List<SearchResult>>($"search/advanced?{queryString}");
            }
            catch (Exception ex)
            {
                HandleApiException("Error performing search", ex);
                return new List<SearchResult>();
            }
        }

        public async Task<FilterOptionsResult> GetFilterOptionsAsync()
        {
            try
            {
                return await GetAsync<FilterOptionsResult>("search/filter-options");
            }
            catch (Exception ex)
            {
                HandleApiException("Error getting filter options", ex);
                return new FilterOptionsResult();
            }
        }

        // Generic API Methods
        public async Task<T> GetAsync<T>(string endpoint)
        {
            ConfigureHttpClient();

            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                _hasShownConnectionError = false; // Reset flag on successful connection
                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch (Exception ex)
            {
                if (ex is HttpRequestException || ex is TaskCanceledException)
                {
                    // Show connection error only once to avoid spamming
                    if (!_hasShownConnectionError)
                    {
                        _toastService.ShowError($"Cannot connect to the API server. Please check your network connection.",
                            "Connection Error", autoClose: false);
                        _hasShownConnectionError = true;
                    }
                }
                throw;
            }
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            ConfigureHttpClient();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, data);
                response.EnsureSuccessStatusCode();

                _hasShownConnectionError = false; // Reset flag on successful connection
                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch (Exception ex)
            {
                if (ex is HttpRequestException || ex is TaskCanceledException)
                {
                    // Show connection error only once to avoid spamming
                    if (!_hasShownConnectionError)
                    {
                        _toastService.ShowError($"Cannot connect to the API server. Please check your network connection.",
                            "Connection Error", autoClose: false);
                        _hasShownConnectionError = true;
                    }
                }
                throw;
            }
        }

        public async Task<bool> PostAsync(string endpoint, object data)
        {
            ConfigureHttpClient();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, data);
                _hasShownConnectionError = false; // Reset flag on successful connection
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                if (ex is HttpRequestException || ex is TaskCanceledException)
                {
                    // Show connection error only once to avoid spamming
                    if (!_hasShownConnectionError)
                    {
                        _toastService.ShowError($"Cannot connect to the API server. Please check your network connection.",
                            "Connection Error", autoClose: false);
                        _hasShownConnectionError = true;
                    }
                }
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string endpoint)
        {
            ConfigureHttpClient();

            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                _hasShownConnectionError = false; // Reset flag on successful connection
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                if (ex is HttpRequestException || ex is TaskCanceledException)
                {
                    // Show connection error only once to avoid spamming
                    if (!_hasShownConnectionError)
                    {
                        _toastService.ShowError($"Cannot connect to the API server. Please check your network connection.",
                            "Connection Error", autoClose: false);
                        _hasShownConnectionError = true;
                    }
                }
                return false;
            }
        }

        private void HandleApiException(string operation, Exception ex)
        {
            Console.WriteLine($"{operation}: {ex.Message}");

            if (ex is HttpRequestException || ex is TaskCanceledException)
            {
                // Show connection error only once to avoid spamming
                if (!_hasShownConnectionError)
                {
                    _toastService.ShowError("Cannot connect to the API server. Please check your network connection.",
                        "Connection Error", autoClose: false);
                    _hasShownConnectionError = true;
                }
            }
            else
            {
                // For other types of errors, show specific message
                _toastService.ShowError($"{operation}: {ex.Message}", "Error", autoClose: false);
            }
        }

        private void ConfigureHttpClient()
        {
            // Add auth token if available
            var token = _preferenceService.Get("AuthToken", string.Empty);
            if (!string.IsNullOrEmpty(token))
            {
                if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                {
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                }
            }
        }
    }
}