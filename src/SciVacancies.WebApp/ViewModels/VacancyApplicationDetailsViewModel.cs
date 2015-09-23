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

        public string StatusDescription
        {
            get
            {
                if (Vacancy == null)
                    return Status.GetDescription();

                var vacancyWinnerInterface = (IVacancyWinnerPretenderInfo)Vacancy;

                #region возвращаемые значения расписаны в соответствии с приложением к ТЗ (Статусы Вакансий и Заявок)
                switch (vacancyWinnerInterface.Status)
                {
                    case VacancyStatus.Published:
                    case VacancyStatus.InCommittee:
                        return "Отправлена";

                    case VacancyStatus.OfferResponseAwaitingFromWinner:
                    case VacancyStatus.OfferResponseAwaitingFromPretender:
                        //описываем заявки проигравших
                        if (vacancyWinnerInterface.WinnerVacancyApplicationGuid != Guid
                            && vacancyWinnerInterface.PretenderVacancyApplicationGuid != Guid)
                            return "Не в финале";
                        //описываем заявку победителя
                        if (vacancyWinnerInterface.WinnerVacancyApplicationGuid == Guid)
                        {
                            //победитель еще не принял решение
                            if (!vacancyWinnerInterface.IsWinnerAccept.HasValue)
                                return "ПОБЕДИТЕЛЬ";
                            //победитель принял решение
                            if (!vacancyWinnerInterface.IsWinnerAccept.Value) //победитель отказался
                                return "Предложение отклонено";
                        }
                        //описываем заявку претендента
                        if (vacancyWinnerInterface.PretenderVacancyApplicationGuid == Guid)
                        {
                            //победитель еще не принял решение
                            if (!vacancyWinnerInterface.IsWinnerAccept.HasValue)
                                return "В финале";
                            //победитель принял решение
                            //претендент еще не принял решение
                            if (!vacancyWinnerInterface.IsPretenderAccept.HasValue)
                                return "ПОБЕДИТЕЛЬ";
                        }
                        break;

                    case VacancyStatus.OfferAcceptedByWinner:
                    case VacancyStatus.OfferAcceptedByPretender:
                        //описываем заявки проигравших
                        if (vacancyWinnerInterface.WinnerVacancyApplicationGuid != Guid
                            && vacancyWinnerInterface.PretenderVacancyApplicationGuid != Guid)
                            return "Отклонена";
                        //описываем заявку победителя
                        if (vacancyWinnerInterface.WinnerVacancyApplicationGuid == Guid)
                        {
                            //победитель принял предложение
                            if (vacancyWinnerInterface.IsWinnerAccept != null && vacancyWinnerInterface.IsWinnerAccept.Value)
                                return "Предложение принято";
                            //победитель отказался от предложения
                            return "Отклонена";
                        }
                        //описываем заявку претендента
                        if (vacancyWinnerInterface.PretenderVacancyApplicationGuid == Guid)
                        {
                            //победитель принял предложение
                            if (vacancyWinnerInterface.IsWinnerAccept != null && vacancyWinnerInterface.IsWinnerAccept.Value)
                                return "Отклонена";
                            //победитель отказался от предложения (значит претендент принял)
                            return "Предложение принято";
                        }
                        break;
                    case VacancyStatus.OfferRejectedByWinner:
                    case VacancyStatus.OfferRejectedByPretender:
                        //описываем заявки проигравших
                        if (vacancyWinnerInterface.WinnerVacancyApplicationGuid != Guid
                            && vacancyWinnerInterface.PretenderVacancyApplicationGuid != Guid)
                            return "Не в финале";
                        //описываем заявку победителя
                        //описываем заявку претендента
                        if (vacancyWinnerInterface.WinnerVacancyApplicationGuid == Guid
                            || vacancyWinnerInterface.PretenderVacancyApplicationGuid == Guid)
                            return "Предложение отклонено";
                        break;
                    case VacancyStatus.Closed:
                        //описываем заявки проигравших
                        if (vacancyWinnerInterface.WinnerVacancyApplicationGuid != Guid
                            && vacancyWinnerInterface.PretenderVacancyApplicationGuid != Guid)
                            return "Отклонена";
                        //описываем заявку победителя
                        if (vacancyWinnerInterface.WinnerVacancyApplicationGuid == Guid)
                        {
                            //победитель принял предложение
                            if (vacancyWinnerInterface.IsWinnerAccept != null && vacancyWinnerInterface.IsWinnerAccept.Value)
                                return "Контракт подписан";
                            //победитель отказался от предложения
                            return "Отклонена";
                        }
                        //описываем заявку претендента
                        if (vacancyWinnerInterface.PretenderVacancyApplicationGuid == Guid)
                        {
                            //победитель принял предложение
                            if (vacancyWinnerInterface.IsWinnerAccept != null && vacancyWinnerInterface.IsWinnerAccept.Value)
                                return "Отклонена";
                            //претендент принял предложение
                            if (vacancyWinnerInterface.IsPretenderAccept != null && vacancyWinnerInterface.IsPretenderAccept.Value)
                                return "Контракт подписан";
                            //претендент отказался от предложения
                            return "Отклонена";
                        }
                        break;
                    case VacancyStatus.Cancelled:
                        return "Отклонена";
                }

                #endregion

                return Status.GetDescription();
            }
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
