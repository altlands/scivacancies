using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.ViewModels;

using MediatR;

namespace SciVacancies.WebApp.Commands
{
    public class RegisterUserCommand : CommandBase, IRequest<SciVacUser>
    {
        public RegisterUserCommand() : base() { }

        public AccountRegisterViewModel Data { get; set; }
    }
}
