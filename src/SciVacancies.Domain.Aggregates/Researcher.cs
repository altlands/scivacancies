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
        private ResearcherDataModel Data { get; set; }

        private ResearcherStatus Status { get; set; }

        private List<VacancyApplication> VacancyApplications { get; set; }

        public List<SearchSubscription> SearchSubscriptions { get; set; }

        public List<Guid> FavoriteVacancyGuids { get; set; }

        public Researcher()
        {
            VacancyApplications = new List<VacancyApplication>();
            SearchSubscriptions = new List<SearchSubscription>();
            FavoriteVacancyGuids = new List<Guid>();
        }
        public Researcher(Guid guid, ResearcherDataModel data)
        {
            VacancyApplications = new List<VacancyApplication>();
            SearchSubscriptions = new List<SearchSubscription>();
            FavoriteVacancyGuids = new List<Guid>();

            RaiseEvent(new ResearcherCreated()
            {
                ResearcherGuid = guid,
                Data = data
            });
        }

        #region Methods

        public void Update(ResearcherDataModel data)
        {
            if (Status == ResearcherStatus.Active)
            {
                RaiseEvent(new ResearcherUpdated()
                {
                    ResearcherGuid = this.Id,
                    Data = data
                });
            }
        }
        public void Remove()
        {
            if (Status != ResearcherStatus.Removed)
            {
                RaiseEvent(new ResearcherRemoved()
                {
                    ResearcherGuid = this.Id
                });
            }
        }

        public int AddVacancyToFavorites(Guid vacancyGuid)
        {
            if (Status == ResearcherStatus.Active && !this.FavoriteVacancyGuids.Contains(vacancyGuid))
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
            if (Status == ResearcherStatus.Active && this.FavoriteVacancyGuids.Contains(vacancyGuid))
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
            if (Status == ResearcherStatus.Active)
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
            return Guid.Empty;
        }
        public void ActivateSearchSubscription(Guid searchSubscriptionGuid)
        {
            if (Status == ResearcherStatus.Active)
            {
                SearchSubscription searchSubscription = this.SearchSubscriptions.Find(f => f.Guid == searchSubscriptionGuid);
                if (searchSubscription != null && searchSubscription.Status == SearchSubscriptionStatus.Cancelled)
                {
                    RaiseEvent(new SearchSubscriptionActivated()
                    {
                        SearchSubscriptionGuid = searchSubscriptionGuid,
                        ResearcherGuid = this.Id
                    });
                }
            }
        }
        public void CancelSearchSubscription(Guid searchSubscriptionGuid)
        {
            if (Status == ResearcherStatus.Active)
            {
                SearchSubscription searchSubscription = this.SearchSubscriptions.Find(f => f.Guid == searchSubscriptionGuid);
                if (searchSubscription != null && searchSubscription.Status == SearchSubscriptionStatus.Active)
                {
                    RaiseEvent(new SearchSubscriptionCanceled()
                    {
                        SearchSubscriptionGuid = searchSubscriptionGuid,
                        ResearcherGuid = this.Id
                    });
                }
            }
        }
        public void RemoveSearchSubscription(Guid searchSubscriptionGuid)
        {
            if (Status == ResearcherStatus.Active)
            {
                SearchSubscription searchSubscription = this.SearchSubscriptions.Find(f => f.Guid == searchSubscriptionGuid);
                if (searchSubscription != null)
                {
                    RaiseEvent(new SearchSubscriptionRemoved()
                    {
                        SearchSubscriptionGuid = searchSubscriptionGuid,
                        ResearcherGuid = this.Id
                    });
                }
            }
        }

        public Guid CreateVacancyApplicationTemplate(Guid vacancyGuid, VacancyApplicationDataModel data)
        {
            if (Status == ResearcherStatus.Active)
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
            return Guid.Empty;
        }
        public void UpdateVacancyApplicationTemplate(Guid vacancyApplicationGuid, VacancyApplicationDataModel data)
        {
            if (Status == ResearcherStatus.Active)
            {
                VacancyApplication vacancyApplication = this.VacancyApplications.Find(f => f.Guid == vacancyApplicationGuid);
                if (vacancyApplication != null && vacancyApplication.Status == VacancyApplicationStatus.InProcess)
                {
                    RaiseEvent(new VacancyApplicationUpdated()
                    {
                        VacancyApplicationGuid = vacancyApplication.Guid,
                        ResearcherGuid = this.Id,
                        Data = data
                    });
                }
            }
        }
        public void RemoveVacancyApplicationTemplate(Guid vacancyApplicationGuid)
        {
            if (Status == ResearcherStatus.Active)
            {
                VacancyApplication vacancyApplication = this.VacancyApplications.Find(f => f.Guid == vacancyApplicationGuid);
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
        }
        public void ApplyToVacancy(Guid vacancyApplicationGuid)
        {
            if (Status == ResearcherStatus.Active)
            {
                VacancyApplication vacancyApplication = this.VacancyApplications.Find(f => f.Guid == vacancyApplicationGuid);
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
        }
        public void CancelVacancyApplication(Guid vacancyApplicationGuid)
        {
            VacancyApplication vacancyApplication = this.VacancyApplications.Find(f => f.Guid == vacancyApplicationGuid);
            if (vacancyApplication != null && (vacancyApplication.Status == VacancyApplicationStatus.InProcess || vacancyApplication.Status == VacancyApplicationStatus.Applied))
            {
                RaiseEvent(new VacancyApplicationCancelled()
                {
                    VacancyApplicationGuid = vacancyApplicationGuid,
                    VacancyGuid = vacancyApplication.VacancyGuid,
                    ResearcherGuid = this.Id
                });
            }
        }
        public void MakeVacancyApplicationWinner(Guid vacancyApplicationGuid, string reason)
        {
            VacancyApplication vacancyApplication = this.VacancyApplications.Find(f => f.Guid == vacancyApplicationGuid);
            if (vacancyApplication != null && vacancyApplication.Status == VacancyApplicationStatus.Applied)
            {
                RaiseEvent(new VacancyApplicationWon
                {
                    VacancyApplicationGuid = vacancyApplicationGuid,
                    VacancyGuid = vacancyApplication.VacancyGuid,
                    ResearcherGuid = this.Id,
                    Reason = reason
                });
            }
        }
        public void MakeVacancyApplicationPretender(Guid vacancyApplicationGuid, string reason)
        {
            VacancyApplication vacancyApplication = this.VacancyApplications.Find(f => f.Guid == vacancyApplicationGuid);
            if (vacancyApplication != null && vacancyApplication.Status == VacancyApplicationStatus.Applied)
            {
                RaiseEvent(new VacancyApplicationPretended
                {
                    VacancyApplicationGuid = vacancyApplicationGuid,
                    VacancyGuid = vacancyApplication.VacancyGuid,
                    ResearcherGuid = this.Id,
                    Reason = reason
                });
            }
        }
        public void MakeVacancyApplicationLooser(Guid vacancyApplicationGuid)
        {
            VacancyApplication vacancyApplication = this.VacancyApplications.Find(f => f.Guid == vacancyApplicationGuid);
            if (vacancyApplication != null && vacancyApplication.Status == VacancyApplicationStatus.Applied)
            {
                RaiseEvent(new VacancyApplicationLost
                {
                    VacancyApplicationGuid = vacancyApplicationGuid,
                    VacancyGuid = vacancyApplication.VacancyGuid,
                    ResearcherGuid = this.Id
                });
            }
        }

        #endregion

        #region Apply-Handlers

        public void Apply(ResearcherCreated @event)
        {
            this.Id = @event.ResearcherGuid;
            this.Data = @event.Data;
        }
        public void Apply(ResearcherUpdated @event)
        {
            this.Data = @event.Data;
        }
        public void Apply(ResearcherRemoved @event)
        {
            this.Status = ResearcherStatus.Removed;
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
                Guid = @event.SearchSubscriptionGuid,
                Data = @event.Data,
                Status = SearchSubscriptionStatus.Active
            });
        }
        public void Apply(SearchSubscriptionActivated @event)
        {
            this.SearchSubscriptions.Find(f => f.Guid == @event.SearchSubscriptionGuid).Status = SearchSubscriptionStatus.Active;
        }
        public void Apply(SearchSubscriptionCanceled @event)
        {
            this.SearchSubscriptions.Find(f => f.Guid == @event.SearchSubscriptionGuid).Status = SearchSubscriptionStatus.Cancelled;
        }
        public void Apply(SearchSubscriptionRemoved @event)
        {
            this.SearchSubscriptions.Find(f => f.Guid == @event.SearchSubscriptionGuid).Status = SearchSubscriptionStatus.Removed;
        }

        public void Apply(VacancyApplicationCreated @event)
        {
            this.VacancyApplications.Add(new VacancyApplication()
            {
                Guid = @event.VacancyApplicationGuid,
                VacancyGuid = @event.VacancyGuid,
                Data = @event.Data
            });
        }
        public void Apply(VacancyApplicationUpdated @event)
        {
            VacancyApplication vacancyApplication = this.VacancyApplications.Find(f => f.Guid == @event.VacancyApplicationGuid);
            vacancyApplication.Data = @event.Data;
        }
        public void Apply(VacancyApplicationRemoved @event)
        {
            this.VacancyApplications.Find(f => f.Guid == @event.VacancyApplicationGuid).Status = VacancyApplicationStatus.Removed;
        }
        public void Apply(VacancyApplicationApplied @event)
        {
            this.VacancyApplications.Find(f => f.Guid == @event.VacancyApplicationGuid).Status = VacancyApplicationStatus.Applied;
        }
        public void Apply(VacancyApplicationCancelled @event)
        {
            this.VacancyApplications.Find(f => f.Guid == @event.VacancyApplicationGuid).Status = VacancyApplicationStatus.Cancelled;
        }
        public void Apply(VacancyApplicationWon @event)
        {
            this.VacancyApplications.Find(f => f.Guid == @event.VacancyApplicationGuid).Status = VacancyApplicationStatus.Won;
        }
        public void Apply(VacancyApplicationPretended @event)
        {
            this.VacancyApplications.Find(f => f.Guid == @event.VacancyApplicationGuid).Status = VacancyApplicationStatus.Pretended;
        }
        public void Apply(VacancyApplicationLost @event)
        {
            this.VacancyApplications.Find(f => f.Guid == @event.VacancyApplicationGuid).Status = VacancyApplicationStatus.Lost;
        }

        #endregion
    }
}
