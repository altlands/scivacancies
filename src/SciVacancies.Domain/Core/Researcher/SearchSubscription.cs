﻿using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Core
{
    public class SearchSubscription
    {
        public Guid SearchSubscriptionGuid { get; set; }

        public SearchSubscriptionDataModel Data { get; set; }

        public SearchSubscriptionStatus Status { get; set; }
    }
}
