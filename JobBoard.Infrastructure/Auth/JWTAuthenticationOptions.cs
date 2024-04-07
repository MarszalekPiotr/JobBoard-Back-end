using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Auth
{
    public  class JWTAuthenticationOptions
    {
        public string? Secret {  get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int ExpireInDays { get; set; }
    }
}
