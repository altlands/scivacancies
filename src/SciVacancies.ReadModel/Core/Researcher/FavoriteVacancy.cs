using System;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("favoritevacancies")]
    [PrimaryKey("guid", AutoIncrement = false)]
    public class FavoriteVacancy : BaseEntity
    {
        public Guid vacancy_guid { get; set; }
        public Guid researcher_guid { get; set; }

        public DateTime creation_date { get; set; }
    }
}
