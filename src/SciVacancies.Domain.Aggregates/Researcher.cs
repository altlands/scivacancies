using SciVacancies.Domain.Core;
using SciVacancies.Domain.DataModels;
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

        private List<Guid> FavoriteVacancyGuids { get; set; }
        private List<SearchSubscription> SearchSubscriptions { get; set; }

        private List<VacancyApplication> VacancyApplications { get; set; }

        public Researcher()
        {
            FavoriteVacancyGuids = new List<Guid>();
            SearchSubscriptions = new List<SearchSubscription>();
            VacancyApplications = new List<VacancyApplication>();
        }
        public Researcher(Guid guid, ResearcherDataModel data)
        {
            FavoriteVacancyGuids = new List<Guid>();
            SearchSubscriptions = new List<SearchSubscription>();
            VacancyApplications = new List<VacancyApplication>();

            RaiseEvent(new ResearcherCreated()
            {
                ResearcherGuid = guid,
                Data = data
            });
        }
        public void Update(ResearcherDataModel data)
        {
            RaiseEvent(new ResearcherUpdated()
            {
                ResearcherGuid = this.Id,
                Data = data
            });
        }
        public void Remove()
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
            if (!this.FavoriteVacancyGuids.Contains(vacancyGuid))
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
            if (this.FavoriteVacancyGuids.Contains(vacancyGuid))
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

        public Guid CreateSearchSubscription(SearchSubscriptionDataModel data)
        {
            Guid searchSubscriptionGuid = Guid.NewGuid();
            RaiseEvent(new SearchSubscriptionCreated()
            {
                SearchSubscriptionGuid = searchSubscriptionGuid,
                ResearcherGuid = this.Id,
                Data = data
            });

            return searchSubscriptionGuid;
        }
        public void ActivateSearchSubscription(Guid searchSubscriptionGuid)
        {
            SearchSubscription searchSubscription = this.SearchSubscriptions.Find(f => f.SearchSubscriptionGuid == searchSubscriptionGuid);
            if (searchSubscription != null && searchSubscription.Status == SearchSubscriptionStatus.Cancelled)
            {
                RaiseEvent(new SearchSubscriptionActivated()
                {
                    SearchSubscriptionGuid = searchSubscriptionGuid,
                    ResearcherGuid = this.Id
                });
            }
        }
        public void CancelSearchSubscription(Guid searchSubscriptionGuid)
        {
            SearchSubscription searchSubscription = this.SearchSubscriptions.Find(f => f.SearchSubscriptionGuid == searchSubscriptionGuid);
            if (searchSubscription != null && searchSubscription.Status == SearchSubscriptionStatus.Active)
            {
                RaiseEvent(new SearchSubscriptionCanceled()
                {
                    SearchSubscriptionGuid = searchSubscriptionGuid,
                    ResearcherGuid = this.Id
                });
            }
        }
        public void RemoveSearchSubscription(Guid searchSubscriptionGuid)
        {
            SearchSubscription searchSubscription = this.SearchSubscriptions.Find(f => f.SearchSubscriptionGuid == searchSubscriptionGuid);
            if (searchSubscription != null)
            {
                RaiseEvent(new SearchSubscriptionRemoved()
                {
                    SearchSubscriptionGuid = searchSubscriptionGuid,
                    ResearcherGuid = this.Id
                });
            }
        }

        public Guid CreateVacancyApplicationTemplate(Guid vacancyGuid, VacancyApplicationDataModel data)
        {
            Guid vacancyApplicationGuid = Guid.NewGuid();
            RaiseEvent(new VacancyApplicationCreated()
            {
                VacancyApplicationGuid = vacancyApplicationGuid,
                VacancyGuid = vacancyGuid,
                ResearcherGuid = this.Id,
                Data = data
            });

            return vacancyApplicationGuid;
        }
        public void UpdateVacancyApplicationTemplate(Guid vacancyApplicationGuid, VacancyApplicationDataModel data)
        {
            VacancyApplication vacancyApplication = this.VacancyApplications.Find(f => f.VacancyApplicationGuid == vacancyApplicationGuid);
            if (vacancyApplication != null && vacancyApplication.Status == VacancyApplicationStatus.InProcess)
            {
                RaiseEvent(new VacancyApplicationUpdated()
                {
                    VacancyApplicationGuid = vacancyApplication.VacancyApplicationGuid,
                    ResearcherGuid = this.Id,
                    Data = data
                });
            }
        }
        public void RemoveVacancyApplicationTemplate(Guid vacancyApplicationGuid)
        {
            VacancyApplication vacancyApplication = this.VacancyApplications.Find(f => f.VacancyApplicationGuid == vacancyApplicationGuid);
            if (vacancyApplication != null && vacancyApplication.Status == VacancyApplicationStatus.InProcess)
            {
                RaiseEvent(new VacancyApplicationRemoved()
                {
                    VacancyApplicationGuid = vacancyApplicationGuid,
                    VacancyGuid = vacancyApplication.VacancyGuid,
                    ResearcherGuid = this.Id
                });
            }
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
        public void CancelVacancyApplication(Guid vacancyApplicationGuid)
        {
            VacancyApplication vacancyApplication = this.VacancyApplications.Find(f => f.VacancyApplicationGuid == vacancyApplicationGuid);
            if (vacancyApplication != null && vacancyApplication.Status == VacancyApplicationStatus.Applied)
            {
                RaiseEvent(new VacancyApplicationCancelled()
                {
                    VacancyApplicationGuid = vacancyApplicationGuid,
                    VacancyGuid = vacancyApplication.VacancyGuid,
                    ResearcherGuid = this.Id
                });
            }
        }
        public void MakeVacancyApplicationWinner(Guid vacancyApplicationGuid)
        {
            VacancyApplication vacancyApplication = this.VacancyApplications.Find(f => f.VacancyApplicationGuid == vacancyApplicationGuid);
            if (vacancyApplication != null && vacancyApplication.Status == VacancyApplicationStatus.Applied)
            {
                RaiseEvent(new VacancyApplicationWon
                {
                    VacancyApplicationGuid = vacancyApplicationGuid,
                    VacancyGuid = vacancyApplication.VacancyGuid,
                    ResearcherGuid = this.Id
                });
            }
        }
        public void MakeVacancyApplicationPretender(Guid vacancyApplicationGuid)
        {
            VacancyApplication vacancyApplication = this.VacancyApplications.Find(f => f.VacancyApplicationGuid == vacancyApplicationGuid);
            if (vacancyApplication != null && vacancyApplication.Status == VacancyApplicationStatus.Applied)
            {
                RaiseEvent(new VacancyApplicationPretended
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

            this.Data.Educations = @event.Data.Educations;
            this.Data.Publications = @event.Data.Publications;
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

        public void Apply(SearchSubscriptionCreated @event)
        {
            this.SearchSubscriptions.Add(new SearchSubscription()
            {
                SearchSubscriptionGuid = @event.SearchSubscriptionGuid,
                Data = @event.Data,
                Status = SearchSubscriptionStatus.Active
            });
        }
        public void Apply(SearchSubscriptionActivated @event)
        {
            this.SearchSubscriptions.Find(f => f.SearchSubscriptionGuid == @event.SearchSubscriptionGuid).Status = SearchSubscriptionStatus.Active;
        }
        public void Apply(SearchSubscriptionCanceled @event)
        {
            this.SearchSubscriptions.Find(f => f.SearchSubscriptionGuid == @event.SearchSubscriptionGuid).Status = SearchSubscriptionStatus.Cancelled;
        }
        public void Apply(SearchSubscriptionRemoved @event)
        {
            this.SearchSubscriptions.Remove(this.SearchSubscriptions.Find(f => f.SearchSubscriptionGuid == @event.SearchSubscriptionGuid));
        }

        public void Apply(VacancyApplicationCreated @event)
        {
            this.VacancyApplications.Add(new VacancyApplication()
            {
                VacancyApplicationGuid = @event.VacancyApplicationGuid,
                VacancyGuid = @event.VacancyGuid,
                Data = @event.Data,
                Status = VacancyApplicationStatus.InProcess
            });
        }
        public void Apply(VacancyApplicationUpdated @event)
        {
            VacancyApplication vacancyApplication = this.VacancyApplications.Find(f => f.VacancyApplicationGuid == @event.VacancyApplicationGuid);
            //TODO - маппинг изменяемых данных
            //vacancyApplication.Data.
        }
        public void Apply(VacancyApplicationRemoved @event)
        {
            this.VacancyApplications.Find(f => f.VacancyApplicationGuid == @event.VacancyApplicationGuid).Status = VacancyApplicationStatus.Removed;
            //this.VacancyApplications.Remove(this.VacancyApplications.Find(f => f.VacancyApplicationGuid == @event.VacancyApplicationGuid));
        }
        public void Apply(VacancyApplicationApplied @event)
        {
            this.VacancyApplications.Find(f => f.VacancyApplicationGuid == @event.VacancyApplicationGuid).Status = VacancyApplicationStatus.Applied;
        }
        public void Apply(VacancyApplicationCancelled @event)
        {
            this.VacancyApplications.Find(f => f.VacancyApplicationGuid == @event.VacancyApplicationGuid).Status = VacancyApplicationStatus.Cancelled;
        }
        public void Apply(VacancyApplicationWon @event)
        {
            this.VacancyApplications.Find(f => f.VacancyApplicationGuid == @event.VacancyApplicationGuid).Status = VacancyApplicationStatus.Won;
        }
        public void Apply(VacancyApplicationPretended @event)
        {
            this.VacancyApplications.Find(f => f.VacancyApplicationGuid == @event.VacancyApplicationGuid).Status = VacancyApplicationStatus.Pretended;
        }
        #endregion
    }
}
