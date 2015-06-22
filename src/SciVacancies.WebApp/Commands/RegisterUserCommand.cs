using MediatR;
using SciVacancies.Domain.Aggregates.Commands;
using SciVacancies.WebApp.Infrastructure;
using SciVacancies.WebApp.Infrastructure.Identity;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Commands
{
    public class RegisterUserCommand : CommandBase, IRequest<SciVacUser>
    {       
        public AccountRegisterViewModel Data { get; set; }
    }
}
