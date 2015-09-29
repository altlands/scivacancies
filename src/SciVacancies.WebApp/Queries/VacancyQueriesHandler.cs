using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;

using NPoco;
using MediatR;
using SciVacancies.ReadModel;
using SciVacancies.WebApp.Engine;

namespace SciVacancies.WebApp.Queries
{
    public class VacancyQueriesHandler :
        IRequestHandler<SingleVacancyQuery, Vacancy>,
        IRequestHandler<CountVacanciesQuery, int>,
        IRequestHandler<SelectPagedVacanciesQuery, Page<Vacancy>>,
        IRequestHandler<SelectPagedVacanciesByOrganizationQuery, Page<Vacancy>>,
        IRequestHandler<SelectVacancyCriteriasQuery, IEnumerable<VacancyCriteria>>,
        IRequestHandler<SelectVacanciesForAutocompleteQuery, IEnumerable<Vacancy>>,
        IRequestHandler<SelectPagedClosedVacanciesByOrganizationQuery, Page<Vacancy>>,
        IRequestHandler<SelectPagedFavoriteVacanciesByResearcherQuery, Page<Vacancy>>,
        IRequestHandler<SelectFavoriteVacancyGuidsByResearcherQuery, IEnumerable<Guid>>,
        IRequestHandler<SelectPagedVacanciesByGuidsQuery, Page<Vacancy>>,
        IRequestHandler<SelectAllVacancyAttachmentsQuery, IEnumerable<VacancyAttachment>>,
        IRequestHandler<SelectAllExceptCommitteeVacancyAttachmentsQuery, IEnumerable<VacancyAttachment>>,
        IRequestHandler<SelectCommitteeVacancyAttachmentsQuery, IEnumerable<VacancyAttachment>>

    {
        private readonly IDatabase _db;

        public VacancyQueriesHandler(IDatabase db)
        {
            _db = db;
        }

        public Vacancy Handle(SingleVacancyQuery message)
        {
            if (message.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {message.VacancyGuid}");

            var vacancy = _db.SingleOrDefaultById<Vacancy>(message.VacancyGuid);
            return vacancy;
        }
        public Page<Vacancy> Handle(SelectPagedVacanciesQuery message)
        {
            var vacancies =
                message.PublishedOnly
                ?
                _db.Page<Vacancy>(message.PageIndex, message.PageSize, new Sql($"SELECT v.* FROM org_vacancies v WHERE v.status = @0 ORDER BY v.creation_date DESC", VacancyStatus.Published))
                :
                _db.Page<Vacancy>(message.PageIndex, message.PageSize, new Sql($"SELECT v.* FROM org_vacancies v WHERE v.status != @0 ORDER BY v.creation_date DESC", VacancyStatus.Removed))
                ;

            return vacancies;
        }
        public Page<Vacancy> Handle(SelectPagedVacanciesByOrganizationQuery message)
        {
            if (message.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {message.OrganizationGuid}");
            if (string.IsNullOrWhiteSpace(message.OrderDirection))
                message.OrderDirection = "DESC";
            if (string.IsNullOrWhiteSpace(message.OrderBy))
                message.OrderBy = nameof(Vacancy.creation_date);


            var vacancies = _db.Page<Vacancy>(message.PageIndex, message.PageSize, new Sql($"SELECT v.* FROM org_vacancies v WHERE v.organization_guid = @0 AND v.status != @1 ORDER BY v.{message.OrderBy} {message.OrderDirection.ToUpper()}", message.OrganizationGuid, VacancyStatus.Removed));

            return vacancies;
        }
        public IEnumerable<Vacancy> Handle(SelectVacanciesForAutocompleteQuery message)
        {
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            var vacancies = message.Take != 0
                ? _db.FetchBy<Vacancy>(f => f.Where(w => w.name.Contains(message.Query) && w.status != VacancyStatus.Removed)).Take(message.Take)
                : _db.FetchBy<Vacancy>(f => f.Where(w => w.name.Contains(message.Query) && w.status != VacancyStatus.Removed));

            return vacancies;
        }
        public Page<Vacancy> Handle(SelectPagedClosedVacanciesByOrganizationQuery message)
        {
            if (message.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {message.OrganizationGuid}");
            if (string.IsNullOrWhiteSpace(message.OrderDirection))
                message.OrderDirection = "DESC";
            if (string.IsNullOrWhiteSpace(message.OrderBy))
                message.OrderBy = nameof(Vacancy.creation_date);

            var vacancies = _db.Page<Vacancy>(message.PageIndex, message.PageSize, new Sql($"SELECT v.* FROM org_vacancies v  WHERE v.organization_guid = @0 AND v.status = @1 ORDER BY v.{message.OrderBy} {message.OrderDirection.ToUpper()}", message.OrganizationGuid, VacancyStatus.Closed));

            return vacancies;
        }
        public Page<Vacancy> Handle(SelectPagedFavoriteVacanciesByResearcherQuery message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");
            if (string.IsNullOrWhiteSpace(message.OrderDirection))
                message.OrderDirection = "DESC";
            if (string.IsNullOrWhiteSpace(message.OrderBy))
                message.OrderBy = nameof(Vacancy.creation_date);

            var favoriteVacancies = _db.Page<Vacancy>(message.PageIndex, message.PageSize, new Sql($"SELECT * FROM res_favoritevacancies fv, org_vacancies v WHERE fv.researcher_guid = @0 AND fv.vacancy_guid = v.guid ORDER BY v.{message.OrderBy} {message.OrderDirection.ToUpper()}", message.ResearcherGuid));

            return favoriteVacancies;
        }
        public IEnumerable<Guid> Handle(SelectFavoriteVacancyGuidsByResearcherQuery message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");

            IEnumerable<Guid> favoriteVacancyGuids = _db.Fetch<Guid>(new Sql($"SELECT fv.vacancy_guid FROM res_favoritevacancies fv WHERE fv.researcher_guid = @0", message.ResearcherGuid));

            return favoriteVacancyGuids;
        }
        public Page<Vacancy> Handle(SelectPagedVacanciesByGuidsQuery message)
        {
            if (!message.VacancyGuids.Any()) throw new ArgumentNullException($"VacancyGuids doesn't contain any guid: {message.VacancyGuids}");

            var vacancies = _db.Page<Vacancy>(message.PageIndex, message.PageSize, new Sql($"SELECT v.* FROM org_vacancies v WHERE v.guid IN (@0)", message.VacancyGuids));

            return vacancies;
        }

        public IEnumerable<VacancyCriteria> Handle(SelectVacancyCriteriasQuery msg)
        {
            IEnumerable<VacancyCriteria> vacancyCriterias = _db.Fetch<VacancyCriteria>(new Sql($"SELECT * FROM org_vacancycriterias ovc WHERE ovc.vacancy_guid = @0", msg.VacancyGuid));

            return vacancyCriterias;
        }
        public IEnumerable<VacancyAttachment> Handle(SelectAllVacancyAttachmentsQuery msg)
        {
            IEnumerable<VacancyAttachment> vaAttachments = _db.Fetch<VacancyAttachment>(new Sql($"SELECT * FROM org_vacancy_attachments ra WHERE ra.vacancy_guid = @0", msg.VacancyGuid));

            return vaAttachments;
        }
        public IEnumerable<VacancyAttachment> Handle(SelectAllExceptCommitteeVacancyAttachmentsQuery msg)
        {
            IEnumerable<VacancyAttachment> vaAttachments = _db.Fetch<VacancyAttachment>(new Sql($"SELECT * FROM org_vacancy_attachments ra WHERE ra.vacancy_guid = @0 AND ra.type_id = 3", msg.VacancyGuid));

            return vaAttachments;
        }
        public IEnumerable<VacancyAttachment> Handle(SelectCommitteeVacancyAttachmentsQuery msg)
        {
            IEnumerable<VacancyAttachment> vaAttachments = _db.Fetch<VacancyAttachment>(new Sql($"SELECT * FROM org_vacancy_attachments ra WHERE ra.vacancy_guid = @0 AND ra.type_id = 1", msg.VacancyGuid));

            return vaAttachments;
        }
        public int Handle(CountVacanciesQuery msg)
        {
            IEnumerable<Vacancy> vacancies = _db.Fetch<Vacancy>(new Sql("SELECT v.* FROM org_vacancies v "));

            return vacancies.Count();
        }
    }
}