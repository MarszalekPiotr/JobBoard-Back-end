using JobBoard.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class CandidateAcccount : BaseAccount
    {
        public required string SurName { get; set; }
        public required DateTimeOffset BirthDate { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public int UserId { get; set; }
        public User User { get; set; } = default!;
        public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    }
}
