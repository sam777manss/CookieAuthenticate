using System;
using System.Collections.Generic;

namespace Api.Models
{
    public partial class SeniorManager
    {
        public int SeniorManagerCode { get; set; }
        public int? LeadManagerCode { get; set; }

        public virtual LeadManager? LeadManagerCodeNavigation { get; set; }
    }
}
