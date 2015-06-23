using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace SciVacancies.WebApp.Infrastructure.Identity
{
    public class SciVacUser : IdentityUser
    {
        public Guid OrganizationGuid { get; set; }
        public Guid ResearcherGuid { get; set; }
    }
}
