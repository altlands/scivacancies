using System;
using SciVacancies.Domain.Enums;

namespace SciVacancies.WebApp.Interfaces
{
    public interface IVacancyWinnerPretenderInfo
    {
        VacancyStatus Status { get; set; }

        bool? IsWinnerAccept { get; set; }
        Guid WinnerResearcherGuid { get; set; }
        DateTime? WinnerRequestDate { get; set; }
        DateTime? WinnerResponseDate { get; set; }
        Guid WinnerVacancyApplicationGuid { get; set; }

        bool? IsPretenderAccept { get; set; }
        Guid PretenderResearcherGuid { get; set; }
        DateTime? PretenderRequestDate { get; set; }
        DateTime? PretenderResponseDate { get; set; }
        Guid PretenderVacancyApplicationGuid { get; set; }
    }
}
