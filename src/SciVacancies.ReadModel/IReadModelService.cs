using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPoco;

namespace SciVacancies.ReadModel
{
    public interface IReadModelService
    {
        [Obsolete("Will be changed")]
        Researcher SingleResearcher(Guid researcherGuid);
        [Obsolete("Will be removed")]
        List<Researcher> SelectResearchers();
        [Obsolete("Will be changed")]
        VacancyApplication SingleVacancyApplication(Guid vacancyApplicationGuid);
        [Obsolete("Will be changed")]
        List<VacancyApplication> SelectVacancyApplicationsByResearcher(Guid researcherGuid);
        [Obsolete("Will be changed")]
        List<VacancyApplication> SelectVacancyApplicationsByVacancy(Guid vacancyGuid);
        [Obsolete("Will be changed")]
        SearchSubscription SingleSearchSubscription(Guid searchSubscriptionGuid);
        [Obsolete("Will be removed")]
        List<SearchSubscription> SelectSearchSubscriptions();
        [Obsolete("Will be removed")]
        List<SearchSubscription> SelectSearchSubscriptions(Guid researcherGuid);

        Attachment SingleAttachment(Guid attachmentGuid);
        List<Attachment> SelectAttachments(Guid vacancyApplicationGuid);
        [Obsolete("Will be changed")]
        Vacancy SingleVacancy(Guid vacancyGuid);
        [Obsolete("Will be changed")]
        List<Vacancy> SelectVacancies(Guid organizationGuid);
        [Obsolete("Will be changed")]
        List<Vacancy> SelectVacancies(string title, int count);
        [Obsolete("Will be changed")]
        Page<Vacancy> SelectVacancies(string orderBy, long pageSize, long pageIndex, string nameFilterValue = null, string addressFilterValue = null);
        [Obsolete("Will be removed")]
        List<Vacancy> SelectClosedVacancies(Guid organizationGuid);
        [Obsolete("Will be changed")]
        Page<Vacancy> SelectClosedVacancies(string orderBy, long pageSize, long pageIndex, string nameFilterValue = null, string addressFilterValue = null);
        [Obsolete("Will be changed")]
        List<Vacancy> SelectFavoriteVacancies(Guid researcherGuid);

        [Obsolete("Will be removed")]
        Notification SingleNotification(Guid notificationGuid);
        [Obsolete("Will be changed")]
        List<Notification> SelectNotificationsByResearcher(Guid researcherGuid);
        [Obsolete("Will be changed")]
        List<Notification> SelectNotificationsByOrganization(Guid organizationGuid);
        [Obsolete("Will be removed")]
        int CountNotificationsByResearcher(Guid researcherGuid);
        [Obsolete("Will be removed")]
        int CountNotificationsByOrganization(Guid organizationGuid);

        [Obsolete("Will be changed")]
        Organization SingleOrganization(Guid organizationGuid);
        [Obsolete("Will be removed")]
        List<Organization> SelectOrganizations();
        /// <summary>
        /// Получить заданное количество организаций, офильтрованных по title
        /// </summary>
        /// <param name="title">Значение фильтра для свойства Title</param>
        /// <param name="count">Если count=0, то возвращается весь отфильтрованный список</param>
        /// <returns></returns>
        [Obsolete("Will be changed")]
        List<Organization> SelectOrganizations(string title, int count);
        /// <summary>
        /// Получить список организаций по условиям
        /// </summary>
        /// <param name="orderBy">поле для сортировки</param>
        /// <param name="pageSize">размер страницы</param>
        /// <param name="pageIndex">Номер страницы (начиная с 1)</param>
        /// <param name="nameFilterValue">Значение фильтра для свойства Название организации (не обязательно)</param>
        /// <param name="addressFilterValue">Значение фильтра для свойства Адрес организации (не обязательно)</param>
        /// <returns></returns>
        [Obsolete("Will be changed")]
        Page<Organization> SelectOrganizations(string orderBy, long pageSize, long pageIndex, string nameFilterValue = null, string addressFilterValue = null);


        /// <summary>
        /// Список позиций(шаблонов вакансий)
        /// </summary>
        /// <param name="positionGuid"></param>
        /// <returns></returns>
        [Obsolete("Will be changed")]
        Position SinglePosition(Guid positionGuid);
        [Obsolete("Will be changed")]
        List<Position> SelectPositions(Guid organizationGuid);

        #region Dictionaries
        /// <summary>
        /// Список должностей
        /// </summary>
        /// <returns></returns>
        List<PositionType> SelectPositionTypes();
        /// <summary>
        /// Список должностей с фильтрацией
        /// </summary>
        /// <param name="query">LIKE '%query%'</param>
        /// <returns></returns>
        List<PositionType> SelectPositionTypes(string query);

        List<Activity> SelectActivities();
        List<Activity> SelectActivities(string query);

        List<Foiv> SelectFoivs();
        List<Foiv> SelectFoivs(string query);
        List<Foiv> SelectFoivs(int parentId);

        List<Criteria> SelectCriterias();
        List<Criteria> SelectCriterias(string query);
        List<Criteria> SelectCriterias(int parentId);

        List<OrgForm> SelectOrgForms();
        List<OrgForm> SelectOrgForms(string query);

        List<Region> SelectRegions();
        List<Region> SelectRegions(string query);

        List<ResearchDirection> SelectResearchDirections();
        List<ResearchDirection> SelectResearchDirections(string query);
        List<ResearchDirection> SelectResearchDirections(int parentId);
        #endregion
    }
}
