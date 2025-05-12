using System.Collections.Generic;

namespace ASNLawClient.Client.Models
{
    public class FilterOptionsResult
    {
        public List<string> Jurisdictions { get; set; } = new List<string>();
        public List<string> DocumentTypes { get; set; } = new List<string>();
        public List<string> Tags { get; set; } = new List<string>();
        public List<string> Categories { get; set; } = new List<string>();
    }
}