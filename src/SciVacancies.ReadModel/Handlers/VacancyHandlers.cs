using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Handlers
{
    public class VacancyPublishedHandler : EventBaseHandler<VacancyPublished>
    {
        public VacancyPublishedHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyPublished msg)
        {
            Position position = _db.SingleById<Position>(msg.PositionGuid);
            position.Status = PositionStatus.Published;

            _db.Update(position);

            Vacancy vacancy = new Vacancy()
            {
                Guid=msg.VacancyGuid,
                PositionGuid=msg.PositionGuid,
                OrganizationGuid = msg.OrganizationGuid,
                Name = msg.Data.Name,
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
    }
    public class VacancyAcceptApplicationsHandler : EventBaseHandler<VacancyAcceptApplications>
    {
        public VacancyAcceptApplicationsHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyAcceptApplications msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
            vacancy.Status = VacancyStatus.AppliesAcceptance;
            _db.Update(vacancy);
        }
    }
    public class VacancyInCommitteeHandler : EventBaseHandler<VacancyInCommittee>
    {
        public VacancyInCommitteeHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyInCommittee msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
            vacancy.Status = VacancyStatus.InCommittee;

            _db.Update(vacancy);
        }
    }
    public class VacancyClosedHandler : EventBaseHandler<VacancyClosed>
    {
        public VacancyClosedHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyClosed msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
            vacancy.Status = VacancyStatus.Closed;

            _db.Update(vacancy);
        }
    }
    public class VacancyCancelledHandler : EventBaseHandler<VacancyCancelled>
    {
        public VacancyCancelledHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyCancelled msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
            vacancy.Status = VacancyStatus.Cancelled;

            _db.Update(vacancy);
        }
    }

    public class VacancyAddedToFavoritesHandler : EventBaseHandler<VacancyAddedToFavorites>
    {
        public VacancyAddedToFavoritesHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyAddedToFavorites msg)
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
    }
    public class VacancyRemovedFromFavoritesHandler : EventBaseHandler<VacancyRemovedFromFavorites>
    {
        public VacancyRemovedFromFavoritesHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyRemovedFromFavorites msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
            vacancy.FollowersCounter--;

            _db.Update(vacancy);

            FavoriteVacancy favoriteVacancy = _db.FetchBy<FavoriteVacancy>(sql => sql.Where(x => x.VacancyGuid == msg.VacancyGuid && x.ResearcherGuid == msg.ResearcherGuid)).FirstOrDefault();
            _db.Delete(favoriteVacancy);
        }
    }
}
