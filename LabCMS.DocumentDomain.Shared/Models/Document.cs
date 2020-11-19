using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.DocumentDomain.Shared.Models
{
    public class Document
    {
        public DocumentIndex Index { get; init; } = null!;
        public DocumentContent Content { get; init; } = null!;
    }
}
