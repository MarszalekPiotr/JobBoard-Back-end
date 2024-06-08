using JobBoard.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class Tag : DomainEntity
    {
        public required string Name { get; set; }
        public ICollection<OfferTag> OfferTags { get; set; } = new List<OfferTag>();
    }
}
