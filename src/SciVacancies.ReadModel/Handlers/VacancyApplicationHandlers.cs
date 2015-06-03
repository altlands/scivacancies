using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Handlers
{
    public class VacancyApplicationCreatedHandler : EventBaseHandler<VacancyApplicationCreated>
    {
        public VacancyApplicationCreatedHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyApplicationCreated msg)
        {
            VacancyApplication vacancyApplication = new VacancyApplication()
            {
                Guid = msg.VacancyApplicationGuid,
                VacancyGuid = msg.VacancyGuid,
                ResearcherGuid =msg.ResearcherGuid,
                CreationdDate = msg.TimeStamp,
                Status =  VacancyApplicationStatus.InProcess
            };

            _db.Insert(vacancyApplication);
        }
    }
    public class VacancyApplicationAppliedHandler : EventBaseHandler<VacancyApplicationApplied>
    {
        public VacancyApplicationAppliedHandler(IDatabase db) : base(db) { }
        public override void Handle(VacancyApplicationApplied msg)
        {
            VacancyApplication vacancyApplication = _db.SingleById<VacancyApplication>(msg.VacancyApplicationGuid);
            vacancyApplication.Status = VacancyApplicationStatus.Applied;
            vacancyApplication.UpdateDate = msg.TimeStamp;

            _db.Update(vacancyApplication);
        }
    }
}