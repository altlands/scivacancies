using System;
using System.Linq;
using System.ComponentModel;
using Microsoft.AspNet.Mvc;

namespace SciVacancies.WebApp.Controllers
{
    public class BindArgumentFromClaimsAttribute : ActionFilterAttribute
    {
        protected string Key;
        protected string ArguName;
        protected Type ArgumentType;

        public BindArgumentFromClaimsAttribute() { }

        public BindArgumentFromClaimsAttribute(string keys, string arguName) : this(keys, arguName, typeof(Guid)) { }
        public BindArgumentFromClaimsAttribute(string keys, string arguName, Type argumentType)
        {
            Key = keys;
            ArguName = arguName;
            ArgumentType = argumentType;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var idClaim = ((Controller)context.Controller).User.Claims.FirstOrDefault(c => c.Type == Key);

            if (idClaim != null)
                context.ActionArguments[ArguName] = TypeDescriptor.GetConverter(ArgumentType).ConvertFrom(idClaim.Value);

            base.OnActionExecuting(context);
        }
    }

    public class BindResearcherIdFromClaimsAttribute : BindArgumentFromClaimsAttribute
    {
        public BindResearcherIdFromClaimsAttribute() : base("researcher_id", "researcherGuid", typeof(Guid)) { }
    }
    public class BindOrganizationIdFromClaimsAttribute : BindArgumentFromClaimsAttribute
    {
        public BindOrganizationIdFromClaimsAttribute() : base("organization_id", "organizationGuid", typeof(Guid)) { }
    }
}