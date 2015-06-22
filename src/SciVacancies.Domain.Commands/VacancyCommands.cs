using SciVacancies.Domain.DataModels;

using System;

using MediatR;

namespace SciVacancies.Domain.Commands
{
    public class PublishVacancy : CommandBase, IRequest<Guid>
    {
        public PublishVacancy() : base() { }

        public Guid OrganizationGuid { get; set; }
        public Guid PositionGuid { get; set; }

        public VacancyDataModel Data { get; set; }
    }
    public class SwitchVacancyToAcceptApplications : CommandBase, IRequest
    {
        public SwitchVacancyToAcceptApplications() : base() { }

        public Guid OrganizationGuid { get; set; }
        public Guid VacancyGuid { get; set; }
    }
    public class SwitchVacancyInCommittee : CommandBase, IRequest
    {
        public SwitchVacancyInCommittee() : base() { }

        public Guid OrganizationGuid { get; set; }
        public Guid VacancyGuid { get; set; }
    }
    public class CloseVacancy : CommandBase, IRequest
    {
        public CloseVacancy() : base() { }

        public Guid OrganizationGuid { get; set; }
        public Guid VacancyGuid { get; set; }

        public Guid WinnerGuid { get; set; }
        public Guid PretenderGuid { get; set; }
    }
    public class CancelVacancy : CommandBase, IRequest
    {
        public CancelVacancy() : base() { }

        public Guid OrganizationGuid { get; set; }
        public Guid VacancyGuid { get; set; }

        public string Reason { get; set; }
    }

    public class AddVacancyToFavorites : CommandBase, IRequest<int>
    {
        public AddVacancyToFavorites() : base() { }

        public Guid ResearcherGuid { get; set; }
        public Guid VacancyGuid { get; set; }
    }
    public class RemoveVacancyFromFavorites : CommandBase, IRequest<int>
    {
        public RemoveVacancyFromFavorites() : base() { }

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