using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Common
{
    public abstract  class BaseAccount
    { 
        public int Id { get; set; }
        public required string Name { get; set; }
        public required DateTimeOffset CreationDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
    }
}
