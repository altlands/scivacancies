using System.Collections.Generic;
using SciVacancies.ReadModel.Core;

namespace SciVacancies.Services.SmtpNotificators
{
    public interface ISmtpNotificatorSearchSubscriptionService
    {
        void Notify(SearchSubscription searchSubscription, Researcher researcher,
            List<SciVacancies.ReadModel.ElasticSearchModel.Model.Vacancy> vacancies);
    }
}