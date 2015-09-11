using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using MediatR;

namespace SciVacancies.WebApp.Infrastructure.Saga
{


    public class SagaBase : ISaga, IEquatable<ISaga>
    {
        private readonly IDictionary<Type, Action<INotification>> handlers = new Dictionary<Type, Action<INotification>>();

        private readonly ICollection<INotification> uncommitted = new LinkedList<INotification>();

        private readonly ICollection<INotification> undispatched = new LinkedList<INotification>();

        public virtual bool Equals(ISaga other)
        {
            return null != other && other.Id == Id;
        }

        public Guid Id { get; protected set; }

        public int Version { get; private set; }

        public SagaBase()
        {
            //TODO КОСТЫЛЬ
            Type type = this.GetType();
            var methods = type.GetMethods();
            foreach (var method in methods)
            {
                //TODO make it flexible
                if (method.Name == "Handle")
                {
                    var parameterType = method.GetParameters().FirstOrDefault().ParameterType;

                    MethodInfo registerMethod = type.GetMethod("Register", BindingFlags.NonPublic | BindingFlags.Instance);

                    MethodInfo genericMethod = registerMethod.MakeGenericMethod(parameterType);

                    var delegateType = typeof(Action<>).MakeGenericType(parameterType);

                    //TODO make it flexible
                    var handleMethod = type.GetMethod("Handle", new Type[] { parameterType });

                    var del = Delegate.CreateDelegate(delegateType, this, handleMethod);

                    genericMethod.Invoke(this, new[] { del });
                }
            }
        }
        public SagaBase(Guid id) : this()
        {
            this.Id = id;
        }


        public void Transition(object message)
        {
            this.handlers[message.GetType()](message as INotification);
            this.uncommitted.Add(message as INotification);
            this.Version++;
        }

        ICollection ISaga.GetUncommittedEvents()
        {
            return this.uncommitted as ICollection;
        }

        void ISaga.ClearUncommittedEvents()
        {
            this.uncommitted.Clear();
        }

        ICollection ISaga.GetUndispatchedMessages()
        {
            return this.undispatched as ICollection;
        }

        void ISaga.ClearUndispatchedMessages()
        {
            this.undispatched.Clear();
        }

        protected void Register<TRegisteredMessage>(Action<TRegisteredMessage> handler)
            where TRegisteredMessage : class, INotification
        {
            this.handlers[typeof(TRegisteredMessage)] = message => handler(message as TRegisteredMessage);
        }

        protected void Dispatch(INotification message)
        {
            this.undispatched.Add(message);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ISaga);
        }
    }
}