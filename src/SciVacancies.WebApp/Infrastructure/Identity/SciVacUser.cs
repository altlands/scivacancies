using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using NPoco;

namespace SciVacancies.WebApp.Infrastructure.Identity
{
    public class SciVacUser : IdentityUser
    {
        /// <summary>
        /// время последних запросов на восстановление пароля
        /// </summary>
        [Ignore]
        public List<DateTime> LastRequests
        {
            get
            {
                return !string.IsNullOrWhiteSpace(PasswordRequests) ? JsonConvert.DeserializeObject<List<DateTime>>(PasswordRequests) : new List<DateTime>();
            }
            set
            {
                PasswordRequests  = value.Any() ? JsonConvert.SerializeObject(value.Select(c=>c.ToUniversalTime()).ToList()) : string.Empty;
            }
        }

        /// <summary>
        /// время последних запросов на восстановление пароля
        /// </summary>
        public string PasswordRequests { get; set; }

    }
}
