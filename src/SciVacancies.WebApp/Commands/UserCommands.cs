using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.ViewModels;

using MediatR;

namespace SciVacancies.WebApp.Commands
{
    public class RegisterUserResearcherCommand : CommandBase, IRequest<SciVacUser>
    {
        public AccountResearcherRegisterViewModel Data { get; set; }
    }

    public class RegisterUserOrganizationCommand : CommandBase, IRequest<SciVacUser>
    {
        public AccountOrganizationRegisterViewModel Data { get; set; }
    }
}
