using JobBoard.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class OfferTag : DomainEntity
    {
        public int TagId { get; set; }
        public Tag Tag { get; set; } = default!;
        public int OfferId { get; set; }
        public Offer Offer { get; set; } = default!;
    }
}
