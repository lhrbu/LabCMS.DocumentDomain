using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.DocumentDomain.Shared.Models
{
    public class DocumentChain
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public List<Guid> Members { get; } = new();
    }
}
