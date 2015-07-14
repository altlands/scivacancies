using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;

using NPoco;
using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class VacancyApplicationQueriesHandler :
        IRequestHandler<SingleVacancyApplicationQuery, VacancyApplication>,
        IRequestHandler<SelectPagedVacancyApplicationsByResearcherQuery, Page<VacancyApplication>>,
        IRequestHandler<SelectPagedVacancyApplicationsByVacancyQuery, Page<VacancyApplication>>
    {
        private readonly IDatabase _db;

        public VacancyApplicationQueriesHandler(IDatabase db)
        {
            _db = db;
        }

        public VacancyApplication Handle(SingleVacancyApplicationQuery message)
        {
            if (message.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {message.VacancyApplicationGuid}");

            VacancyApplication vacancyApplication = _db.SingleOrDefault<VacancyApplication>(new Sql($"SELECT va.* FROM res_vacancyapplications va WHERE va.guid = @0 AND va.status != @1", message.VacancyApplicationGuid, (int)VacancyApplicationStatus.Removed));

            return vacancyApplication;
        }
        public Page<VacancyApplication> Handle(SelectPagedVacancyApplicationsByResearcherQuery message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");

            Page<VacancyApplication> vacancyApplications = _db.Page<VacancyApplication>(message.PageIndex, message.PageSize, new Sql($"SELECT va.* FROM res_vacancyapplications va WHERE va.status != @0 ORDER BY va.guid DESC", (int)VacancyApplicationStatus.Removed));

            return vacancyApplications;
        }
        public Page<VacancyApplication> Handle(SelectPagedVacancyApplicationsByVacancyQuery message)
        {
            if (message.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {message.VacancyGuid}");

            Page<VacancyApplication> vacancyApplications = _db.Page<VacancyApplication>(message.PageIndex, message.PageSize, new Sql("SELECT va.* FROM res_vacancyapplications va WHERE va.status != @0 ORDER BY va.guid DESC", (int)VacancyApplicationStatus.Removed));

            return vacancyApplications;
        }

        public IEnumerable<VacancyApplicationAttachment> Handle(SelectVacancyApplicationAttachmentsQuery msg)
        {
            IEnumerable<VacancyApplicationAttachment> vaAttachments = _db.Fetch<VacancyApplicationAttachment>(new Sql($"SELECT * FROM res_vacancyapplication_attachments ra WHERE ra.vacancyapplication_guid = @0", msg.VacancyApplicationGuid));

            return vaAttachments;
        }
    }
}