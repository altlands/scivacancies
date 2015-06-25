using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;

using System;

using NPoco;
using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class SingleVacancyApplicationQueryHandler : IRequestHandler<SingleVacancyApplicationQuery, VacancyApplication>
    {
        private readonly IDatabase _db;

        public SingleVacancyApplicationQueryHandler(IDatabase db)
        {
            _db = db;
        }

        public VacancyApplication Handle(SingleVacancyApplicationQuery message)
        {
            if (message.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {message.VacancyApplicationGuid}");

            //VacancyApplication vacancyApplication = _db.SingleOrDefaultById<VacancyApplication>(message.VacancyApplicationGuid);
            VacancyApplication vacancyApplication = _db.SingleOrDefault<VacancyApplication>(new Sql("SELECT va.* FROM \"VacancyApplications\" va WHERE va.\"Guid\"=" + message.VacancyApplicationGuid + " AND va.\"Status\"!=" + VacancyApplicationStatus.Removed));

            return vacancyApplication;
        }
    }
    public class SelectPagedVacancyApplicationsByResearcherQueryHandler : IRequestHandler<SelectPagedVacancyApplicationsByResearcherQuery, Page<VacancyApplication>>
    {
        private readonly IDatabase _db;

        public SelectPagedVacancyApplicationsByResearcherQueryHandler(IDatabase db)
        {
            _db = db;
        }

        public Page<VacancyApplication> Handle(SelectPagedVacancyApplicationsByResearcherQuery message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");

            Page<VacancyApplication> vacancyApplications = _db.Page<VacancyApplication>(message.PageIndex, message.PageSize, new Sql("SELECT va.* FROM \"VacancyApplications\" va WHERE va.\"Status\"!=" + VacancyApplicationStatus.Removed + " ORDER BY va.\"Guid\" DESC"));

            return vacancyApplications;
        }
    }
    public class SelectPagedVacancyApplicationsByVacancyQueryHandler : IRequestHandler<SelectPagedVacancyApplicationsByVacancyQuery, Page<VacancyApplication>>
    {
        private readonly IDatabase _db;

        public SelectPagedVacancyApplicationsByVacancyQueryHandler(IDatabase db)
        {
            _db = db;
        }

        public Page<VacancyApplication> Handle(SelectPagedVacancyApplicationsByVacancyQuery message)
        {
            if (message.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {message.VacancyGuid}");

            Page<VacancyApplication> vacancyApplications = _db.Page<VacancyApplication>(message.PageIndex, message.PageSize, new Sql("SELECT va.* FROM \"VacancyApplications\" va WHERE va.\"Status\"!=" + VacancyApplicationStatus.Removed + " ORDER BY va.\"Guid\" DESC"));

            return vacancyApplications;
        }
    }
}