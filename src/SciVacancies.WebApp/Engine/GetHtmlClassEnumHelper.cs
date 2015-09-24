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
                case VacancyStatus.OfferRejectedByWinner:
                case VacancyStatus.OfferRejectedByPretender:
                    return "executed"; //green
                case VacancyStatus.Closed:
                case VacancyStatus.Cancelled:
                case VacancyStatus.Removed:
                    return "closed"; //light-grey
                default: return null;
            }
        }
    }
}
