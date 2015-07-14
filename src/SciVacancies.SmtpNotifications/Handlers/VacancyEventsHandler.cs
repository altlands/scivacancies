//using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;

using MediatR;
using NPoco;

namespace SciVacancies.SmtpNotifications.Handlers
{
    public class VacancyEventsHandler :
        INotificationHandler<VacancyInCommittee>,
        INotificationHandler<VacancyOfferAcceptedByWinner>,
        INotificationHandler<VacancyOfferRejectedByWinner>,
        INotificationHandler<VacancyOfferAcceptedByPretender>,
        INotificationHandler<VacancyOfferRejectedByPretender>,
        INotificationHandler<VacancyClosed>,
        INotificationHandler<VacancyCancelled>
    {
        private readonly IDatabase _db;

        public VacancyEventsHandler(IDatabase db)
        {
            _db = db;
        }
        public void Handle(VacancyInCommittee msg)
        {
            List<Guid> researcherGuids = _db.Fetch<Guid>(new Sql($"SELECT va.researcher_guid FROM res_vacancyapplications va WHERE va.vacancy_guid = @0", msg.VacancyGuid));
            List<Researcher> researchers = _db.Fetch<Researcher>(new Sql($"SELECT * FROM res_researchers r WHERE r.guid IN (@0)", researcherGuids));

            foreach(Researcher res in researchers)
            {

            }
        }
        public void Handle(VacancyOfferAcceptedByWinner msg)
        {

        }
        public void Handle(VacancyOfferRejectedByWinner msg)
        {

        }
        public void Handle(VacancyOfferAcceptedByPretender msg)
        {

        }
        public void Handle(VacancyOfferRejectedByPretender msg)
        {

        }
        public void Handle(VacancyClosed msg)
        {
            List<Guid> researcherGuids = _db.Fetch<Guid>(new Sql($"SELECT va.researcher_guid FROM res_vacancyapplications va WHERE va.vacancy_guid = @0", msg.VacancyGuid));
            List<Researcher> researchers = _db.Fetch<Researcher>(new Sql($"SELECT * FROM res_researchers r WHERE r.guid IN (@0)", researcherGuids));

            foreach (Researcher res in researchers)
            {

            }
        }
        public void Handle(VacancyCancelled msg)
        {
            List<Guid> researcherGuids = _db.Fetch<Guid>(new Sql($"SELECT va.researcher_guid FROM res_vacancyapplications va WHERE va.vacancy_guid = @0", msg.VacancyGuid));
            List<Researcher> researchers = _db.Fetch<Researcher>(new Sql($"SELECT * FROM res_researchers r WHERE r.guid IN (@0)", researcherGuids));

            foreach (Researcher res in researchers)
            {

            }
        }
    }
}
