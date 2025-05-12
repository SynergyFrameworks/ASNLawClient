using System;
using System.Collections.Generic;

namespace ASNLawClient.Client.Models
{
    public class Document
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Jurisdiction { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public string PdfUrl { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        public List<string> Tags { get; set; } = new List<string>();
        public string Description { get; set; } = string.Empty;
        public int PageCount { get; set; } = 0;
        public string Author { get; set; } = string.Empty;
        public bool IsFavorite { get; set; } = false;
    }
}