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
                //return "failed"; //red
                //return "draft"; //orange

                case VacancyStatus.InProcess:
                case VacancyStatus.Published:
                case VacancyStatus.InCommittee:
                    return "work"; //orange
                case VacancyStatus.OfferResponseAwaitingFromWinner:
                case VacancyStatus.OfferResponseAwaitingFromPretender:
                case VacancyStatus.OfferAcceptedByWinner:
                case VacancyStatus.OfferAcceptedByPretender:
                    return "executed"; //green
                case VacancyStatus.OfferRejectedByWinner:
                case VacancyStatus.OfferRejectedByPretender:
                    return "closed"; //light-grey
                case VacancyStatus.Closed:
                case VacancyStatus.Cancelled:
                case VacancyStatus.Removed:
                    return "closed"; //light-grey
                default: return null;
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
