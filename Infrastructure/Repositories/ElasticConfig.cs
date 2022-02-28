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
            try
            {
                var node = new Uri("https://search-mike-smart-f4xjzhosk6etqx4gaq4m4nu4my.us-east-1.es.amazonaws.com/");
                var settings = new ConnectionSettings(node);
                settings.DefaultIndex("smart");
                settings.RequestTimeout(TimeSpan.FromSeconds(300));
                settings.BasicAuthentication("admin", "Michael1@");
                //settings.DisableDirectStreaming();

                return new ElasticClient(settings);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
    }
}