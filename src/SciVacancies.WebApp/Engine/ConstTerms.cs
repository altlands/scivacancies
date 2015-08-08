namespace SciVacancies.WebApp.Engine
{
    public class ConstTerms
    {
        public const string OrderByAscending = "asc";
        public const string OrderByDescending= "desc";

        public const string OrderDirectionFieldName = "SortDirection";
        public const string OrderFieldName = "SortField";



        public const string OrderByFieldDate = "date";
        public const string OrderByFieldApplyDate = "applydate";
        public const string OrderByFieldFullName= "fullname";
        public const string OrderByFieldPublishDate = "publishdate";
        public const string OrderByFieldCreationDate = "creationdate";
        public const string OrderByFieldClosedDate = "closeddate";
        public const string OrderByFieldVacancyStatus= "vacancystatus";

        public const string OrderByName= "name"; //?? это значение ещё востребовано
        public const string OrderByCount= "count"; //?? это значение ещё востребовано


        public const string OrderByCreationDateDescending = "CreationDate";
        public const string OrderByDateStartDescending = "DateStart";
        //public const string OrderByDateStartAscending = "DateStart_ascending";
        public const string OrderByCreationDateAscending = "CreationDate_ascending";

        public const string OrderByVacancyCountDescending = "PublishedVacancies";
        public const string OrderByVacancyCountAscending = "PublishedVacancies_ascending";

        public const string SearchFilterOrderByRelevant = "relevant";
        public const string SearchFilterOrderByDateDescending = "date_descending";
        public const string SearchFilterOrderByDateAscending = "date_ascanding";

        public const string RequireRoleOrganizationAdmin = "organization_admin";
        public const string RequireRoleResearcher = "researcher";
        public const string RequireRoleAdministrator = "administrator";
        public const string RequireRoleRoot = "root";

        public const string ClaimTypeOrganizationId = "organization_id";
        public const string ClaimTypeResearcherId = "researcher_id";

        public const string FolderApplicationsAttachments= "\\uploads\\applications\\attachments";
        public const string FolderResearcherPhoto = "\\uploads\\researcherphoto";
    }
}
