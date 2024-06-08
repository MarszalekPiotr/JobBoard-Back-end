using JobBoard.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class FieldFilling : DomainEntity
    {
        public int FormFillingId { get; set; }
        public FormFilling FormFilling { get; set; } = default!;
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
