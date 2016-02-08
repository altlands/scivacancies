using System.Collections.Generic;

using Microsoft.Extensions.Caching.Memory;

using Autofac;


namespace SciVacancies.WebApp.Infrastructure
{
    public class CacheModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MemoryCache(new MemoryCacheOptions() {}))
                .As<IMemoryCache>()
                .SingleInstance();
        }
    }
}