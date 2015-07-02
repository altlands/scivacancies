using MediatR;
using Microsoft.AspNet.Mvc;
using Nest;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.Queries;

using Newtonsoft.Json.Linq;
using System;
//using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
//using System.Web.Mvc;
using Thinktecture.IdentityModel.Client;

namespace SciVacancies.WebApp.Controllers
{
    public class ResearcherController : Controller
    {
        private readonly IElasticClient _elastic;
        private readonly IMediator _mediator;

        public ResearcherController(IElasticClient elastic, IMediator mediator)
        {
            _elastic = elastic;
            _mediator = mediator;
        }

        Uri authEndpoint = new Uri("http://www.sciencemon.ru/oauth/v2/auth");
        Uri tokenEndpoint = new Uri("http://www.sciencemon.ru/oauth/v2/token");
        string clientId = "1_ikniwc909y8kog4k4sc00oogg0g8scc8o4k0wocgw4cg84k00";
        string clientSecret = "67sq61c6xekgw0wgkosg04gwo488osk48ogks4og40cgws8ook";
        Uri redirectUrl = new Uri("http://localhost:59075/researcher/callback");
        //Uri redirectUrl = new Uri("http://localhost:59075");
        string scope = null;

        // GET: /<controller>/
        public ActionResult Index(string id)
        {

            var client = new OAuth2Client(authEndpoint);

            var s = client.CreateCodeFlowUrl(clientId, scope, redirectUrl.AbsoluteUri);

            return Redirect(s);
        }

        public void Callback(string code)
        {
            var client = new OAuth2Client(tokenEndpoint);

            //var s = client.RequestAuthorizationCodeAsync(code,)

            var r = 0;

        }
        //private async Task<>



        private async Task<TokenResponse> GetTokenAsync()
        {
            var client = new OAuth2Client(new Uri("http://www.sciencemon.ru/oauth/v2/token"), "1_ikniwc909y8kog4k4sc00oogg0g8scc8o4k0wocgw4cg84k00", "67sq61c6xekgw0wgkosg04gwo488osk48ogks4og40cgws8ook");
            //var client = new OAuth2Client(new Uri("http://www.sciencemon.ru/oauth/v2/auth"), "1_ikniwc909y8kog4k4sc00oogg0g8scc8o4k0wocgw4cg84k00", "67sq61c6xekgw0wgkosg04gwo488osk48ogks4og40cgws8ook");

            //var s=client.CreateCodeFlowUrl()

            return await client.RequestClientCredentialsAsync();
        }

        private async Task<string> CallApi(string token)
        {
            var client = new System.Net.Http.HttpClient();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            //var response =  client.GetAsync(new Uri("http://www.sciencemon.ru/oauth/v2/auth")).Result;
            var response = client.GetAsync(new Uri("http://www.sciencemon.ru/api/user")).Result;

            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
