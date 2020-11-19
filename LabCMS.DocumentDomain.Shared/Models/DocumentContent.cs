using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.DocumentDomain.Shared.Models
{
    public class DocumentContent
    {
        public Guid Id { get; init; }
        public string Base64Content { get; init; } = null!;
    }
}
