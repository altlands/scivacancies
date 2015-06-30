using MediatR;
using Nest;
using SciVacancies.Domain.Events;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.ElasticSearchModel.Model;

namespace SciVacancies.ReadModel.ElasticSearchModel.EventHandlers
{
    public class VacancyEventsHandler : 
        INotificationHandler<VacancyPublished>,
        INotificationHandler<VacancyAcceptApplications>,
        INotificationHandler<VacancyInCommittee>,
        INotificationHandler<VacancyClosed>,
        INotificationHandler<VacancyCancelled>
    {
        private readonly IElasticClient _elasticClient;

        public VacancyEventsHandler(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }
        public void Handle(VacancyPublished msg)
        {
            Vacancy vacancy = new Vacancy()
            {
                Id = msg.VacancyGuid,
                PositionGuid = msg.PositionGuid,
                OrganizationGuid = msg.OrganizationGuid,
                Name = msg.Data.Name,

                ResearchDirectionId = msg.Data.ResearchDirectionId,
                ResearchThemeId = msg.Data.ResearchThemeId,

                OrganizationName=msg.Data.OrganizationName,

                FullName=msg.Data.FullName,

                Status=msg.Data.Status,

               //Foiv=msg.Data.fo

                ResearchDirection = msg.Data.ResearchDirection,
                ResearchTheme = msg.Data.ResearchTheme,
                Tasks = msg.Data.Tasks,
                SalaryFrom = msg.Data.SalaryFrom,
                SalaryTo = msg.Data.SalaryTo,
                Bonuses = msg.Data.Bonuses,
                ContractType = msg.Data.ContractType,
                ContractTime = msg.Data.ContractTime,
                SocialPackage = msg.Data.SocialPackage,
                Rent = msg.Data.Rent,
                OfficeAccomodation = msg.Data.OfficeAccomodation,
                TransportCompensation = msg.Data.TransportCompensation,

                Region = msg.Data.Region,
                RegionId = msg.Data.RegionId,

                CityName = msg.Data.CityName,
                Details = msg.Data.Details,
                ContactName = msg.Data.ContactName,
                ContactEmail = msg.Data.ContactEmail,
                ContactPhone = msg.Data.ContactPhone,
                ContactDetails = msg.Data.ContactDetails,
                DateStart = msg.Data.DateStart,
                DateStartAcceptance = msg.Data.DateStartAcceptance,
                DateFinish = msg.Data.DateFinish
            };

            _elasticClient.Index(vacancy);
        }

        public void Handle(VacancyAcceptApplications msg)
        {
            Vacancy vacancy = _elasticClient.Get<Vacancy>(msg.VacancyGuid.ToString()).Source;
            vacancy.Status = VacancyStatus.AppliesAcceptance;

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }

        public void Handle(VacancyInCommittee msg)
        {
            Vacancy vacancy = _elasticClient.Get<Vacancy>(msg.VacancyGuid.ToString()).Source;
            vacancy.Status = VacancyStatus.InCommittee;

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }

        public void Handle(VacancyClosed msg)
        {
            Vacancy vacancy = _elasticClient.Get<Vacancy>(msg.VacancyGuid.ToString()).Source;
            vacancy.Status = VacancyStatus.Closed;

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }

        public void Handle(VacancyCancelled msg)
        {
            Vacancy vacancy = _elasticClient.Get<Vacancy>(msg.VacancyGuid.ToString()).Source;
            vacancy.Status = VacancyStatus.Cancelled;

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }
    }
}
