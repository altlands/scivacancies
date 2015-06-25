using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class SearchQuery : IRequest<string>
    {
        public string Query { get; set; }

        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string OrderBy { get; set; }


    }
}
