using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Nest;

namespace Infrastructure.Repositories
{
    public class ElasticConfig : IElasticConfig
    {
        public ElasticClient GetClient()
        {
            var node = new Uri("{OPEN_SEARCH_URL}");
            var settings = new ConnectionSettings(node);
            settings.DefaultIndex("prop");
            settings.RequestTimeout(TimeSpan.FromSeconds(300));
            settings.PrettyJson();
            settings.BasicAuthentication("{USERNAME}", "{PASSWORD}");
            //settings.DisableDirectStreaming();

            return new ElasticClient(settings);
        }
    }
}