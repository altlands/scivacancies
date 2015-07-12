using SciVacancies.Domain.Core;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;

using System;
using System.Collections.Generic;

using CommonDomain.Core;

namespace SciVacancies.Domain.Aggregates
{
    public class Vacancy : AggregateBase
    {
        private Guid OrganizationGuid { get; set; }

        private VacancyDataModel Data { get; set; }

        private VacancyStatus Status { get; set; }

        private List<Guid> VacancyApplicationGuids { get; set; }

        public Vacancy()
        {

        }
        public Vacancy(Guid guid, Guid organizationGuid, VacancyDataModel data)
        {
            RaiseEvent(new VacancyCreated
            {

            });
        }

        public void Update(VacancyDataModel data)
        {
            if (Status == VacancyStatus.InProcess)
            {
                RaiseEvent(new VacancyUpdated
                {

                });
            }
        }
        public void Remove()
        {
            if (Status == VacancyStatus.InProcess)
            {
                RaiseEvent(new VacancyRemoved
                {

                });
            }
        }
        public void Publish()
        {
            if (Status == VacancyStatus.InProcess)
            {
                RaiseEvent(new VacancyPublished
                {

                });
            }
        }
        public void VacancyToCommittee()
        {
            if (Status == VacancyStatus.Published)
            {
                RaiseEvent(new VacancyInCommittee
                {

                });
            }
        }
        public void SetWinner()
        {
            if (Status == VacancyStatus.InCommittee)
            {
                RaiseEvent(new VacancyWinnerSet
                {

                });
            }
        }
        public void SetPretender()
        {
            if (Status == VacancyStatus.InCommittee)
            {
                RaiseEvent(new VacancyPretenderSet
                {

                });
            }
        }
        public void WinnerAcceptOffer()
        {
            if (Status == VacancyStatus.InCommittee)
            {
                RaiseEvent(new VacancyOfferAcceptedByWinner
                {

                });
            }
        }
        public void WinnerRejectOffer()
        {
            if (Status == VacancyStatus.InCommittee)
            {
                RaiseEvent(new VacancyOfferRejectedByWinner
                {

                });
            }
        }
        public void PretenderAcceptOffer()
        {
            if (Status == VacancyStatus.InCommittee)
            {
                RaiseEvent(new VacancyOfferAcceptedByPretender
                {

                });
            }
        }
        public void PretenderRejectOffer()
        {
            if (Status == VacancyStatus.InCommittee)
            {
                RaiseEvent(new VacancyOfferRejectedByPretender
                {

                });
            }
        }
        public void Close()
        {
            if (Status == VacancyStatus.OfferAccepted)
            {
                RaiseEvent(new VacancyClosed
                {

                });
            }
        }
        public void Cancel()
        {
            if (Status == VacancyStatus.Published || Status == VacancyStatus.InCommittee || Status == VacancyStatus.OfferRejected)
            {
                RaiseEvent(new VacancyCancelled
                {

                });
            }
        }
    }
}
