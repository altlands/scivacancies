using SciVacancies.Domain.Interfaces;
using SciVacancies.Domain.Aggregates;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CommonDomain.Persistence.EventStore;

namespace SciVacancies.Domain.Services
{
    public class ResearcherService : IResearcherService
    {
        private EventStoreRepository _repository;
        
        public ResearcherService(EventStoreRepository repository)
        {
            _repository = repository;
        }

        public Guid CreateResearcher()
        {
            Researcher researcher = new Researcher(Guid.NewGuid());
            _repository.Save(researcher, Guid.NewGuid(), null);

            return researcher.Id;
        }
        public void RemoveResearcher(Guid researcherGuid)
        {
            Researcher researcher = _repository.GetById<Researcher>(researcherGuid);
            researcher.RemoveResearcher();
            _repository.Save(researcher, Guid.NewGuid(), null);
        }

        public Guid CreateVacancyApplicationTemplate(Guid researcherGuid, Guid vacancyGuid)
        {
            Researcher researcher = _repository.GetById<Researcher>(researcherGuid);
            Guid vacancyApplicationGuid = researcher.CreateVacancyApplicationTemplate(vacancyGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);

            return vacancyApplicationGuid;
        }
        public void ApplyToVacancy(Guid researcherGuid, Guid vacancyApplicationGuid)
        {
            Researcher researcher = _repository.GetById<Researcher>(researcherGuid);
            researcher.ApplyToVacancy(vacancyApplicationGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
    }
}
