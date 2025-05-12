using System;
using System.Collections.Generic;

namespace ASNLawClient.Client.Helpers
{
    public static class QueryHelpers
    {
        public static Dictionary<string, string> ParseQuery(string queryString)
        {
            var query = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(queryString))
                return query;

            // Remove the leading '?' if present
            if (queryString.StartsWith('?'))
                queryString = queryString.Substring(1);

            // Split query string by '&'
            var pairs = queryString.Split('&');

            foreach (var pair in pairs)
            {
                // Skip empty pairs
                if (string.IsNullOrEmpty(pair))
                    continue;

                // Split pair by '='
                var parts = pair.Split('=');
                var key = Uri.UnescapeDataString(parts[0]);

                // If no value, assign empty string
                var value = parts.Length > 1 ? Uri.UnescapeDataString(parts[1]) : string.Empty;

                // Add to dictionary, overwriting if key already exists
                query[key] = value;
            }

            return query;
        }

        public static bool TryGetValue(Dictionary<string, string> query, string key, out string value)
        {
            if (query.TryGetValue(key, out var val))
            {
                value = val;
                return true;
            }

            value = string.Empty;
            return false;
        }
    }
}