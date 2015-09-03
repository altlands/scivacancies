using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Infrastructure.Identity;
using System.Linq;

namespace SciVacancies.WebApp.ViewComponents
{
    public class CallEmailConfirm : ViewComponent
    {
        private readonly SciVacUserManager _userManager;

        public CallEmailConfirm(SciVacUserManager userManager)
        {
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            bool result = false; //нужно или нет запросить подтверждение Email
            if (User.Identity.IsAuthenticated)
            {
                var user = _userManager.FindByName(User.Identity.Name);
                if (user != null)
                {
                    var logins = _userManager.GetLogins(user.Id);
                    
                    //проверяем, что пользователь Не чужой (и нам нужно подтверждать его Email)
                    if (logins == null || !logins.Any())
                    {
                        result = !user.EmailConfirmed;
                    }
                }
            }
            return View("/Views/Partials/_CallEmailConfirm", result);
        }
    }
}
