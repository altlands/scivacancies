using MediatR;
using Nest;
using SciVacancies.Domain.Events;

namespace SciVacancies.ReadModel.ElasticSearchModel.EventHandlers
{
    //public class VacancyPublishedHandler : INotificationHandler<VacancyPublished>
    //{
    //    private readonly IElasticClient _elasticClient;

    //    public VacancyPublishedHandler(IElasticClient elasticClient)
    //    {
    //        _elasticClient = elasticClient;
    //    }
    //    public void Handle(VacancyPublished msg)
    //    {
    //        //Vacancy vacancy = new Vacancy()
    //        //{
    //        //    Guid = msg.VacancyGuid,
    //        //    PositionGuid = msg.PositionGuid,
    //        //    OrganizationGuid = msg.OrganizationGuid,
    //        //    Name = msg.Data.Name,

    //        //    //TODO - Clean Up
    //        //    PositionTypeGuid = msg.Data.PositionTypeGuid,
    //        //    ResearchDirectionId = msg.Data.ResearchDirectionId,
    //        //    ResearchThemeId = msg.Data.ResearchThemeId,



    //        //    FullName = msg.Data.FullName,
    //        //    ResearchDirection = msg.Data.ResearchDirection,
    //        //    ResearchTheme = msg.Data.ResearchTheme,
    //        //    Tasks = msg.Data.Tasks,
    //        //    Criteria = msg.Data.Criteria,
    //        //    SalaryFrom = msg.Data.SalaryFrom,
    //        //    SalaryTo = msg.Data.SalaryTo,
    //        //    Bonuses = msg.Data.Bonuses,
    //        //    ContractType = msg.Data.ContractType,
    //        //    ContractTime = msg.Data.ContractTime,
    //        //    SocialPackage = msg.Data.SocialPackage,
    //        //    Rent = msg.Data.Rent,
    //        //    OfficeAccomodation = msg.Data.OfficeAccomodation,
    //        //    TransportCompensation = msg.Data.TransportCompensation,
    //        //    Region = msg.Data.Region,
    //        //    RegionId = msg.Data.RegionId,
    //        //    CityName = msg.Data.CityName,
    //        //    Details = msg.Data.Details,
    //        //    ContactName = msg.Data.ContactName,
    //        //    ContactEmail = msg.Data.ContactEmail,
    //        //    ContactPhone = msg.Data.ContactPhone,
    //        //    ContactDetails = msg.Data.ContactDetails,
    //        //    DateStart = msg.Data.DateStart,
    //        //    DateStartAcceptance = msg.Data.DateStartAcceptance,
    //        //    DateFinish = msg.Data.DateFinish
    //        //};

    //        //_elastic.Index(vacancy);
    //    }
    //}
    //public class VacancyAcceptApplicationsHandler : INotificationHandler<VacancyAcceptApplications>
    //{
    //    private readonly IElasticClient _elasticClient;

    //    public VacancyAcceptApplicationsHandler(IElasticClient elasticClient)
    //    {
    //        _elasticClient = elasticClient;
    //    }
    //    public void Handle(VacancyAcceptApplications msg)
    //    {
    //        //Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
    //        //vacancy.Status = VacancyStatus.AppliesAcceptance;
    //        //_db.Update(vacancy);

    //        //_elastic.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
    //    }
    //}
    //public class VacancyInCommitteeHandler : INotificationHandler<VacancyInCommittee>
    //{
    //    private readonly IElasticClient _elasticClient;

    //    public VacancyInCommitteeHandler(IElasticClient elasticClient)
    //    {
    //        _elasticClient = elasticClient;
    //    }
    //    public void Handle(VacancyInCommittee msg)
    //    {
    //        //Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
    //        //vacancy.Status = VacancyStatus.InCommittee;

    //        //_db.Update(vacancy);

    //        //_elastic.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
    //    }
    //}
    //public class VacancyClosedHandler : INotificationHandler<VacancyClosed>
    //{
    //    private readonly IElasticClient _elasticClient;

    //    public VacancyClosedHandler(IElasticClient elasticClient)
    //    {
    //        _elasticClient = elasticClient;
    //    }
    //    public void Handle(VacancyClosed msg)
    //    {
    //        //Position position = _db.SingleById<Position>(msg.PositionGuid);
    //        //position.Status = PositionStatus.InProcess;

    //        //_db.Update(position);

    //        //Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
    //        //vacancy.Status = VacancyStatus.Closed;

    //        //_db.Update(vacancy);

    //        //_elastic.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
    //    }
    //}
    //public class VacancyCancelledHandler : INotificationHandler<VacancyCancelled>
    //{
    //    private readonly IElasticClient _elasticClient;

    //    public VacancyCancelledHandler(IElasticClient elasticClient)
    //    {
    //        _elasticClient = elasticClient;
    //    }
    //    public void Handle(VacancyCancelled msg)
    //    {

    //        //Vacancy vacancy = _db.SingleById<Vacancy>(msg.VacancyGuid);
    //        //vacancy.Status = VacancyStatus.Cancelled;

    //        //_db.Update(vacancy);

    //        //_elastic.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
    //    }
    //}
}
