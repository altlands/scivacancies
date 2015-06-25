﻿using SciVacancies.ReadModel.Core;
using SciVacancies.ReadModel;

using System;
using System.Threading.Tasks;
using NPoco;
using MediatR;

namespace SciVacancies.WebApp.Queries
{
    public class SinglePositionQueryHandler : IRequestHandler<SinglePositionQuery, Position>
    {
        private readonly IDatabase _db;

        public SinglePositionQueryHandler(IDatabase db)
        {
            _db = db;
        }

        public Position Handle(SinglePositionQuery message)
        {
            if (message.PositionGuid == Guid.Empty) throw new ArgumentNullException($"PositionGuid is empty: {message.PositionGuid}");

            Position position = _db.SingleOrDefaultById<Position>(message.PositionGuid);

            return position;
        }
    }
    public class SelectPagedPositionsQueryHandler : IRequestHandler<SelectPagedPositionsQuery, Page<Position>>
    {
        private readonly IDatabase _db;

        public SelectPagedPositionsQueryHandler(IDatabase db)
        {
            _db = db;
        }

        public Page<Position> Handle(SelectPagedPositionsQuery message)
        {
            if (message.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {message.OrganizationGuid}");

            Page<Position> positions = _db.Page<Position>(message.PageIndex, message.PageSize, new Sql("SELECT p.* FROM \"Positions\" p WHERE p.\"OrganizationGuid\"=" + message.OrganizationGuid + " ORDER BY p.\"Guid\" DESC"));

            return positions;
        }
    }
}