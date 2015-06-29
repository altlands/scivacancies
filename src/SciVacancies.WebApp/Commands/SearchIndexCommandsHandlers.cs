using Nest;
using MediatR;
using SciVacancies.ReadModel.ElasticSearchModel.Model;

namespace SciVacancies.WebApp.Commands
{
    public class CreateSearchIndexCommandHandler : RequestHandler<CreateSearchIndexCommand>
    {
        private readonly IElasticClient _elastic;
        public CreateSearchIndexCommandHandler(IElasticClient elastic)
        {
            _elastic = elastic;
        }
        protected override void HandleCore(CreateSearchIndexCommand message)
        {
            //TODO - имя индекса в конфиг
            _elastic.CreateIndex("scivacancies", c => c
                                .AddMapping<Vacancy>(am => am
                                    .MapFromAttributes()
                                )
                                .AddMapping<Organization>(am => am
                                    .MapFromAttributes()
                                )
                            );
        }
    }
    public class RemoveSearchIndexCommandHandler : RequestHandler<RemoveSearchIndexCommand>
    {
        private readonly IElasticClient _elastic;
        public RemoveSearchIndexCommandHandler(IElasticClient elastic)
        {
            _elastic = elastic;
        }
        protected override void HandleCore(RemoveSearchIndexCommand message)
        {
            //TODO - имя индекса в конфиг
            _elastic.DeleteIndex(s => s.Index("scivacancies"));
        }
    }
    public class RestoreSearchIndexFromReadModelCommandHandler : RequestHandler<RestoreSearchIndexFromReadModelCommand>
    {
        private readonly IElasticClient _elastic;
        public RestoreSearchIndexFromReadModelCommandHandler(IElasticClient elastic)
        {
            _elastic = elastic;
        }
        protected override void HandleCore(RestoreSearchIndexFromReadModelCommand message)
        {
            //TODO придумать)
        }
    }
}
