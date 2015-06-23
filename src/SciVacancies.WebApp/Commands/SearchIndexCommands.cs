using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

namespace SciVacancies.WebApp.Commands
{
    public class CreateSearchIndexCommand : CommandBase, IRequest
    {

    }
    public class RemoveSearchIndexCommand : CommandBase, IRequest
    {

    }
    public class RestoreSearchIndexFromReadModelCommand : CommandBase, IRequest
    {

    }
    public class PutVacancyToSearchIndexCommand : CommandBase, IRequest
    {
        public Vacancy Data { get; set; }
    }
    public class UpdateVacancyInSearchIndexCommand : CommandBase, IRequest
    {
        public Vacancy Data { get; set; }
    }
}
