using System;
using System.Linq;
using System.ComponentModel;
using Microsoft.AspNet.Mvc;
using SciVacancies.WebApp.Engine;

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
        public BindResearcherIdFromClaimsAttribute() : base(ConstTerms.ClaimTypeResearcherId, "researcherGuid", typeof(Guid)) { }
        public BindResearcherIdFromClaimsAttribute(string argumentName) : base(ConstTerms.ClaimTypeResearcherId, argumentName, typeof(Guid)) { }
    }
    public class BindOrganizationIdFromClaimsAttribute : BindArgumentFromClaimsAttribute
    {
        public BindOrganizationIdFromClaimsAttribute(string argumentName) : base(ConstTerms.ClaimTypeOrganizationId, argumentName, typeof(Guid)) { }
        public BindOrganizationIdFromClaimsAttribute() : base(ConstTerms.ClaimTypeOrganizationId, "organizationGuid", typeof(Guid)) { }
    }
}