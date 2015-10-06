using FluentValidation;
using SciVacancies.WebApp.Commands;

namespace SciVacancies.WebApp.Validation
{
    public class CreatePositionCommandValidator : AbstractValidator<CreateVacancyCommand>
    {
        public CreatePositionCommandValidator()
        {
            RuleFor(c => c.Data.Name).NotEmpty();
        }
    }
}
