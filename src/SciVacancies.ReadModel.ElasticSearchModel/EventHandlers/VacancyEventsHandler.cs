using SciVacancies.Domain.Events;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.ElasticSearchModel.Model;

using MediatR;
using Nest;
using AutoMapper;

namespace SciVacancies.ReadModel.ElasticSearchModel.EventHandlers
{
    public class VacancyEventsHandler :
        INotificationHandler<VacancyCreated>,
        INotificationHandler<VacancyUpdated>,
        INotificationHandler<VacancyRemoved>,
        INotificationHandler<VacancyPublished>,
        INotificationHandler<VacancyInCommittee>,
        INotificationHandler<VacancyPretenderSet>,
        INotificationHandler<VacancyClosed>,
        INotificationHandler<VacancyCancelled>
    {
        private readonly IElasticClient _elasticClient;

        public VacancyEventsHandler(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public void Handle(VacancyCreated msg)
        {
            Vacancy vacancy = Mapper.Map<Vacancy>(msg);

            _elasticClient.Index(vacancy);
        }
        public void Handle(VacancyUpdated msg)
        {
            Vacancy vacancy = Mapper.Map<Vacancy>(msg);

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }
        public void Handle(VacancyRemoved msg)
        {
            _elasticClient.Delete<Vacancy>(msg.VacancyGuid.ToString());
        }
        public void Handle(VacancyPublished msg)
        {
            Vacancy vacancy = _elasticClient.Get<Vacancy>(msg.VacancyGuid.ToString()).Source;
            vacancy.PublishDate = msg.TimeStamp;
            vacancy.Status = VacancyStatus.Published;
            vacancy.PublishDate = msg.TimeStamp;

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }
        public void Handle(VacancyInCommittee msg)
        {
            Vacancy vacancy = _elasticClient.Get<Vacancy>(msg.VacancyGuid.ToString()).Source;
            vacancy.CommitteeDate = msg.TimeStamp;
            vacancy.Status = VacancyStatus.InCommittee;

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }
        public void Handle(VacancyPretenderSet msg)
        {
            Vacancy vacancy = _elasticClient.Get<Vacancy>(msg.VacancyGuid.ToString()).Source;
            vacancy.AwaitingDate = msg.TimeStamp;
            vacancy.Status = VacancyStatus.OfferResponseAwaiting;

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }
        public void Handle(VacancyClosed msg)
        {
            Vacancy vacancy = _elasticClient.Get<Vacancy>(msg.VacancyGuid.ToString()).Source;
            vacancy.AnnouncementDate = msg.TimeStamp;
            vacancy.Status = VacancyStatus.Closed;

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }
        public void Handle(VacancyCancelled msg)
        {
            Vacancy vacancy = _elasticClient.Get<Vacancy>(msg.VacancyGuid.ToString()).Source;
            vacancy.AnnouncementDate = msg.TimeStamp;
            vacancy.CancelReason = msg.Reason;
            vacancy.Status = VacancyStatus.Cancelled;

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }
    }
}
