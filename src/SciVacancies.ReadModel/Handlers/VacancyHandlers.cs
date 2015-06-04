using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Handlers
{
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
