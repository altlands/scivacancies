using Microsoft.AspNet.Identity.EntityFramework;

namespace SciVacancies.WebApp.Infrastructure.Identity
{
    public class SciVacUserStore: UserStore<SciVacUser>
    {
        public SciVacUserStore(SciVacUserDbContext dbcontext) : base(dbcontext) {}
    }
}
