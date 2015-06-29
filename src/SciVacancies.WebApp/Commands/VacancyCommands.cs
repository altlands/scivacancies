using SciVacancies.Domain.DataModels;

using System;

using MediatR;

namespace SciVacancies.WebApp.Commands
{
    public class PublishVacancyCommand : CommandBase, IRequest<Guid>
    {
        public Guid OrganizationGuid { get; set; }
        public Guid PositionGuid { get; set; }

        public VacancyDataModel Data { get; set; }
    }
    public class SwitchVacancyToAcceptApplicationsCommand : CommandBase, IRequest
    {
        public Guid OrganizationGuid { get; set; }
        public Guid VacancyGuid { get; set; }
    }
    public class SwitchVacancyInCommitteeCommand : CommandBase, IRequest
    {
        public Guid OrganizationGuid { get; set; }
        public Guid VacancyGuid { get; set; }
    }
    public class CloseVacancyCommand : CommandBase, IRequest
    {
        public Guid OrganizationGuid { get; set; }
        public Guid VacancyGuid { get; set; }

        public Guid WinnerGuid { get; set; }
        public Guid PretenderGuid { get; set; }
    }
    public class CancelVacancyCommand : CommandBase, IRequest
    {
        public Guid OrganizationGuid { get; set; }
        public Guid VacancyGuid { get; set; }

        public string Reason { get; set; }
    }
    public class SetVacancyWinnerCommand : CommandBase, IRequest
    {
        public Guid OrganizationGuid { get; set; }
        public Guid VacancyGuid { get; set; }

        public Guid ResearcherGuid { get; set; }
        public Guid VacancyApplicationGuid { get; set; }
    }
    public class SetVacancyPretenderCommand : CommandBase, IRequest
    {
        public Guid OrganizationGuid { get; set; }
        public Guid VacancyGuid { get; set; }

        public Guid ResearcherGuid { get; set; }
        public Guid VacancyApplicationGuid { get; set; }
    }


    public class AddVacancyToFavoritesCommand : CommandBase, IRequest<int>
    {
        public Guid ResearcherGuid { get; set; }
        public Guid VacancyGuid { get; set; }
    }
    public class RemoveVacancyFromFavoritesCommand : CommandBase, IRequest<int>
    {
        public Guid ResearcherGuid { get; set; }
        public Guid VacancyGuid { get; set; }
    }
}