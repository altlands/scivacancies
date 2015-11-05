using System.Collections.Generic;

using Microsoft.Extensions.Caching;
using Microsoft.Extensions.Caching.Memory;

using Autofac;
using MediatR;


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