using System;
using System.Reflection;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public class SagaFactory : IConstructSagas
    {
        public ISaga Build(Type type, Guid id)
        {
            ConstructorInfo constructor = type.GetConstructor(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new[] { typeof(Guid) },
                null);

            if (constructor==null) throw new ArgumentNullException("constructor is null");

            return constructor.Invoke(new object[] { id }) as ISaga;
        }
    }
}
