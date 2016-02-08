using Nest;
using MediatR;
using SciVacancies.ReadModel.ElasticSearchModel.Model;
using Microsoft.Extensions.OptionsModel;

namespace SciVacancies.WebApp.Commands
{
    public class CreateSearchIndexCommandHandler : RequestHandler<CreateSearchIndexCommand>
    {
        private readonly IElasticClient _elastic;
        private readonly IOptions<ElasticSettings> _elasticSettings;

        public CreateSearchIndexCommandHandler(IElasticClient elastic, IOptions<ElasticSettings> elasticSettings)
        {
            _elastic = elastic;
            _elasticSettings = elasticSettings;
        }
        protected override void HandleCore(CreateSearchIndexCommand message)
        {
         _elastic.CreateIndex(_elasticSettings.Value.DefaultIndex, c => c
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
        private readonly IOptions<ElasticSettings> _elasticSettings;

        public RemoveSearchIndexCommandHandler(IElasticClient elastic, IOptions<ElasticSettings> elasticSettings)
        {
            _elastic = elastic;
            _elasticSettings = elasticSettings;
        }
        protected override void HandleCore(RemoveSearchIndexCommand message)
        {
            try
            {
                _elastic.DeleteIndex(s => s.Index(_elasticSettings.Value.DefaultIndex));
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
    public class RestoreSearchIndexFromReadModelCommandHandler : RequestHandler<RestoreSearchIndexFromReadModelCommand>
    {
        private readonly IElasticClient _elastic;
        private readonly IOptions<ElasticSettings> _elasticSettings;

        public RestoreSearchIndexFromReadModelCommandHandler(IElasticClient elastic, IOptions<ElasticSettings> elasticSettings)
        {
            _elastic = elastic;
            _elasticSettings = elasticSettings;
        }
        protected override void HandleCore(RestoreSearchIndexFromReadModelCommand message)
        {
            //TODO придумать)
        }
    }
}
