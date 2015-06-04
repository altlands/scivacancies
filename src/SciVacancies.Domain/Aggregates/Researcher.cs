using SciVacancies.Domain.Core;
using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;

using System;
using System.Collections.Generic;

using CommonDomain.Core;

namespace SciVacancies.Domain.Aggregates
{
    public class Researcher : AggregateBase
    {
        private bool Removed { get; set; }
        private ResearcherDataModel Data { get; set; }

        //private List<Education> EducationList { get; set; }
        //private List<Publication> Publications { get; set; }
        private List<Guid> FavoriteVacancyGuids { get; set; }
        //private List<SearchSubscription> SearchSubscriptions { get; set; }

        private List<VacancyApplication> VacancyApplications { get; set; }

        public Researcher()
        {

        }
        public Researcher(Guid guid, ResearcherDataModel data)
        {
            RaiseEvent(new ResearcherCreated()
            {
                ResearcherGuid = guid,
                Data = data
            });
        }
        public void UpdateResearcher(ResearcherDataModel data)
        {
            RaiseEvent(new ResearcherUpdated()
            {
                ResearcherGuid = this.Id,
                Data = data
            });
        }
        public void RemoveResearcher()
        {
            if (!Removed)
            {
                RaiseEvent(new ResearcherRemoved()
                {
                    ResearcherGuid = this.Id
                });
            }
        }

        public int AddVacancyToFavorites(Guid vacancyGuid)
        {
            if(!this.FavoriteVacancyGuids.Contains(vacancyGuid))
            {
                RaiseEvent(new VacancyAddedToFavorites()
                {
                    VacancyGuid = vacancyGuid,
                    ResearcherGuid = this.Id
                });
                return this.FavoriteVacancyGuids.Count + 1;
            }
            else
            {
                return this.FavoriteVacancyGuids.Count;
            }

        }
        public int RemoveVacancyFromFavorites(Guid vacancyGuid)
        {
            if(this.FavoriteVacancyGuids.Contains(vacancyGuid))
            {
                RaiseEvent(new VacancyRemovedFromFavorites()
                {
                    VacancyGuid = vacancyGuid,
                    ResearcherGuid = this.Id
                });
                return this.FavoriteVacancyGuids.Count - 1;
            }
            else
            {
                return this.FavoriteVacancyGuids.Count;
            }
        }

        public Guid CreateVacancyApplicationTemplate(Guid vacancyGuid)
        {
            Guid vacancyApplicationGuid = Guid.NewGuid();
            RaiseEvent(new VacancyApplicationCreated()
            {
                VacancyApplicationGuid = vacancyApplicationGuid,
                VacancyGuid = vacancyGuid,
                ResearcherGuid = this.Id
            });

            return vacancyApplicationGuid;
        }
        public void ApplyToVacancy(Guid vacancyApplicationGuid)
        {
            VacancyApplication vacancyApplication = this.VacancyApplications.Find(f => f.VacancyApplicationGuid == vacancyApplicationGuid);
            if (vacancyApplication != null && vacancyApplication.Status == VacancyApplicationStatus.InProcess)
            {
                RaiseEvent(new VacancyApplicationApplied()
                {
                    VacancyApplicationGuid = vacancyApplicationGuid,
                    VacancyGuid = vacancyApplication.VacancyGuid,
                    ResearcherGuid = this.Id
                });
            }
        }

        #region Apply-Handlers
        public void Apply(ResearcherCreated @event)
        {
            this.Id = @event.ResearcherGuid;
            this.Data = @event.Data;
        }
        public void Apply(ResearcherUpdated @event)
        {
            this.Data.FirstName = @event.Data.FirstName;
            this.Data.SecondName = @event.Data.SecondName;
            this.Data.Patronymic = @event.Data.Patronymic;

            this.Data.FirstNameEng = @event.Data.FirstNameEng;
            this.Data.SecondNameEng = @event.Data.SecondNameEng;
            this.Data.PatronymicEng = @event.Data.PatronymicEng;

            this.Data.PreviousSecondName = @event.Data.PreviousSecondName;

            this.Data.BirthDate = @event.Data.BirthDate;

            this.Data.ExtraEmail = @event.Data.ExtraEmail;

            this.Data.ExtraPhone = @event.Data.ExtraPhone;

            this.Data.Nationality = @event.Data.Nationality;

            this.Data.ResearchActivity = @event.Data.ResearchActivity;
            this.Data.TeachingActivity = @event.Data.TeachingActivity;
            this.Data.OtherActivity = @event.Data.OtherActivity;

            this.Data.ScienceDegree = @event.Data.ScienceDegree;
            this.Data.AcademicStatus = @event.Data.AcademicStatus;
            this.Data.Rewards = @event.Data.Rewards;
            this.Data.Memberships = @event.Data.Memberships;
            this.Data.Conferences = @event.Data.Conferences;

            //this.Data.Publications = @event.Data.Publications;
        }
        public void Apply(ResearcherRemoved @event)
        {
            this.Removed = true;
        }

        public void Apply(VacancyAddedToFavorites @event)
        {
            this.FavoriteVacancyGuids.Add(@event.VacancyGuid);
        }
        public void Apply(VacancyRemovedFromFavorites @event)
        {
            this.FavoriteVacancyGuids.Remove(@event.VacancyGuid);
        }

        public void Apply(VacancyApplicationCreated @event)
        {
            this.VacancyApplications.Add(new VacancyApplication()
            {
                VacancyApplicationGuid = @event.VacancyApplicationGuid,
                VacancyGuid = @event.VacancyGuid,
                Status = VacancyApplicationStatus.InProcess
            });
        }
        public void Apply(VacancyApplicationApplied @event)
        {
            this.VacancyApplications.Find(f => f.VacancyApplicationGuid == @event.VacancyApplicationGuid).Status = VacancyApplicationStatus.Applied;
        }
        #endregion
    }
}
