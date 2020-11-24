using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.DocumentDomain.Shared.Models
{
    public class Document
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid DocumentChainId { get; init; }
        public string No { get; init; } = null!;
        public string Title { get; init; } = null!;
        public string Author { get; init; } = null!;
        public DateTime PublishedDate { get; init; } = DateTime.Now;
        public string[]? Levels { get; init; }
        public string[]? Tags { get; init; }
        public string Base64Content { get; init; } = null!;
    }
}
