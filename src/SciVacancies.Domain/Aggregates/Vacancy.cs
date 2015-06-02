using System;
using CommonDomain.Core;
using SciVacancies.Domain.Events;

namespace SciVacancies.Domain.Aggregates
{
    //public class Vacancy : AggregateBase
    //{
    //    #region Fields
    //    private Guid OrganizationId { get; set; }
    //    #endregion
    //    public Vacancy()
    //    {

    //    }
    //    public Vacancy(Guid id, Guid organizationId)
    //    {
    //        RaiseEvent(new VacancyCreated()
    //        {
    //            Id = Guid.NewGuid(),
    //            TimeStamp = DateTime.UtcNow,
    //            VacancyId = id,
    //            OrganizationId = organizationId
    //        });
    //    }
    //    public void Delete()
    //    {
    //        RaiseEvent(new VacancyRemoved()
    //        {

    //        });
    //    }
    //    #region Apply-Handlers
    //    public void Apply(VacancyCreated @event)
    //    {
    //        this.Id = @event.VacancyId;
    //        this.OrganizationId = @event.OrganizationId;
    //    }
    //    public void Apply(VacancyRemoved @event)
    //    {

    //    }
    //    #endregion
    //}
}
