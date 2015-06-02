using SciVacancies.Domain.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.MssqlDB.Handlers
{
    public class ApplicantCreatedHandler : EventBaseHandler<ApplicantCreated>
    {
        public ApplicantCreatedHandler(IDatabase db) : base(db) { }
        public override void Handle(ApplicantCreated msg)
        {
            
        }
    }
    //public class RequestCreatedHandler:EventBaseHandler<RequestCreated>
    //{
    //    public RequestCreatedHandler(IDatabase db) : base(db) { }
    //    public override void Handle(RequestCreated msg)
    //    {
            
    //    }
    //}
}
