using System;
using System.Collections.Generic;

namespace Api.Models
{
    public partial class LeadManager
    {
        public LeadManager()
        {
            SeniorManagers = new HashSet<SeniorManager>();
        }

        public int CompanyCode { get; set; }
        public int LeadManagerCode { get; set; }

        public virtual Company CompanyCodeNavigation { get; set; } = null!;
        public virtual ICollection<SeniorManager> SeniorManagers { get; set; }
    }
}
