using SciVacancies.ReadModel.Core;
using SciVacancies.ReadModel;

using System;
using System.Collections.Generic;
using System.Linq;

using NPoco;
using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class SingleOrganizationQueryHandler : IRequestHandler<SingleOrganizationQuery, Organization>
    {
        private readonly IDatabase _db;

        public SingleOrganizationQueryHandler(IDatabase db)
        {
            _db = db;
        }

        public Organization Handle(SingleOrganizationQuery message)
        {
            if (message.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {message.OrganizationGuid}");

            Organization organization = _db.SingleOrDefaultById<Organization>(message.OrganizationGuid);

            return organization;
        }
    }
    public class SelectOrganizationsForAutocompleteQueryHandler : IRequestHandler<SelectOrganizationsForAutocompleteQuery, IEnumerable<Organization>>
    {
        private readonly IDatabase _db;
        private readonly IElasticService _elastic;

        public SelectOrganizationsForAutocompleteQueryHandler(IDatabase db, IElasticService elastic)
        {
            _db = db;
            _elastic = elastic;
        }

        public IEnumerable<Organization> Handle(SelectOrganizationsForAutocompleteQuery message)
        {
            //TODO - настройки эластика в конфиг, сделать фабрику.
            if (String.IsNullOrEmpty(message.Query)) throw new ArgumentNullException($"Query is empty: {message.Query}");

            IEnumerable<Organization> organizations;
            if (message.Take != 0)
            {
                organizations = _db.FetchBy<Organization>(f => f.Where(w => w.Name.Contains(message.Query))).Take(message.Take);
                //organizations = _elastic.Connect().Search<Organization>(s => s
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
                organizations = _db.FetchBy<Organization>(f => f.Where(w => w.Name.Contains(message.Query)));
                //organizations = _elastic.Connect().Search<Organization>(s => s
                //                                                    .Index("scivacancies")
                //                                                    .Query(q => q
                //                                                        .Match(m => m
                //                                                            .Query(message.Query)
                //                                                            .OnField(of => of.Name)
                //                                                        )
                //                                                    )
                //                                                    ).Documents;
            }

            return organizations;
        }
    }
    public class SelectPagedOrganizationsQueryHandler : IRequestHandler<SelectPagedOrganizationsQuery, Page<Organization>>
    {
        private readonly IDatabase _db;

        public SelectPagedOrganizationsQueryHandler(IDatabase db)
        {
            _db = db;
        }

        public Page<Organization> Handle(SelectPagedOrganizationsQuery message)
        {
            Page<Organization> organizations = _db.Page<Organization>(message.PageIndex, message.PageSize, new Sql("SELECT o.* FROM \"Organizations\" o ORDER BY o.\"Guid\" DESC"));

            return organizations;
        }
    }
}