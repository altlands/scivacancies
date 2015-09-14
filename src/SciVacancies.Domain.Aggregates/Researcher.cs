using SciVacancies.Domain.Core;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;

using System;
using System.Collections.Generic;

using CommonDomain.Core;

namespace SciVacancies.Domain.Aggregates
{
    public class Researcher : AggregateBase
    {
        private ResearcherDataModel Data { get; set; }

        public ResearcherStatus Status { get; private set; }

        public List<Guid> FavoriteVacancyGuids { get; private set; }

        public Researcher()
        {
            FavoriteVacancyGuids = new List<Guid>();
        }
        public Researcher(Guid guid, ResearcherDataModel data)
        {
            if (guid.Equals(Guid.Empty)) throw new ArgumentNullException("guid is empty");
            if (data == null) throw new ArgumentNullException("data is empty");

            FavoriteVacancyGuids = new List<Guid>();

            RaiseEvent(new ResearcherCreated()
            {
                ResearcherGuid = guid,
                Data = data
            });
        }

        #region Methods

        public void Update(ResearcherDataModel data)
        {
            if (data == null) throw new ArgumentNullException("data is empty");
            if (Status != ResearcherStatus.Active) throw new InvalidOperationException("researcher state is invalid");

            RaiseEvent(new ResearcherUpdated()
            {
                ResearcherGuid = this.Id,
                Data = data
            });
        }
        public void Remove()
        {
            if (Status == ResearcherStatus.Removed) throw new InvalidOperationException("researcher state is invalid");

            RaiseEvent(new ResearcherRemoved()
            {
                ResearcherGuid = this.Id
            });
        }

        public int AddVacancyToFavorites(Guid vacancyGuid)
        {
            if (vacancyGuid.Equals(Guid.Empty)) throw new ArgumentNullException("vacancyGuid is empty");
            if (FavoriteVacancyGuids.Contains(vacancyGuid)) throw new ArgumentException("vacancyGuid has been added already");
            if (Status != ResearcherStatus.Active) throw new InvalidOperationException("researcher state is invalid");

            RaiseEvent(new VacancyAddedToFavorites()
            {
                VacancyGuid = vacancyGuid,
                ResearcherGuid = this.Id
            });

            return this.FavoriteVacancyGuids.Count + 1;
        }
        public int RemoveVacancyFromFavorites(Guid vacancyGuid)
        {
            if (vacancyGuid.Equals(Guid.Empty)) throw new ArgumentNullException("vacancyGuid is empty");
            if (!FavoriteVacancyGuids.Contains(vacancyGuid)) throw new ArgumentException("list doesn't have vacancyGuid as member");
            if (Status != ResearcherStatus.Active) throw new InvalidOperationException("researcher state is invalid");

            RaiseEvent(new VacancyRemovedFromFavorites()
            {
                VacancyGuid = vacancyGuid,
                ResearcherGuid = this.Id
            });

            return this.FavoriteVacancyGuids.Count - 1;
        }

        #endregion

        #region Apply-Handlers

        public void Apply(ResearcherCreated @event)
        {
            this.Id = @event.ResearcherGuid;
            this.Data = @event.Data;
        }
        public void Apply(ResearcherUpdated @event)
        {
            this.Data = @event.Data;
        }
        public void Apply(ResearcherRemoved @event)
        {
            this.Status = ResearcherStatus.Removed;
        }

        public void Apply(VacancyAddedToFavorites @event)
        {
            this.FavoriteVacancyGuids.Add(@event.VacancyGuid);
        }
        public void Apply(VacancyRemovedFromFavorites @event)
        {
            this.FavoriteVacancyGuids.Remove(@event.VacancyGuid);
        }

        #endregion
    }
}
