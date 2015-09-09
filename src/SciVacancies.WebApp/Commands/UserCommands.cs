using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.ViewModels;

using MediatR;
using SciVacancies.WebApp.Models.DataModels;

namespace SciVacancies.WebApp.Commands
{
    public class RegisterUserResearcherCommand : CommandBase, IRequest<SciVacUser>
    {
        public ResearcherRegisterDataModel Data { get; set; }
    }

    public class RegisterUserOrganizationCommand : CommandBase, IRequest<SciVacUser>
    {
        //TODO: написать и использовать вместо AccountOrganizationRegisterViewModel класс AccountOrganizationRegisterDataModel
        public AccountOrganizationRegisterViewModel Data { get; set; }
    }
}
