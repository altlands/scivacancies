namespace SciVacancies.WebApp.Engine
{
    public class ConstTerms
    {
        public const string OrderByRelevant= "relevant";
        public const string OrderByDateDescending = "date";
        public const string OrderByDateAscending = "date_ascending";
        public const string OrderByDateStartDescending = "DateStart";
        public const string OrderByDateStartAscending = "DateStart_ascending";
        public const string OrderByCreationDateDescending = "CreationDate";
        public const string OrderByCreationDateAscending = "CreationDate_ascending";

        public const string OrderByVacancyCountDescending = "PublishedVacancies";
        public const string OrderByVacancyCountAscending = "PublishedVacancies_ascending";
        public const string OrderByNameDescending = "name";
        public const string OrderByNameAscending =  "name_ascending";
        public const string OrderByCountDescending = "count";
        public const string OrderByCountAscending =  "count_ascending";

        public const string RequireRoleOrganizationAdmin = "organization_admin";
        public const string RequireRoleResearcher = "researcher";
        public const string RequireRoleAdministrator = "administrator";
        public const string RequireRoleRoot = "root";

        public const string ClaimTypeOrganizationId = "organization_id";
        public const string ClaimTypeResearcherId = "researcher_id";
    }
}
