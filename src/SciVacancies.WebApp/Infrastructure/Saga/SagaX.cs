using System;
using MediatR;
using SciVacancies.Domain.Events;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    //public class SagaX : SagaBase, ISagaMessageHandler<VacancyApplicationCreated>
    //{


    //    public void Handle(VacancyApplicationCreated notification)
    //    {
    //        // do nothing
    //        var test = 0;
    //        Id = Guid.NewGuid().ToString();

    //        var atimer = new System.Timers.Timer(10000);
    //        atimer.AutoReset = true;

    //        atimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);

    //        atimer.Enabled = true;

    //        GC.KeepAlive(atimer);
    //    }
    //    private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
    //    {
    //        var test = this.Id;
    //        Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
    //    }
    //}

    //public class XSagaActivator : INotificationHandler<VacancyApplicationCreated>
    //{
    //    private readonly ISagaRepository _repository;

    //    public XSagaActivator(ISagaRepository repository)
    //    {
    //        _repository = repository;
    //    }

    //    public void Handle(VacancyApplicationCreated notification)
    //    {
    //        var test = 0;
    //        var saga = new SagaX();
    //        saga.Transition<VacancyApplicationCreated>(notification);
    //        //var atimer = new System.Timers.Timer(10000);
    //        //atimer.AutoReset = true;

    //        //atimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);

    //        //atimer.Enabled = true;

    //        _repository.Save("123", saga, Guid.NewGuid(), null);
    //        //var saga = _repository.GetById<SagaX>("", "");
    //        //saga.Transition<VacancyApplicationCreated>(notification);
    //        //_repository.Save("", saga, Guid.NewGuid(), null);
    //        //GC.KeepAlive(atimer);
    //    }
    //    private static void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
    //    {
    //        Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
    //    }
    //}



}
