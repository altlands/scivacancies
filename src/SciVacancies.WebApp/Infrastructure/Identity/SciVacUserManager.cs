using Microsoft.AspNet.DataProtection;
using Microsoft.AspNet.Identity;

namespace SciVacancies.WebApp.Infrastructure.Identity
{
    public class SciVacUserManager : UserManager<SciVacUser>
    {
        public SciVacUserManager(IUserStore<SciVacUser> store) : base(store)
        {
            UserValidator = new UserValidator<SciVacUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false
            };

            UserTokenProvider = new EmailTokenProvider<SciVacUser, string>();

        }
    }
}
