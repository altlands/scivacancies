using Microsoft.AspNet.Identity.EntityFramework;

namespace SciVacancies.WebApp.Infrastructure
{
    public class SciVacUserStore: UserStore<SciVacUser>
    {
        public SciVacUserStore(SciVacUserDbContext dbcontext) : base(dbcontext) {}
    }
}
