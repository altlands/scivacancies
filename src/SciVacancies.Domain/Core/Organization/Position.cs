using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;

using System;

namespace SciVacancies.Domain.Core
{
    [Obsolete("Will be removed")]
    public class Position
    {
        /// <summary>
        /// Guid позиции
        /// </summary>
        public Guid PositionGuid { get; set; }

        /// <summary>
        /// Вся информация о позиции
        /// </summary>
        public PositionDataModel Data { get; set; }

        /// <summary>
        /// Статус позиции
        /// </summary>
        public PositionStatus Status { get; set; }
    }
}
