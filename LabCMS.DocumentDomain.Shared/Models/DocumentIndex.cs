using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.DocumentDomain.Shared.Models
{
    public class DocumentIndex
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? No { get; set; }
        public string? Version { get; set; }
        public string? Name { get; set; }
        public string? Author { get; set; }
        public DateTime PublishedDate { get; set; } = DateTime.Now;
        public string[] Levels { get; set; } = Array.Empty<string>();
        public string[]? Tags { get; set; }

    }
}
