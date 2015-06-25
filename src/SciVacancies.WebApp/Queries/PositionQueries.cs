using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;

using MediatR;
using NPoco;

namespace SciVacancies.WebApp.Queries
{
    public class SinglePositionQuery:IRequest<Position>
    {
        public Guid PositionGuid { get; set; }
    }
    public class SelectPagedPositionsQuery:IRequest<Page<Position>>
    {
        public Guid OrganizationGuid { get; set; }

        public long PageSize { get; set; }
        public long PageIndex { get; set; }
        public string OrderBy { get; set; }
        
        //TODO - добавить фильтр сортировки по колонкам
    }
}
