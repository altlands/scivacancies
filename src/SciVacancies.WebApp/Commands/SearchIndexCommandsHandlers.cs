using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Nest;
using MediatR;
using SciVacancies.ReadModel.Core;
using SciVacancies.ReadModel;

namespace SciVacancies.WebApp.Commands
{
    public class CreateSearchIndexCommandHandler : RequestHandler<CreateSearchIndexCommand>
    {
        private readonly IElasticService _elastic;
        public CreateSearchIndexCommandHandler(IElasticService elastic)
        {
            _elastic = elastic;
        }
        protected override void HandleCore(CreateSearchIndexCommand message)
        {
            _elastic.CreateIndex();
        }
    }
    public class RemoveSearchIndexCommandHandler : RequestHandler<RemoveSearchIndexCommand>
    {
        private readonly IElasticService _elastic;
        public RemoveSearchIndexCommandHandler(IElasticService elastic)
        {
            _elastic = elastic;
        }
        protected override void HandleCore(RemoveSearchIndexCommand message)
        {
            _elastic.RemoveIndex();
        }
    }
    public class RestoreSearchIndexFromReadModelCommandHandler : RequestHandler<RestoreSearchIndexFromReadModelCommand>
    {
        private readonly IElasticService _elastic;
        public RestoreSearchIndexFromReadModelCommandHandler(IElasticService elastic)
        {
            _elastic = elastic;
        }
        protected override void HandleCore(RestoreSearchIndexFromReadModelCommand message)
        {
            //TODO придумать)
        }
    }
    public class PutVacancyToSearchIndexCommandHandler : RequestHandler<PutVacancyToSearchIndexCommand>
    {
        private readonly IElasticService _elastic;
        public PutVacancyToSearchIndexCommandHandler(IElasticService elastic)
        {
            _elastic = elastic;
        }
        protected override void HandleCore(PutVacancyToSearchIndexCommand message)
        {
            _elastic.Connect().Index(message.Data);
        }
    }
    public class UpdateVacancyInSearchIndexCommandHandler : RequestHandler<UpdateVacancyInSearchIndexCommand>
    {
        private readonly IElasticService _elastic;
        public UpdateVacancyInSearchIndexCommandHandler(IElasticService elastic)
        {
            _elastic = elastic;
        }
        protected override void HandleCore(UpdateVacancyInSearchIndexCommand message)
        {
            _elastic.Connect().Update<Vacancy>(u => u.IdFrom(message.Data).Doc(message.Data));
        }
    }

}
