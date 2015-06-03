using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Core
{
    public class SearchSubscription
    {
        public Guid SearchSubscriptionGuid { get; set; }
        
        public SearchSubscriptionStatus Status { get; set; }
    }
}
