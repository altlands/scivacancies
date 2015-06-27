using System;

namespace SciVacancies.WebApp.Infrastructure.Saga
{
    public interface IConstructSagas
    {
        ISaga Build(Type type, string id);
    }
}