using SciVacancies.Domain.Core;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;

using System;
using System.Collections.Generic;

using CommonDomain.Core;

namespace SciVacancies.Domain.Aggregates
{
    public class Organization : AggregateBase
    {

        private bool Removed { get; set; }
        public OrganizationDataModel Data { get; set; }

        private List<Position> Positions { get; }
        private List<Vacancy> Vacancies { get; }

        public Organization()
        {
            Positions = new List<Position>();
            Vacancies = new List<Vacancy>();
        }
        public Organization(Guid guid, OrganizationDataModel data)
        {
            Positions = new List<Position>();
            Vacancies = new List<Vacancy>();
            RaiseEvent(new OrganizationCreated()
            {
                OrganizationGuid = guid,
                Data = data
            });
        }

        public void Update(OrganizationDataModel data)
        {
            RaiseEvent(new OrganizationUpdated()
            {
                OrganizationGuid = Id,
                Data = data
            });
        }
        public void Remove()
        {
            if (!Removed)
            {
                RaiseEvent(new OrganizationRemoved()
                {
                    OrganizationGuid = Id
                });
            }
        }

        public Guid CreatePosition(PositionDataModel data)
        {
            var positionGuid = Guid.NewGuid();
            RaiseEvent(new PositionCreated()
            {
                PositionGuid = positionGuid,
                OrganizationGuid = Id,
                Data = data
            });

            return positionGuid;
        }
        public void UpdatePosition(Guid positionGuid, PositionDataModel data)
        {
            var position = Positions.Find(f => f.PositionGuid == positionGuid);
            if (position != null && position.Status == PositionStatus.InProcess)
            {
                RaiseEvent(new PositionUpdated()
                {
                    PositionGuid = position.PositionGuid,
                    OrganizationGuid = Id,
                    Data = data
                });
            }
        }
        public void RemovePosition(Guid positionGuid)
        {
            var position = Positions.Find(f => f.PositionGuid == positionGuid);
            if (position != null && position.Status == PositionStatus.InProcess)
            {
                RaiseEvent(new PositionRemoved()
                {
                    PositionGuid = positionGuid,
                    OrganizationGuid = Id
                });
            }
        }

        public Guid PublishVacancy(Guid positionGuid, VacancyDataModel data)
        {
            var position = Positions.Find(f => f.PositionGuid == positionGuid);
            if (position != null && position.Status == PositionStatus.InProcess)
            {
                var vacancyGuid = Guid.NewGuid();
                RaiseEvent(new VacancyPublished()
                {
                    VacancyGuid = vacancyGuid,
                    PositionGuid = positionGuid,
                    OrganizationGuid = Id,
                    Data = data
                });

                return vacancyGuid;
            }

            return Guid.Empty;
        }
        public void SwitchVacancyToAcceptApplications(Guid vacancyGuid)
        {
            var vacancy = Vacancies.Find(f => f.VacancyGuid == vacancyGuid);
            if (vacancy != null && vacancy.Status == VacancyStatus.Published)
            {
                RaiseEvent(new VacancyAcceptApplications()
                {
                    VacancyGuid = vacancyGuid,
                    PositionGuid = vacancy.PositionGuid,
                    OrganizationGuid = Id
                });
            }
        }
        public void SwitchVacancyInCommittee(Guid vacancyGuid)
        {
            var vacancy = Vacancies.Find(f => f.VacancyGuid == vacancyGuid);
            if (vacancy != null && vacancy.Status == VacancyStatus.AppliesAcceptance)
            {
                RaiseEvent(new VacancyInCommittee()
                {
                    VacancyGuid = vacancyGuid,
                    PositionGuid = vacancy.PositionGuid,
                    OrganizationGuid = Id
                });
            }
        }
        public void CloseVacancy(Guid vacancyGuid, Guid winnerGuid, Guid pretenderGuid)
        {
            var vacancy = Vacancies.Find(f => f.VacancyGuid == vacancyGuid);
            if (vacancy != null && vacancy.Status == VacancyStatus.InCommittee)
            {
                RaiseEvent(new VacancyClosed()
                {
                    VacancyGuid = vacancyGuid,
                    PositionGuid = vacancy.PositionGuid,
                    OrganizationGuid = Id,
                    WinnerGuid = winnerGuid,
                    PretenderGuid = pretenderGuid
                });
            }
        }
        public void CancelVacancy(Guid vacancyGuid, string reason)
        {
            var vacancy = Vacancies.Find(f => f.VacancyGuid == vacancyGuid);
            if (vacancy != null && (vacancy.Status == VacancyStatus.Published || vacancy.Status == VacancyStatus.AppliesAcceptance))
            {
                RaiseEvent(new VacancyCancelled()
                {
                    VacancyGuid = vacancyGuid,
                    PositionGuid = vacancy.PositionGuid,
                    OrganizationGuid = Id,
                    Reason = reason
                });
            }
        }

        #region Apply-Handlers
        public void Apply(OrganizationCreated @event)
        {
            Id = @event.OrganizationGuid;
            Data = @event.Data;
        }
        public void Apply(OrganizationUpdated @event)
        {
            Data = @event.Data;

        }
        public void Apply(OrganizationRemoved @event)
        {
            Removed = true;
        }

        public void Apply(PositionCreated @event)
        {
            Positions.Add(new Position()
            {
                PositionGuid = @event.PositionGuid,
                OrganizationGuid = @event.OrganizationGuid,
                Data = @event.Data,
                Status = PositionStatus.InProcess
            });
        }
        public void Apply(PositionUpdated @event)
        {
            var position = Positions.Find(f => f.PositionGuid == @event.PositionGuid);

            //TODO position.Data
        }
        public void Apply(PositionRemoved @event)
        {
            Positions.Remove(Positions.Find(f => f.PositionGuid == @event.PositionGuid));
        }

        public void Apply(VacancyPublished @event)
        {
            Positions.Find(f => f.PositionGuid == @event.PositionGuid).Status = PositionStatus.Published;

            Vacancies.Add(new Vacancy()
            {
                VacancyGuid = @event.VacancyGuid,
                PositionGuid = @event.PositionGuid,
                OrganizationGuid = @event.OrganizationGuid,
                Data = @event.Data
            });
        }
        public void Apply(VacancyAcceptApplications @event)
        {
            Vacancies.Find(f => f.VacancyGuid == @event.VacancyGuid).Status = VacancyStatus.AppliesAcceptance;
        }
        public void Apply(VacancyInCommittee @event)
        {
            Vacancies.Find(f => f.VacancyGuid == @event.VacancyGuid).Status = VacancyStatus.InCommittee;
        }
        public void Apply(VacancyClosed @event)
        {
            var vacancy = Vacancies.Find(f => f.VacancyGuid == @event.VacancyGuid);

            vacancy.Data.WinnerGuid = @event.WinnerGuid;
            vacancy.Data.PretenderGuid = @event.PretenderGuid;

            vacancy.Status = VacancyStatus.Closed;
        }
        public void Apply(VacancyCancelled @event)
        {
            Vacancies.Find(f => f.VacancyGuid == @event.VacancyGuid).Status = VacancyStatus.Cancelled;
        }
        #endregion
    }
}
