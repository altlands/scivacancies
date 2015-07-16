using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.WebApp.Models.OAuth
{
    public class OAuthOrgInformation
    {
        public string email { get; set; }
        public OAuthOrgFoiv foiv { get; set; }
        public string headFirstName { get; set; }
        public string headLastName { get; set; }
        public string headPatronymic { get; set; }
        public string inn { get; set; }
        public string ogrn { get; set; }


        public OAuthOrgOpf opf { get; set; }

        public string postAddress { get; set; }

        public OAuthOrgRegion region { get; set; }

        public List<OAuthOrgResearchDirection> researchDirections { get; set; }

        public string title { get; set; }
        public string shortTitle { get; set; }
    }
    public class OAuthOrgResearchDirection
    {
        public string title { get; set; }
        public string id { get; set; }
    }
    public class OAuthOrgFoiv
    {
        public string id { get; set; }
        public string title { get; set; }
    }
    public class OAuthOrgOpf
    {
        public string id { get; set; }
        public string title { get; set; }
    }
    public class OAuthOrgRegion
    {
        public string id { get; set; }
        public string title { get; set; }
    }
}
