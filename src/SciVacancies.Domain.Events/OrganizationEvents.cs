﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciVacancies.Domain.Events
{
    public class OrganizationEventBase : EventBase
    {
        public OrganizationEventBase() : base() { }

        public Guid OrganizationGuid { get; set; }
    }
    
    public class OrganizationCreated : OrganizationEventBase
    {
        public OrganizationCreated() : base() { }

        public string Name { get; set; }
        public string ShortName { get; set; }
    }
    
    public class OrganizationRemoved : OrganizationEventBase
    {
        public OrganizationRemoved() : base() { }
    }
    public class OrganizationUpdated : OrganizationEventBase
    {
        public OrganizationUpdated() : base() { }

        public DateTime UpdateDate { get; set; }
        //public Guid Id { get; set; }
        //public string Login { get; set; }
        //public string FirstName { get; set; }
        //public string SecondName { get; set; }
        //public string Patronymic { get; set; }
        //public string Email { get; set; }
        //public string Phone { get; set; }
    }
}
