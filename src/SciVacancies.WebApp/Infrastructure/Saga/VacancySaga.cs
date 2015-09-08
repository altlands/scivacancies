using SciVacancies.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SciVacancies.Domain.Events;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public class VacancySaga : SagaBase
    {
        public DateTime PublishStartDate { get; set; }
        public DateTime InComitteeStartDate { get; set; }
        public DateTime OfferResponseStartDate { get; set; }
        public DateTime ClosedDate { get; set; }
        public VacancyStatus State { get; set; }

        public void Handle(VacancyApplicationCreated notification)
        {
            // do nothing
            var test = 0;
            Id = Guid.NewGuid().ToString();

            var atimer = new System.Timers.Timer(10000);
            atimer.AutoReset = true;

            atimer.Elapsed += new System.Timers.ElapsedEventHandler(RequestTimeout);

            atimer.Enabled = true;

            GC.KeepAlive(atimer);
        }
        private void RequestTimeout(object source, System.Timers.ElapsedEventArgs e)
        {
            switch (this.State)
            {
                case VacancyStatus.Published:
                    if (this.InComitteeStartDate < DateTime.UtcNow)
                    {

                    }
                    break;
                case VacancyStatus.InCommittee:
                    if (this.OfferResponseStartDate < DateTime.UtcNow)
                    {

                    }
                    break;
                case VacancyStatus.OfferResponseAwaiting:
                    if (this.ClosedDate < DateTime.UtcNow)
                    {

                    }
                    break;
            }

            var test = this.Id;
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
        }

    }
}

//using System;
//using MediatR;
//using SciVacancies.Domain.Events;

//namespace SciVacancies.WebApp.Infrastructure.Saga
//{
//    public class SagaX : SagaBase, ISagaMessageHandler<VacancyApplicationCreated>
//    {


//        public void Handle(VacancyApplicationCreated notification)
//        {
//            // do nothing
//            var test = 0;
//            Id = Guid.NewGuid().ToString();
//        }
//    }

//    public class XSagaActivator : INotificationHandler<VacancyApplicationCreated>
//    {
//        private readonly ISagaRepository _repository;

//        public XSagaActivator(ISagaRepository repository)
//        {
//            _repository = repository;
//        }

//        public void Handle(VacancyApplicationCreated notification)
//        {
//            var test = 0;
//            var saga = new SagaX();
//            //saga.Transition<VacancyApplicationCreated>(notification);
//            _repository.Save("123", saga, Guid.NewGuid(), null);
//            //var saga = _repository.GetById<SagaX>("", "");
//            //saga.Transition<VacancyApplicationCreated>(notification);
//            //_repository.Save("", saga, Guid.NewGuid(), null);
//        }
//    }



//}
