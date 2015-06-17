using SciVacancies.Domain.Events;
using SciVacancies.Domain.Interfaces;
using SciVacancies.Domain.Aggregates;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CommonDomain.Persistence;

namespace SciVacancies.Domain.Services
{
    public class ResearcherService : IResearcherService
    {
        private readonly IRepository _repository;

        public ResearcherService(IRepository repository)
        {
            _repository = repository;
        }

        public Guid CreateResearcher(ResearcherDataModel data)
        {
            Researcher researcher = new Researcher(Guid.NewGuid(), data);
            _repository.Save(researcher, Guid.NewGuid(), null);

            return researcher.Id;
        }
        public void UpdateResearcher(Guid researcherGuid, ResearcherDataModel data)
        {
            Researcher researcher = _repository.GetById<Researcher>(researcherGuid);
            researcher.Update(data);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
        public void RemoveResearcher(Guid researcherGuid)
        {
            Researcher researcher = _repository.GetById<Researcher>(researcherGuid);
            researcher.Remove();
            _repository.Save(researcher, Guid.NewGuid(), null);
        }

        public int AddVacancyToFavorites(Guid researcherGuid, Guid vacancyGuid)
        {
            Researcher researcher = _repository.GetById<Researcher>(researcherGuid);
            int favoritesCount = researcher.AddVacancyToFavorites(vacancyGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);

            return favoritesCount;
        }
        public int RemoveVacancyFromFavorites(Guid researcherGuid, Guid vacancyGuid)
        {
            Researcher researcher = _repository.GetById<Researcher>(researcherGuid);
            int favoritesCount = researcher.RemoveVacancyFromFavorites(vacancyGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);

            return favoritesCount;
        }

        public Guid CreateSearchSubscription(Guid researcherGuid, SearchSubscriptionDataModel data)
        {
            Researcher researcher = _repository.GetById<Researcher>(researcherGuid);
            Guid searchSubscriptionGuid = researcher.CreateSearchSubscription(data);
            _repository.Save(researcher, Guid.NewGuid(), null);

            return searchSubscriptionGuid;
        }
        public void ActivateSearchSubscription(Guid researcherGuid, Guid searchSubscriptionGuid)
        {
            Researcher researcher = _repository.GetById<Researcher>(researcherGuid);
            researcher.ActivateSearchSubscription(searchSubscriptionGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
        public void CancelSearchSubscription(Guid researcherGuid, Guid searchSubscriptionGuid)
        {
            Researcher researcher = _repository.GetById<Researcher>(researcherGuid);
            researcher.CancelSearchSubscription(searchSubscriptionGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
        public void RemoveSearchSubscription(Guid researcherGuid, Guid searchSubscriptionGuid)
        {
            Researcher researcher = _repository.GetById<Researcher>(researcherGuid);
            researcher.RemoveSearchSubscription(searchSubscriptionGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }

        public Guid CreateVacancyApplicationTemplate(Guid researcherGuid, Guid vacancyGuid)
        {
            Researcher researcher = _repository.GetById<Researcher>(researcherGuid);
            Guid vacancyApplicationGuid = researcher.CreateVacancyApplicationTemplate(vacancyGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);

            return vacancyApplicationGuid;
        }
        public void UpdateVacancyApplicationTemplate(Guid researcherGuid, Guid vacancyApplicationGuid, VacancyApplicationDataModel data)
        {
            Researcher researcher = _repository.GetById<Researcher>(researcherGuid);
            researcher.UpdateVacancyApplicationTemplate(vacancyApplicationGuid, data);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
        public void RemoveVacancyApplicationTemplate(Guid researcherGuid, Guid vacancyApplicationGuid)
        {
            Researcher researcher = _repository.GetById<Researcher>(researcherGuid);
            researcher.RemoveVacancyApplicationTemplate(vacancyApplicationGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
        public void ApplyToVacancy(Guid researcherGuid, Guid vacancyApplicationGuid)
        {
            Researcher researcher = _repository.GetById<Researcher>(researcherGuid);
            researcher.ApplyToVacancy(vacancyApplicationGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
    }
}
