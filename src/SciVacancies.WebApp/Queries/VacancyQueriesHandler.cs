using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;

using NPoco;
using MediatR;
using Nest;

namespace SciVacancies.WebApp.Queries
{
    public class VacancyQueriesHandler :
        IRequestHandler<SingleVacancyQuery, Vacancy>,
        IRequestHandler<SelectPagedVacanciesQuery, Page<Vacancy>>,
        IRequestHandler<SelectPagedVacanciesByOrganizationQuery, Page<Vacancy>>,
        IRequestHandler<SelectVacanciesForAutocompleteQuery, IEnumerable<Vacancy>>,
        IRequestHandler<SelectPagedClosedVacanciesByOrganizationQuery, Page<Vacancy>>,
        IRequestHandler<SelectPagedFavoriteVacanciesByResearcherQuery, Page<Vacancy>>,
        IRequestHandler<SelectPagedVacanciesByGuidsQuery, Page<Vacancy>>
    {
        private readonly IDatabase _db;

        public VacancyQueriesHandler(IDatabase db)
        {
            _db = db;
        }

        public Vacancy Handle(SingleVacancyQuery message)
        {
            if (message.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {message.VacancyGuid}");

            Vacancy vacancy = _db.SingleOrDefaultById<Vacancy>(message.VacancyGuid);

            return vacancy;
        }
        public Page<Vacancy> Handle(SelectPagedVacanciesQuery message)
        {
            Page<Vacancy> vacancies =
                message.PublishedOnly
                ?
                _db.Page<Vacancy>(message.PageIndex, message.PageSize, new Sql($"SELECT v.* FROM org_vacancies v WHERE v.status = @0 ORDER BY v.publish_date DESC", (int)VacancyStatus.Published))
                :
                _db.Page<Vacancy>(message.PageIndex, message.PageSize, new Sql($"SELECT v.* FROM org_vacancies v ORDER BY v.publish_date DESC"))
                ;

            return vacancies;
        }
        public Page<Vacancy> Handle(SelectPagedVacanciesByOrganizationQuery message)
        {
            if (message.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {message.OrganizationGuid}");

            Page<Vacancy> vacancies = _db.Page<Vacancy>(message.PageIndex, message.PageSize, new Sql($"SELECT v.* FROM org_vacancies v WHERE v.organization_guid = @0 ORDER BY v.guid DESC", message.OrganizationGuid));

            return vacancies;
        }
        public IEnumerable<Vacancy> Handle(SelectVacanciesForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<Vacancy> vacancies;
            if (message.Take != 0)
            {
                vacancies = _db.FetchBy<Vacancy>(f => f.Where(w => w.name.Contains(message.Query))).Take(message.Take);
            }
            else
            {
                vacancies = _db.FetchBy<Vacancy>(f => f.Where(w => w.name.Contains(message.Query)));
            }

            return vacancies;
        }
        public Page<Vacancy> Handle(SelectPagedClosedVacanciesByOrganizationQuery message)
        {
            if (message.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {message.OrganizationGuid}");

            Page<Vacancy> vacancies = _db.Page<Vacancy>(message.PageIndex, message.PageSize, new Sql($"SELECT v.* FROM org_vacancies v  WHERE v.organization_guid = @0 AND v.status = @1 ORDER BY v.guid DESC", message.OrganizationGuid, (int)VacancyStatus.Closed));

            return vacancies;
        }
        public Page<Vacancy> Handle(SelectPagedFavoriteVacanciesByResearcherQuery message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");

            Page<Vacancy> favoriteVacancies = _db.Page<Vacancy>(message.PageIndex, message.PageSize, new Sql($"SELECT fv.vacancy_guid FROM res_favoritevacancies fv WHERE fv.researcher_guid = @0 INNER JOIN org_vacancies v ON (fv.vacancy_guid = v.guid)", message.ResearcherGuid));

            return favoriteVacancies;
        }
        public Page<Vacancy> Handle(SelectPagedVacanciesByGuidsQuery message)
        {
            if (!message.VacancyGuids.Any()) throw new ArgumentNullException($"VacancyGuids doesn't contain any guid: {message.VacancyGuids}");

            var vacancies = _db.Page<Vacancy>(message.PageIndex, message.PageSize, new Sql($"SELECT v.* FROM org_vacancies v WHERE v.guid IN (@0)", message.VacancyGuids));

            return vacancies;
        }
    }
}