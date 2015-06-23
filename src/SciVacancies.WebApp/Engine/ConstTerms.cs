namespace SciVacancies.WebApp.Engine
{
    public class ConstTerms
    {
        public const string OrderByDateDescending = "date_descending";
        public const string OrderByDateAscending = "date_ascending";
        public const string OrderByVacancyCountDescending = "vacancycount_descending";
        public const string OrderByVacancyCountAscending = "vacancycount_ascending";
        public const string OrderByNameDescending = "name_descending";
        public const string OrderByNameAscending =  "name_ascending";
        public const string OrderByCountDescending = "count_descending";
        public const string OrderByCountAscending =  "count_ascending";

        public const string RequireRoleOrganizationAdmin = "organization_admin";
        public const string RequireRoleResearcher = "researcher";
        public const string RequireRoleAdministrator = "administrator";
        public const string RequireRoleRoot = "root";

        public const string ClaimTypeOrganizationId = "organization_id";
        public const string ClaimTypeResearcherId = "researcher_id";
    }
}
