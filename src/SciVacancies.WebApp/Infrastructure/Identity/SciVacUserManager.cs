using Microsoft.AspNet.Identity;

namespace SciVacancies.WebApp.Infrastructure.Identity
{
    public class SciVacUserManager: UserManager<SciVacUser>
    {
        public SciVacUserManager(IUserStore<SciVacUser> store) : base(store)
        {
            this.UserValidator = new UserValidator<SciVacUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false
            };
        }
    }
}
