using JobBoard.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class City : DomainEntity
    {
        public required  string Name { get; set; } 
        public ICollection<Offer> Offers { get; set; } = new List<Offer>();
    }
}
