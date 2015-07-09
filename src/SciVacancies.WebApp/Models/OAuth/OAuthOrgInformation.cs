using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.WebApp.Models.OAuth
{
    public class OAuthOrgInformation
    {
        public string inn { get; set; }
        public string ogrn { get; set; }
        public string title { get; set; }
        public string shortTitle { get; set; }
        public string email { get; set; }
        public string headFirstName { get; set; }
        public string headLastName { get; set; }
        public string headPatronymic { get; set; }
        public string postAddress { get; set; }
        public string cityName { get; set; }
        public string foiv { get; set; }
        public int foivId { get; set; }
        public string region { get; set; }
        public int regionId { get; set; }
        public string opf { get; set; }
        public int opfId { get; set; }
        public List<OAuthOrgResearchDirection> researchDirections { get; set; }
    }
    public class OAuthOrgResearchDirection
    {
        public string title { get; set; }
        public string id { get; set; }
    }
}
