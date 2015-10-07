using System;
using NPoco;

namespace SciVacancies.ReadModel.Core
{
    public class BaseEntity
    {
        [Column]
        public Guid guid { get; set; }
    }
}
