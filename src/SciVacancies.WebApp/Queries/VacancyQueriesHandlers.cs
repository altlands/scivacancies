using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;
using SciVacancies.ReadModel;

using System;
using System.Collections.Generic;
using System.Linq;

using NPoco;
using MediatR;
using Nest;

namespace SciVacancies.WebApp.Queries
{
    public class SingleVacancyQueryHandler : IRequestHandler<SingleVacancyQuery, Vacancy>
    {
        private readonly IDatabase _db;

        public SingleVacancyQueryHandler(IDatabase db)
        {
            _db = db;
        }

        public Vacancy Handle(SingleVacancyQuery message)
        {
            if (message.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {message.VacancyGuid}");

            Vacancy vacancy = _db.SingleOrDefaultById<Vacancy>(message.VacancyGuid);

            return vacancy;
        }
    }
    public class SelectPagedVacanciesQueryHandler : IRequestHandler<SelectPagedVacanciesQuery, Page<Vacancy>>
    {
        private readonly IDatabase _db;

        public SelectPagedVacanciesQueryHandler(IDatabase db)
        {
            _db = db;
        }

        public Page<Vacancy> Handle(SelectPagedVacanciesQuery message)
        {

            Page<Vacancy> vacancies =
                message.PublishedOnly
                ?
                _db.Page<Vacancy>(message.PageIndex, message.PageSize, new Sql($"SELECT v.* FROM \"Vacancies\" v WHERE v.\"Status\"={(int)VacancyStatus.Published} OR v.\"Status\"={(int)VacancyStatus.AppliesAcceptance} ORDER BY v.\"DateStart\" DESC"))
                :
                _db.Page<Vacancy>(message.PageIndex, message.PageSize, new Sql($"SELECT v.* FROM \"Vacancies\" v WHERE ORDER BY v.\"DateStart\" DESC"))
                ;

            return vacancies;
        }
    }
    public class SelectPagedVacanciesByOrganizationQueryHandler : IRequestHandler<SelectPagedVacanciesByOrganizationQuery, Page<Vacancy>>
    {
        private readonly IDatabase _db;

        public SelectPagedVacanciesByOrganizationQueryHandler(IDatabase db)
        {
            _db = db;
        }

        public Page<Vacancy> Handle(SelectPagedVacanciesByOrganizationQuery message)
        {
            if (message.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {message.OrganizationGuid}");

            Page<Vacancy> vacancies = _db.Page<Vacancy>(message.PageIndex, message.PageSize, new Sql($"SELECT v.* FROM \"Vacancies\" v  WHERE v.\"OrganizationGuid\"='{message.OrganizationGuid}' ORDER BY v.\"Guid\" DESC"));

            return vacancies;
        }
    }
    public class SelectVacanciesForAutocompleteQueryHandler : IRequestHandler<SelectVacanciesForAutocompleteQuery, IEnumerable<Vacancy>>
    {
        private readonly IDatabase _db;
        private readonly IElasticClient _elastic;

        public SelectVacanciesForAutocompleteQueryHandler(IDatabase db, IElasticClient elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<Vacancy> Handle(SelectVacanciesForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<Vacancy> vacancies;
            if (message.Take != 0)
            {
                vacancies = _db.FetchBy<Vacancy>(f => f.Where(w => w.Name.Contains(message.Query))).Take(message.Take);
                //vacancies = _elastic.Search<Vacancy>(s => s
                //                                                    .Index("scivacancies")
                //                                                    .Take(message.Take)
                //                                                    .Query(q => q
                //                                                        .Match(m => m
                //                                                            .Query(message.Query)
                //                                                            .OnField(of => of.Name)
                //                                                        )
                //                                                    )
                //                                                    ).Documents;
            }
            else
            {
                vacancies = _db.FetchBy<Vacancy>(f => f.Where(w => w.Name.Contains(message.Query)));
                //vacancies = _elastic.Search<Vacancy>(s => s
                //                                                    .Index("scivacancies")
                //                                                    .Query(q => q
                //                                                        .Match(m => m
                //                                                            .Query(message.Query)
                //                                                            .OnField(of => of.Name)
                //                                                        )
                //                                                    )
                //                                                    ).Documents;
            }

            return vacancies;
        }
    }
    public class SelectPagedClosedVacanciesByOrganizationQueryHandler : IRequestHandler<SelectPagedClosedVacanciesByOrganizationQuery, Page<Vacancy>>
    {
        private readonly IDatabase _db;

        public SelectPagedClosedVacanciesByOrganizationQueryHandler(IDatabase db)
        {
            _db = db;
        }

        public Page<Vacancy> Handle(SelectPagedClosedVacanciesByOrganizationQuery message)
        {
            if (message.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {message.OrganizationGuid}");

            Page<Vacancy> vacancies = _db.Page<Vacancy>(message.PageIndex, message.PageSize, new Sql($"SELECT v.* FROM \"Vacancies\" v  WHERE v.\"OrganizationGuid\"='{message.OrganizationGuid}' AND v.\"Status\"={(int)VacancyStatus.Closed } ORDER BY v.\"Guid\" DESC"));

            return vacancies;
        }
    }
    public class SelectPagedFavoriteVacanciesByResearcherQueryHandler : IRequestHandler<SelectPagedFavoriteVacanciesByResearcherQuery, Page<Vacancy>>
    {
        private readonly IDatabase _db;

        public SelectPagedFavoriteVacanciesByResearcherQueryHandler(IDatabase db)
        {
            _db = db;
        }

        public Page<Vacancy> Handle(SelectPagedFavoriteVacanciesByResearcherQuery message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");

            IList<Guid> favoriteVacanciesGuids = _db.FetchBy<FavoriteVacancy>(f => f.Where(w => w.ResearcherGuid == message.ResearcherGuid)).Select(s => s.VacancyGuid).ToList();

            if (favoriteVacanciesGuids.Any())
            {
                var sql = new SqlBuilder().AddTemplate("SELECT v.* FROM \"Vacancies\" v WHERE v.\"Guid\" IN (@items)", new { items = favoriteVacanciesGuids });
                var vacancies = _db.Page<Vacancy>(message.PageIndex, message.PageSize, sql);
                return vacancies;
            }
            return null;
        }
    }
    public class SelectPagedVacanciesByGuidsQueryHandler : IRequestHandler<SelectPagedVacanciesByGuidsQuery, Page<Vacancy>>
    {
        private readonly IDatabase _db;

        public SelectPagedVacanciesByGuidsQueryHandler(IDatabase db)
        {
            _db = db;
        }

        public Page<Vacancy> Handle(SelectPagedVacanciesByGuidsQuery message)
        {
            if (!message.VacanciesGuids.Any()) throw new ArgumentNullException($"VacanciesGuids doesn't contain any guid: {message.VacanciesGuids}");

            var sql = new SqlBuilder().AddTemplate("SELECT v.* FROM \"Vacancies\" v WHERE v.\"Guid\" IN (@items)", new { items = message.VacanciesGuids });
            var vacancies = _db.Page<Vacancy>(message.PageIndex, message.PageSize, sql);
            return vacancies;
        }
    }
}