using System;
using MediatR;
using SciVacancies.Domain.Events;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    //Автофак неправильно резолвит евенты. Вместо ReadModel хэндлера он кидает диспатчить евент сюда. Здесь диспатча не происходит. Приложение валится по StackOverflow




    //public class SagaX : SagaBase, ISagaMessageHandler<VacancyApplicationCreated>
    //{
    //    public void Handle(VacancyApplicationCreated notification)
    //    {
    //        // do nothing
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
    //        var saga = _repository.GetById<SagaX>("","");
    //        saga.Transition<VacancyApplicationCreated>(notification);
    //        _repository.Save("", saga,Guid.NewGuid(), null);
    //    }
    //}


   
}
