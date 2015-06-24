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

            VacancyApplication vacancyApplication = _db.SingleOrDefaultById<VacancyApplication>(message.VacancyApplicationGuid);

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

            Page<VacancyApplication> vacancyApplications = _db.Page<VacancyApplication>(message.PageIndex, message.PageSize, new Sql("SELECT va.* FROM \"VacancyApplications\" va ORDER BY va.\"Guid\" DESC"));

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

            Page<VacancyApplication> vacancyApplications = _db.Page<VacancyApplication>(message.PageIndex, message.PageSize, new Sql("SELECT va.* FROM \"VacancyApplications\" va ORDER BY va.\"Guid\" DESC"));

            return vacancyApplications;
        }
    }
}




//public class SelectPagedOrganizationsQueryHandler : IRequestHandler<SelectPagedOrganizationsQuery, Page<Organization>>
//{
//    private readonly IDatabase _db;

//    public SelectPagedOrganizationsQueryHandler(IDatabase db)
//    {
//        _db = db;
//    }

//    public Page<Organization> Handle(SelectPagedOrganizationsQuery message)
//    {
//        Page<Organization> organizations = _db.Page<Organization>(message.PageIndex, message.PageSize, new Sql("SELECT o.* FROM \"Organizations\" o ORDER BY o.\"Guid\" DESC"));

//        return organizations;
//    }
//}