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
                case VacancyStatus.Published:
                case VacancyStatus.InCommittee:
                    return "work"; //orange
                case VacancyStatus.OfferResponseAwaiting:
                case VacancyStatus.OfferAccepted:
                    return "executed"; //green
                case VacancyStatus.OfferRejected:
                    return "failed"; //red

                case VacancyStatus.Closed:
                case VacancyStatus.Cancelled:
                    return "draft"; //orange
                case VacancyStatus.Removed:
                    return "closed"; //light-grey
                default:  return null;
            }
        }

        public static string GetHtmlClass(this VacancyApplicationStatus value)
        {
            switch (value)
            {
                case VacancyApplicationStatus.InProcess:
                case VacancyApplicationStatus.Applied:
                    return "work"; //orange
                case VacancyApplicationStatus.Won:
                case VacancyApplicationStatus.Pretended:
                    return "executed"; //green
                case VacancyApplicationStatus.Lost:
                case VacancyApplicationStatus.Removed:
                case VacancyApplicationStatus.Cancelled:
                    return "draft"; //grey
                default:
                    return null;
            }
        }
    }
}
