using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using NPoco;
using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class VacancyApplicationQueriesHandler :
        IRequestHandler<SingleVacancyApplicationQuery, VacancyApplication>,
        IRequestHandler<SelectPagedVacancyApplicationsByResearcherQuery, Page<VacancyApplication>>,
        IRequestHandler<SelectVacancyApplicationsByResearcherQuery, IEnumerable<VacancyApplication>>,
        IRequestHandler<SelectPagedVacancyApplicationsByVacancyQuery, Page<VacancyApplication>>,
        IRequestHandler<SelectAllVacancyApplicationAttachmentsQuery, IEnumerable<VacancyApplicationAttachment>>,
        IRequestHandler<CountVacancyApplicationInVacancyQuery, int>,
        IRequestHandler<SelectVacancyApplicationInVacancyByStatusesQuery, IEnumerable<VacancyApplication>>,
        IRequestHandler<SingleVacancyApplicationAttachmentByPathGuidQuery, VacancyApplicationAttachment>
    {
        private readonly IDatabase _db;

        public VacancyApplicationQueriesHandler(IDatabase db)
        {
            _db = db;
        }

        public VacancyApplication Handle(SingleVacancyApplicationQuery message)
        {
            if (message.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {message.VacancyApplicationGuid}");

            var vacancyApplication = _db.SingleOrDefault<VacancyApplication>(new Sql($"SELECT va.* FROM res_vacancyapplications va WHERE va.guid = @0 AND va.status != @1", message.VacancyApplicationGuid, VacancyApplicationStatus.Removed));

            return vacancyApplication;
        }
        public Page<VacancyApplication> Handle(SelectPagedVacancyApplicationsByResearcherQuery message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");
            if (string.IsNullOrWhiteSpace(message.OrderDirection))
                message.OrderDirection = "DESC";
            if (string.IsNullOrWhiteSpace(message.OrderBy))
                message.OrderBy = nameof(VacancyApplication.creation_date);

            var vacancyApplications = _db.Page<VacancyApplication>(message.PageIndex, message.PageSize, new Sql($"SELECT va.* FROM res_vacancyapplications va WHERE va.status != @0 AND va.researcher_guid=@1  ORDER BY va.{message.OrderBy} {message.OrderDirection.ToUpper()}", VacancyApplicationStatus.Removed, message.ResearcherGuid));

            return vacancyApplications;
        }
        public IEnumerable<VacancyApplication> Handle(SelectVacancyApplicationsByResearcherQuery message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");
            var vacancyApplications = _db.Fetch<VacancyApplication>(new Sql($"SELECT va.* FROM res_vacancyapplications va WHERE va.status != @0 AND va.researcher_guid=@1", VacancyApplicationStatus.Removed, message.ResearcherGuid));

            return vacancyApplications;
        }
        public Page<VacancyApplication> Handle(SelectPagedVacancyApplicationsByVacancyQuery message)
        {
            if (message.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {message.VacancyGuid}");
            if (string.IsNullOrWhiteSpace(message.OrderDirection))
                message.OrderDirection = "DESC";
            if (string.IsNullOrWhiteSpace(message.OrderBy))
                message.OrderBy = nameof(VacancyApplication.apply_date);

            var vacancyApplications = _db.Page<VacancyApplication>(message.PageIndex, message.PageSize, new Sql($"SELECT va.* FROM res_vacancyapplications va WHERE va.status != @0 AND va.vacancy_guid = @1 ORDER BY va.{message.OrderBy} {message.OrderDirection.ToUpper()}", VacancyApplicationStatus.Removed, message.VacancyGuid));

            return vacancyApplications;
        }

        public IEnumerable<VacancyApplicationAttachment> Handle(SelectAllVacancyApplicationAttachmentsQuery message)
        {
            IEnumerable<VacancyApplicationAttachment> vaAttachments = _db.Fetch<VacancyApplicationAttachment>(new Sql($"SELECT * FROM res_vacancyapplication_attachments ra WHERE ra.vacancyapplication_guid = @0", message.VacancyApplicationGuid));

            return vaAttachments;
        }
        public int Handle(CountVacancyApplicationInVacancyQuery message)
        {
            if (message.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"{nameof(message.VacancyGuid)} is empty");

            var vacancyApplication = _db.Fetch<VacancyApplication>(new Sql("SELECT * FROM res_vacancyapplications va WHERE va.vacancy_guid = @0 AND va.status = @1", message.VacancyGuid, (int)message.Status));

            if (vacancyApplication != null && vacancyApplication.Count > 0)
            {
                return vacancyApplication.Count;
            }
            return 0;
        }

        public IEnumerable<VacancyApplication> Handle(SelectVacancyApplicationInVacancyByStatusesQuery message)
        {
            if (message.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"{nameof(message.VacancyGuid)} is empty");

            var vacancyApplications = _db.Fetch<VacancyApplication>(new Sql("SELECT * FROM res_vacancyapplications va WHERE va.vacancy_guid = @0 AND va.status in (@1)", message.VacancyGuid, message.Statuses.Select(c => (int)c).ToList()));

            return vacancyApplications;
        }


        public VacancyApplicationAttachment Handle(SingleVacancyApplicationAttachmentByPathGuidQuery msg)
        {
            VacancyApplicationAttachment vacancyApplicatioAttachment = _db.FirstOrDefault<VacancyApplicationAttachment>(new Sql($"SELECT * FROM res_vacancyapplication_attachments ra WHERE ra.url LIKE @0 and ra.name LIKE @1", "%" + msg.UrlPath + "%", "%" + msg.FileName + "%"));

            return vacancyApplicatioAttachment;
        }
    }
}