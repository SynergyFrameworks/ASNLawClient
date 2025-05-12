using System.Collections.Generic;

namespace ASNLawClient.Client.Models
{
    public class FilterOptions
    {
        public string Jurisdiction { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string DateRange { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public string SortBy { get; set; } = "relevance";
        public bool IncludeArchived { get; set; } = false;
    }
}