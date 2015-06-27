using MediatR;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public interface ISagaMessageHandler<in TMessage> where TMessage : INotification
    {       
        void Handle(TMessage message);
    }    
}
