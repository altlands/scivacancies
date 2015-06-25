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

        private List<Position> Positions { get; set; }
        private List<Vacancy> Vacancies { get; set; }

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
                OrganizationGuid = this.Id,
                Data = data
            });
        }
        public void Remove()
        {
            if (!this.Removed)
            {
                RaiseEvent(new OrganizationRemoved()
                {
                    OrganizationGuid = this.Id
                });
            }
        }

        public Guid CreatePosition(PositionDataModel data)
        {
            Guid positionGuid = Guid.NewGuid();
            RaiseEvent(new PositionCreated()
            {
                PositionGuid = positionGuid,
                OrganizationGuid = this.Id,
                Data = data
            });

            return positionGuid;
        }
        public void UpdatePosition(Guid positionGuid, PositionDataModel data)
        {
            Position position = this.Positions.Find(f => f.PositionGuid == positionGuid);
            if (position != null && position.Status == PositionStatus.InProcess)
            {
                RaiseEvent(new PositionUpdated()
                {
                    PositionGuid = position.PositionGuid,
                    OrganizationGuid = this.Id,
                    Data = data
                });
            }
        }
        public void RemovePosition(Guid positionGuid)
        {
            Position position = this.Positions.Find(f => f.PositionGuid == positionGuid);
            if (position != null && position.Status == PositionStatus.InProcess)
            {
                RaiseEvent(new PositionRemoved()
                {
                    PositionGuid = positionGuid,
                    OrganizationGuid = this.Id
                });
            }
        }

        public Guid PublishVacancy(Guid positionGuid, VacancyDataModel data)
        {
            Position position = this.Positions.Find(f => f.PositionGuid == positionGuid);
            if (position != null && position.Status == PositionStatus.InProcess)
            {
                Guid vacancyGuid = Guid.NewGuid();
                RaiseEvent(new VacancyPublished()
                {
                    VacancyGuid = vacancyGuid,
                    PositionGuid = positionGuid,
                    OrganizationGuid = this.Id,
                    Data = data
                });

                return vacancyGuid;
            }

            return Guid.Empty;
        }
        public void SwitchVacancyToAcceptApplications(Guid vacancyGuid)
        {
            Vacancy vacancy = this.Vacancies.Find(f => f.VacancyGuid == vacancyGuid);
            if (vacancy != null && vacancy.Status == VacancyStatus.Published)
            {
                RaiseEvent(new VacancyAcceptApplications()
                {
                    VacancyGuid = vacancyGuid,
                    PositionGuid = vacancy.PositionGuid,
                    OrganizationGuid = this.Id
                });
            }
        }
        public void SwitchVacancyInCommittee(Guid vacancyGuid)
        {
            Vacancy vacancy = this.Vacancies.Find(f => f.VacancyGuid == vacancyGuid);
            if (vacancy != null && vacancy.Status == VacancyStatus.AppliesAcceptance)
            {
                RaiseEvent(new VacancyInCommittee()
                {
                    VacancyGuid = vacancyGuid,
                    PositionGuid = vacancy.PositionGuid,
                    OrganizationGuid = this.Id
                });
            }
        }
        public void CloseVacancy(Guid vacancyGuid, Guid winnerGuid, Guid pretenderGuid)
        {
            Vacancy vacancy = this.Vacancies.Find(f => f.VacancyGuid == vacancyGuid);
            if (vacancy != null && vacancy.Status == VacancyStatus.InCommittee)
            {
                RaiseEvent(new VacancyClosed()
                {
                    VacancyGuid = vacancyGuid,
                    PositionGuid = vacancy.PositionGuid,
                    OrganizationGuid = this.Id,
                    WinnerGuid = winnerGuid,
                    PretenderGuid = pretenderGuid
                });
            }
        }
        public void CancelVacancy(Guid vacancyGuid, string reason)
        {
            Vacancy vacancy = this.Vacancies.Find(f => f.VacancyGuid == vacancyGuid);
            if (vacancy != null && (vacancy.Status == VacancyStatus.Published || vacancy.Status == VacancyStatus.AppliesAcceptance))
            {
                RaiseEvent(new VacancyCancelled()
                {
                    VacancyGuid = vacancyGuid,
                    PositionGuid = vacancy.PositionGuid,
                    OrganizationGuid = this.Id,
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
            this.Data = @event.Data;

        }
        public void Apply(OrganizationRemoved @event)
        {
            this.Removed = true;
        }

        public void Apply(PositionCreated @event)
        {
            this.Positions.Add(new Position()
            {
                PositionGuid = @event.PositionGuid,
                OrganizationGuid = @event.OrganizationGuid,
                Data = @event.Data,
                Status = PositionStatus.InProcess
            });
        }
        public void Apply(PositionUpdated @event)
        {
            Position position = this.Positions.Find(f => f.PositionGuid == @event.PositionGuid);

            //TODO position.Data
        }
        public void Apply(PositionRemoved @event)
        {
            this.Positions.Find(f => f.PositionGuid == @event.PositionGuid).Status = PositionStatus.Removed;
            //this.Positions.Remove(this.Positions.Find(f => f.PositionGuid == @event.PositionGuid));
        }

        public void Apply(VacancyPublished @event)
        {
            this.Positions.Find(f => f.PositionGuid == @event.PositionGuid).Status = PositionStatus.Published;

            this.Vacancies.Add(new Vacancy()
            {
                VacancyGuid = @event.VacancyGuid,
                PositionGuid = @event.PositionGuid,
                OrganizationGuid = @event.OrganizationGuid,
                Data = @event.Data
            });
        }
        public void Apply(VacancyAcceptApplications @event)
        {
            this.Vacancies.Find(f => f.VacancyGuid == @event.VacancyGuid).Status = VacancyStatus.AppliesAcceptance;
        }
        public void Apply(VacancyInCommittee @event)
        {
            this.Vacancies.Find(f => f.VacancyGuid == @event.VacancyGuid).Status = VacancyStatus.InCommittee;
        }
        public void Apply(VacancyClosed @event)
        {
            Vacancy vacancy = this.Vacancies.Find(f => f.VacancyGuid == @event.VacancyGuid);

            vacancy.Data.WinnerGuid = @event.WinnerGuid;
            vacancy.Data.PretenderGuid = @event.PretenderGuid;

            vacancy.Status = VacancyStatus.Closed;
        }
        public void Apply(VacancyCancelled @event)
        {
            this.Vacancies.Find(f => f.VacancyGuid == @event.VacancyGuid).Status = VacancyStatus.Cancelled;
        }
        #endregion
    }
}
