using System;
using System.ComponentModel;
using SciVacancies.Domain.Enums;

namespace SciVacancies.WebApp.Engine
{

    public static class GetHtmlClassEnumHelper
    {

        public static string GetHtmlClass(this VacancyStatus value)
        {
            switch (value)
            {
                case VacancyStatus.InProcess:
                    return "draft"; //grey
                case VacancyStatus.Published:
                    return "executed"; //green
                case VacancyStatus.InCommittee:
                    return "work"; //orange
                case VacancyStatus.Closed:
                    return "closed"; //light-grey
                case VacancyStatus.Cancelled:
                    return "work"; //orange
                case VacancyStatus.OfferResponseAwaiting:
                    return "work"; //orange
                case VacancyStatus.OfferAccepted:
                    return "work"; //orange
                case VacancyStatus.OfferRejected:
                    return "failed"; //red
                case VacancyStatus.Removed:
                    return "failed"; //red
                default:  return null;
            }
        }

        public static string GetHtmlClass(this VacancyApplicationStatus value)
        {
            switch (value)
            {
                case VacancyApplicationStatus.InProcess:
                    return "draft"; //grey
                case VacancyApplicationStatus.Cancelled:
                    return "failed"; //red
                case VacancyApplicationStatus.Removed:
                    return "failed"; //red
                case VacancyApplicationStatus.Applied:
                    return "work"; //orange
                case VacancyApplicationStatus.Won:
                    return "executed"; //green
                case VacancyApplicationStatus.Pretended:
                    return "executed"; //green
                case VacancyApplicationStatus.Lost:
                    return "failed"; //red
                default:
                    return null;
            }
        }
    }
}
