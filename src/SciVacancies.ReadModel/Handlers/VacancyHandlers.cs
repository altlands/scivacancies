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
            Vacancy vacancy = new Vacancy()
            {
                
            };
            //TODO
            _db.Insert(vacancy);
        }
    }
    public class VacancyAcceptApplicationsHandler : EventBaseHandler<VacancyAcceptApplications>
    {
        public VacancyAcceptApplicationsHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyAcceptApplications msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
            //TODO
            _db.Update(vacancy);
        }
    }
    public class VacancyInCommitteeHandler : EventBaseHandler<VacancyInCommittee>
    {
        public VacancyInCommitteeHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyInCommittee msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
            //TODO
            _db.Update(vacancy);
        }
    }
    public class VacancyClosedHandler : EventBaseHandler<VacancyClosed>
    {
        public VacancyClosedHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyClosed msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
            //TODO
            _db.Update(vacancy);
        }
    }
    public class VacancyCancelledHandler : EventBaseHandler<VacancyCancelled>
    {
        public VacancyCancelledHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyCancelled msg)
        {
            Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
            //TODO
            _db.Update(vacancy);
        }
    }

    public class VacancyAddedToFavoritesHandler : EventBaseHandler<VacancyAddedToFavorites>
    {
        public VacancyAddedToFavoritesHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyAddedToFavorites msg)
        {
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
            FavoriteVacancy favoriteVacancy = _db.FetchBy<FavoriteVacancy>(sql => sql.Where(x => x.VacancyGuid == msg.VacancyGuid && x.ResearcherGuid == msg.ResearcherGuid)).FirstOrDefault();
            _db.Delete(favoriteVacancy);
        }
    }
}
