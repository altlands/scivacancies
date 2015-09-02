using System.Collections.Generic;
using System.Security.Claims;

namespace SciVacancies.WebApp.Models.DataModels
{
    /// <summary>
    /// класс для регистрации пользователя
    /// </summary>
    public class ResearcherRegisterDataModel : ResearcherUpdateDataModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Сюда пишутся access_token  и прочее, если OAuth авторизация
        /// </summary>
        public List<Claim> Claims { get; set; }
    }
}
