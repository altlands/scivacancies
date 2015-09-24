using System.ComponentModel;

namespace SciVacancies.WebApp.Engine
{
    public enum AuthorizeUserTypes
    {
        [Description("Администратор")]
        Admin = 0,

        [Description("Исследователь")]
        Researcher = 1,

        [Description("Организация")]
        Organization= 2
    }
    public enum AuthorizeResourceTypes
    {
        [Description("Собственная авторизация")]
        OwnAuthorization = 0,

        [Description("Карта Науки")]
        ScienceMap = 1,

        [Description("Информика")]
        Sciencemon = 2
    }

}
