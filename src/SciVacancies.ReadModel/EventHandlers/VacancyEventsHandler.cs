using System.Linq;
using MediatR;
using NPoco;
using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.ReadModel.EventHandlers
{
    public class VacancyEventsHandler : 
        INotificationHandler<VacancyPublished>, 
        INotificationHandler<VacancyAcceptApplications>,
        INotificationHandler<VacancyInCommittee>,
        INotificationHandler<VacancyClosed>,
        INotificationHandler<VacancyCancelled>,
        INotificationHandler<VacancyAddedToFavorites>,
        INotificationHandler<VacancyRemovedFromFavorites>
    {
        private readonly IDatabase _db;

        public VacancyEventsHandler(IDatabase db)
        {
            _db = db;
        }
        public void Handle(VacancyPublished msg)
        {
            Position position = _db.SingleById<Position>(msg.PositionGuid);
            position.Status = PositionStatus.Published;

            _db.Update(position);

            Vacancy vacancy = new Vacancy()
            {
                Guid = msg.VacancyGuid,
                PositionGuid = msg.PositionGuid,
                OrganizationGuid = msg.OrganizationGuid,
                Name = msg.Data.Name,

                //TODO - Clean Up
                PositionTypeGuid = msg.Data.PositionTypeGuid,
                ResearchDirectionId = msg.Data.ResearchDirectionId,
                ResearchThemeId = msg.Data.ResearchThemeId,



                FullName = msg.Data.FullName,
                ResearchDirection = msg.Data.ResearchDirection,
                ResearchTheme = msg.Data.ResearchTheme,
                Tasks = msg.Data.Tasks,
                Criteria = msg.Data.Criteria,
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

            _db.Insert(vacancy);
        }

        public void Handle(VacancyAcceptApplications msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
            vacancy.Status = VacancyStatus.AppliesAcceptance;

            _db.Update(vacancy);
        }

        public void Handle(VacancyInCommittee msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
            vacancy.Status = VacancyStatus.InCommittee;

            _db.Update(vacancy);
        }

        public void Handle(VacancyClosed msg)
        {
            Position position = _db.SingleById<Position>(msg.PositionGuid);
            position.Status = PositionStatus.InProcess;

            _db.Update(position);

            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
            vacancy.Status = VacancyStatus.Closed;

            _db.Update(vacancy);
        }

        public void Handle(VacancyCancelled msg)
        {
            Position position = _db.SingleById<Position>(msg.PositionGuid);
            position.Status = PositionStatus.InProcess;

            _db.Update(position);

            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
            vacancy.Status = VacancyStatus.Cancelled;

            _db.Update(vacancy);
        }
        public void Handle(VacancyWinnerSet msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
            vacancy.WinnerGuid = msg.ReasearcherGuid;
            vacancy.Status = VacancyStatus.Cancelled;

            _db.Update(vacancy);
        }
        public void Handle(VacancyPretenderSet msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
            vacancy.PretenderGuid = msg.ReasearcherGuid;

            _db.Update(vacancy);
        }

        public void Handle(VacancyAddedToFavorites msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
            vacancy.FollowersCounter++;

            _db.Update(vacancy);

            FavoriteVacancy favoriteVacancy = new FavoriteVacancy()
            {
                VacancyGuid = msg.VacancyGuid,
                ResearcherGuid = msg.ResearcherGuid
            };

            _db.Insert(favoriteVacancy);
        }

        public void Handle(VacancyRemovedFromFavorites msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
            vacancy.FollowersCounter--;

            _db.Update(vacancy);

            FavoriteVacancy favoriteVacancy = _db.FetchBy<FavoriteVacancy>(sql => sql.Where(x => x.VacancyGuid == msg.VacancyGuid && x.ResearcherGuid == msg.ResearcherGuid)).FirstOrDefault();
            _db.Delete(favoriteVacancy);
        }
    }
}
