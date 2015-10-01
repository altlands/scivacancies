using System;
using System.Collections.Generic;
using SciVacancies.Domain.Enums;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Interfaces;
using SciVacancies.WebApp.ViewModels.Base;

namespace SciVacancies.WebApp.ViewModels
{
    public class VacancyApplicationDetailsViewModel : PageViewModelBase
    {

        public Guid VacancyGuid { get; set; }
        public VacancyDetailsViewModel Vacancy { get; set; }
        public Guid ResearcherGuid { get; set; }
        public ResearcherDetailsViewModel Researcher { get; set; }

        public int PositionTypeId { get; set; }
        public string PositionTypeName { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ExtraPhone { get; set; }

        public List<ActivityEditViewModel> ResearchActivity { get; set; }
        public List<ActivityEditViewModel> TeachingActivity { get; set; }
        public List<ActivityEditViewModel> OtherActivity { get; set; }

        public string ScienceDegree { get; set; }
        public string AcademicStatus { get; set; }
        public List<RewardDetailsViewModel> Rewards { get; set; }
        public List<MembershipDetailsViewModel> Memberships { get; set; }
        public List<ConferenceEditViewModel> Conferences { get; set; }


        public VacancyApplicationStatus Status { get; set; }

        public Tuple<string, string> StatusDescription
        {
            get
            {
                if (Vacancy == null)
                    return new Tuple<string, string>(Status.GetDescription(), "draft"); //grey

                if (Status == VacancyApplicationStatus.Lost)
                    //описываем заявки проигравших
                    return new Tuple<string, string>("Не в финале", "draft"); //grey

                //return new Tuple<string, string>("Предложение отклонено", "draft"); //grey

                var vacancyWinnerInterface = (IVacancyWinnerPretenderInfo)Vacancy;

                #region возвращаемые значения расписаны в соответствии с приложением к ТЗ (Статусы Вакансий и Заявок)
                switch (vacancyWinnerInterface.Status)
                {
                    case VacancyStatus.Published:
                    case VacancyStatus.InCommittee:
                        return new Tuple<string, string>("Отправлена", "work"); //orange

                    case VacancyStatus.OfferResponseAwaitingFromWinner:
                        //описываем заявку победителя
                        if (vacancyWinnerInterface.WinnerVacancyApplicationGuid == Guid)
                            return new Tuple<string, string>("ПОБЕДИТЕЛЬ", "executed"); //green

                        //описываем заявку претендента
                        if (vacancyWinnerInterface.PretenderVacancyApplicationGuid == Guid)
                            return new Tuple<string, string>("В финале", "executed"); //green
                        break;

                    case VacancyStatus.OfferAcceptedByWinner:
                        //описываем заявку победителя
                        if (vacancyWinnerInterface.WinnerVacancyApplicationGuid == Guid)
                            return new Tuple<string, string>("Предложение принято", "executed"); //green
                        //описываем заявку претендента
                        if (vacancyWinnerInterface.PretenderVacancyApplicationGuid == Guid)
                            return new Tuple<string, string>("В финале", "executed"); //green
                        break;

                    case VacancyStatus.OfferRejectedByWinner:
                        //описываем заявку победителя
                        if (vacancyWinnerInterface.WinnerVacancyApplicationGuid == Guid)
                            return new Tuple<string, string>("Предложение отклонено", "draft"); //grey
                        //описываем заявку претендента
                        if (vacancyWinnerInterface.PretenderVacancyApplicationGuid == Guid)
                            return new Tuple<string, string>("В финале", "executed"); //green
                        break;

                    case VacancyStatus.OfferResponseAwaitingFromPretender:
                        //описываем заявку победителя
                        if (vacancyWinnerInterface.WinnerVacancyApplicationGuid == Guid)
                            return DisplayWinnersDecision(vacancyWinnerInterface);
                        //описываем заявку претендента
                        if (vacancyWinnerInterface.PretenderVacancyApplicationGuid == Guid)
                            return new Tuple<string, string>("ПОБЕДИТЕЛЬ", "executed"); //green
                        break;

                    case VacancyStatus.OfferAcceptedByPretender:
                        //описываем заявку победителя
                        if (vacancyWinnerInterface.WinnerVacancyApplicationGuid == Guid)
                            return DisplayWinnersDecision(vacancyWinnerInterface);
                        //описываем заявку претендента
                        if (vacancyWinnerInterface.PretenderVacancyApplicationGuid == Guid)
                            return new Tuple<string, string>("Предложение принято", "executed"); //green
                        break;

                    case VacancyStatus.OfferRejectedByPretender:
                        //описываем заявку победителя
                        if (vacancyWinnerInterface.WinnerVacancyApplicationGuid == Guid)
                            return DisplayWinnersDecision(vacancyWinnerInterface);
                        //описываем заявку претендента
                        if (vacancyWinnerInterface.PretenderVacancyApplicationGuid == Guid)
                            return new Tuple<string, string>("Предложение отклонено", "draft"); //grey
                        break;

                    case VacancyStatus.Closed:
                        //описываем заявку победителя
                        if (vacancyWinnerInterface.WinnerVacancyApplicationGuid == Guid)
                        {
                            //претенденту делали предложение
                            if (vacancyWinnerInterface.PretenderRequestDate.HasValue)
                                return DisplayWinnersDecision(vacancyWinnerInterface);
                            //претенденту НЕ делали предложение
                            return new Tuple<string, string>("Контракт подписан", "executed"); //green
                        }

                        //описываем заявку претендента
                        if (vacancyWinnerInterface.PretenderVacancyApplicationGuid == Guid)
                        {
                            //претенденту делали предложение
                            if (vacancyWinnerInterface.PretenderRequestDate.HasValue)
                                return new Tuple<string, string>("Контракт подписан", "executed"); //green
                            //претенденту НЕ делали предложение
                            return new Tuple<string, string>("Не в финале", "draft"); //grey
                        }
                        break;

                    case VacancyStatus.Cancelled:
                        //описываем заявку победителя
                        if (vacancyWinnerInterface.WinnerVacancyApplicationGuid == Guid)
                            return DisplayWinnersDecision(vacancyWinnerInterface);

                        //описываем заявку претендента
                        if (vacancyWinnerInterface.PretenderVacancyApplicationGuid == Guid)
                        {
                            //претенденту делали предложение
                            if (vacancyWinnerInterface.PretenderRequestDate.HasValue)
                            {
                                //претендент принял предложение
                                if (vacancyWinnerInterface.IsPretenderAccept.HasValue && vacancyWinnerInterface.IsPretenderAccept.Value)
                                    return new Tuple<string, string>("Предложение отозвано", "draft"); //grey
                                //претендент отказался от предложения
                                return new Tuple<string, string>("Предложение отклонено", "draft"); //grey
                            }

                            //претенденту НЕ делали предложение
                            return new Tuple<string, string>("Не в финале", "draft"); //grey
                        }
                        break;

                }

                #endregion

                return new Tuple<string, string>(Status.GetDescription(), "draft"); //grey
            }
        }

        /// <summary>
        /// победитель принял решение. показать статус заявки победителя
        /// </summary>
        /// <param name="vacancyWinnerInterface"></param>
        /// <returns></returns>
        private static Tuple<string, string> DisplayWinnersDecision(IVacancyWinnerPretenderInfo vacancyWinnerInterface)
        {
            if (vacancyWinnerInterface.IsWinnerAccept.HasValue && vacancyWinnerInterface.IsWinnerAccept.Value)
                //победитель принял предложение
                return new Tuple<string, string>("Предложение отозвано", "draft"); //grey
            //победитель отказался от предложения
            return new Tuple<string, string>("Предложение отклонено", "draft"); //grey
        }

        public DateTime CreationDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? SentDate { get; set; }
        /// <summary>
        /// Прикрепленные файлы
        /// </summary>
        public IEnumerable<VacancyApplicationAttachment> Attachments { get; set; }

        public string CoveringLetter { get; set; }

        /// <summary>
        /// Образование
        /// </summary>
        public List<Education> Educations { get; set; }
        /// <summary>
        /// Публикации
        /// </summary>
        public List<Publication> Publications { get; set; }

        public string FolderApplicationsAttachmentsUrl { get; set; }
    }
}
