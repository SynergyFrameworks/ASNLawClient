using System;
using System.Collections.Generic;

namespace ASNLawClient.Client.Models
{
    public class Annotation
    {
        public string Id { get; set; } = string.Empty;
        public string DocumentId { get; set; } = string.Empty;
        public string? ChunkId { get; set; }
        public int Page { get; set; }
        public string Type { get; set; } = string.Empty;
        public string? Text { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public bool IsShared { get; set; }
        public bool IsApproved { get; set; }
        public string? ParentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Annotation> Replies { get; set; } = new();
    }
}