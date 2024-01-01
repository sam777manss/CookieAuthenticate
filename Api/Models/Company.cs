using System;
using System.Collections.Generic;

namespace Api.Models
{
    public partial class Company
    {
        public Company()
        {
            LeadManagers = new HashSet<LeadManager>();
        }

        public int CompanyCode { get; set; }
        public string Founder { get; set; } = null!;

        public virtual ICollection<LeadManager> LeadManagers { get; set; }
    }
}
