using System;
using System.Collections.Generic;

namespace ASNLawClient.Client.Models
{
    public class SearchResult
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string Jurisdiction { get; set; } = string.Empty;
        public string Snippet { get; set; } = string.Empty;
        public List<string> MatchedTerms { get; set; } = new List<string>();
        public double Score { get; set; }
        public DateTime PublishedDate { get; set; }
        public List<string> Categories { get; set; } = new List<string>();
        public List<string> Tags { get; set; } = new List<string>();
        public List<int> MatchedPages { get; set; } = new List<int>();
    }
}