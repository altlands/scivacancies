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
        INotificationHandler<VacancyProlongedInCommittee>,
        INotificationHandler<VacancyCommitteeResolutionSet>,
        INotificationHandler<VacancyInOfferResponseAwaitingFromWinner>,
        INotificationHandler<VacancyOfferAcceptedByWinner>,
        INotificationHandler<VacancyOfferRejectedByWinner>,
        INotificationHandler<VacancyInOfferResponseAwaitingFromPretender>,
        INotificationHandler<VacancyOfferAcceptedByPretender>,
        INotificationHandler<VacancyOfferRejectedByPretender>,
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
            vacancy.CommitteeStartDate = msg.InCommitteeStartDate;
            vacancy.CommitteeEndDate = msg.InCommitteeEndDate;
            vacancy.Status = VacancyStatus.Published;

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }
        public void Handle(VacancyInCommittee msg)
        {
            Vacancy vacancy = _elasticClient.Get<Vacancy>(msg.VacancyGuid.ToString()).Source;
            vacancy.CommitteeStartDate = msg.TimeStamp;
            vacancy.Status = VacancyStatus.InCommittee;

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }
        public void Handle(VacancyProlongedInCommittee msg)
        {
            Vacancy vacancy = _elasticClient.Get<Vacancy>(msg.VacancyGuid.ToString()).Source;
            vacancy.CommitteeEndDate = msg.InCommitteeEndDate;
            vacancy.ProlongingInCommitteeReason = msg.Reason;

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }
        public void Handle(VacancyCommitteeResolutionSet msg)
        {
            Vacancy vacancy = _elasticClient.Get<Vacancy>(msg.VacancyGuid.ToString()).Source;
            vacancy.CommitteeResolution = msg.Resolution;

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }
        public void Handle(VacancyInOfferResponseAwaitingFromWinner msg)
        {
            Vacancy vacancy = _elasticClient.Get<Vacancy>(msg.VacancyGuid.ToString()).Source;
            vacancy.AwaitingDate = msg.TimeStamp;
            vacancy.Status = VacancyStatus.OfferResponseAwaitingFromWinner;

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }
        public void Handle(VacancyOfferAcceptedByWinner msg)
        {
            Vacancy vacancy = _elasticClient.Get<Vacancy>(msg.VacancyGuid.ToString()).Source;
            vacancy.Status = VacancyStatus.OfferAcceptedByWinner;

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }
        public void Handle(VacancyOfferRejectedByWinner msg)
        {
            Vacancy vacancy = _elasticClient.Get<Vacancy>(msg.VacancyGuid.ToString()).Source;
            vacancy.Status = VacancyStatus.OfferRejectedByWinner;

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }
        public void Handle(VacancyInOfferResponseAwaitingFromPretender msg)
        {
            Vacancy vacancy = _elasticClient.Get<Vacancy>(msg.VacancyGuid.ToString()).Source;
            vacancy.Status = VacancyStatus.OfferResponseAwaitingFromPretender;

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }
        public void Handle(VacancyOfferAcceptedByPretender msg)
        {
            Vacancy vacancy = _elasticClient.Get<Vacancy>(msg.VacancyGuid.ToString()).Source;
            vacancy.Status = VacancyStatus.OfferAcceptedByPretender;

            _elasticClient.Update<Vacancy>(u => u.IdFrom(vacancy).Doc(vacancy));
        }
        public void Handle(VacancyOfferRejectedByPretender msg)
        {
            Vacancy vacancy = _elasticClient.Get<Vacancy>(msg.VacancyGuid.ToString()).Source;
            vacancy.Status = VacancyStatus.OfferRejectedByPretender;

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
