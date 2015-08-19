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
                    return "draft";
                case VacancyStatus.Published:
                    return "executed";
                case VacancyStatus.InCommittee:
                    return "work";
                case VacancyStatus.Closed:
                    return "failed";
                case VacancyStatus.Cancelled:
                    return "work";
                case VacancyStatus.OfferResponseAwaiting:
                    return "work";
                case VacancyStatus.OfferAccepted:
                    return "work";
                case VacancyStatus.OfferRejected:
                    return "failed";
                case VacancyStatus.Removed:
                    return "failed";
                default:  return null;
            }
        }

        public static string GetHtmlClass(this VacancyApplicationStatus value)
        {
            switch (value)
            {
                case VacancyApplicationStatus.InProcess:
                    return "draft";
                case VacancyApplicationStatus.Cancelled:
                    return "failed";
                case VacancyApplicationStatus.Removed:
                    return "failed";
                case VacancyApplicationStatus.Applied:
                    return "work";
                case VacancyApplicationStatus.Won:
                    return "executed";
                case VacancyApplicationStatus.Pretended:
                    return "executed";
                case VacancyApplicationStatus.Lost:
                    return "failed";
                default:
                    return null;
            }
        }
    }
}
