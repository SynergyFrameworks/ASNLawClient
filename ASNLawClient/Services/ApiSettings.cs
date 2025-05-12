using System;

namespace ASNLawClient.Client.Options
{
    public class ApiSettings
    {
        public const string SectionName = "ApiSettings";

        public string BaseUrl { get; set; } = "https://localhost:5001/api/";
        public int TimeoutSeconds { get; set; } = 30;
    }
}