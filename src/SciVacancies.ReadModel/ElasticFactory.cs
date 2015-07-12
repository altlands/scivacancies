using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Nest;

namespace SciVacancies.ReadModel
{
    public class ElasticFactory : ElasticClient
    {
        public ElasticFactory() : base(new ConnectionSettings(new Uri("http://localhost:9200/"), defaultIndex: "scivacancies")) { }
        //public ElasticFactory() : base(new ConnectionSettings(new Uri("http://altlandev01.cloudapp.net:9200/"), defaultIndex: "scivacancies")) { }
    }
}