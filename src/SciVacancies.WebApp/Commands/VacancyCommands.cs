using SciVacancies.Domain.DataModels;

using System;

using MediatR;

namespace SciVacancies.WebApp.Commands
{
    public class PublishVacancyCommand : CommandBase, IRequest<Guid>
    {
        public PublishVacancyCommand() : base() { }

        public Guid OrganizationGuid { get; set; }
        public Guid PositionGuid { get; set; }

        public VacancyDataModel Data { get; set; }
    }
    public class SwitchVacancyToAcceptApplicationsCommand : CommandBase, IRequest
    {
        public SwitchVacancyToAcceptApplicationsCommand() : base() { }

        public Guid OrganizationGuid { get; set; }
        public Guid VacancyGuid { get; set; }
    }
    public class SwitchVacancyInCommitteeCommand : CommandBase, IRequest
    {
        public SwitchVacancyInCommitteeCommand() : base() { }

        public Guid OrganizationGuid { get; set; }
        public Guid VacancyGuid { get; set; }
    }
    public class CloseVacancyCommand : CommandBase, IRequest
    {
        public CloseVacancyCommand() : base() { }

        public Guid OrganizationGuid { get; set; }
        public Guid VacancyGuid { get; set; }

        public Guid WinnerGuid { get; set; }
        public Guid PretenderGuid { get; set; }
    }
    public class CancelVacancyCommand : CommandBase, IRequest
    {
        public CancelVacancyCommand() : base() { }

        public Guid OrganizationGuid { get; set; }
        public Guid VacancyGuid { get; set; }

        public string Reason { get; set; }
    }

    public class AddVacancyToFavoritesCommand : CommandBase, IRequest<int>
    {
        public AddVacancyToFavoritesCommand() : base() { }

        public Guid ResearcherGuid { get; set; }
        public Guid VacancyGuid { get; set; }
    }
    public class RemoveVacancyFromFavoritesCommand : CommandBase, IRequest<int>
    {
        public RemoveVacancyFromFavoritesCommand() : base() { }

        public Guid ResearcherGuid { get; set; }
        public Guid VacancyGuid { get; set; }
    }
}








//namespace SciVacancies.Domain.Commands
//{

//    public class CreateOrganization : CommandBase, IRequest<Guid>
//    {
//        public CreateOrganization() : base() { }

//        OrganizationDataModel Data { get; set; }
//    }
//    public class UpdateOrganization : IRequest
//    {
//        public UpdateOrganization() : base() { }

//        public Guid OrganizationGuid { get; set; }

//        public OrganizationDataModel Data { get; set; }
//    }
//    public class RemoveOrganization : IRequest
//    {
//        public Guid OrganizationGuid { get; set; }
//    }
//}