﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Core
{
    [TableName("FavoriteVacancies")]
    [PrimaryKey("Guid", AutoIncrement = false)]
    public class FavoriteVacancy:BaseEntity
    {
        public Guid VacancyGuid { get; set; }
        public Guid ResearcherGuid { get; set; }

        public DateTime CreationdDate { get; set; }
    }
}