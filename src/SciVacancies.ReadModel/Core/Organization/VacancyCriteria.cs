using System;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("org_vacancycriterias")]
    [PrimaryKey("guid", AutoIncrement = false)]
    public class VacancyCriteria : BaseEntity
    {
        public int criteria_id { get; set; }
        public Guid vacancy_guid { get; set; }

        public long? from { get; set; }
        public long? to { get; set; }
    }
}
