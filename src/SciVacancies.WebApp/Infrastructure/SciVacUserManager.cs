using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SciVacancies.WebApp.Infrastructure
{
    public class SciVacUserManager: UserManager<SciVacUser>
    {
        public SciVacUserManager(IUserStore<SciVacUser> store) : base(store)
        {
        }

        //public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        //{
        //    var appDbContext = context.Get<ApplicationDbContext>();
        //    var appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(appDbContext));

        //    return appUserManager;
        //}
    }
}
