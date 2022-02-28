using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Nest;

namespace Infrastructure.Services
{
    public class AppService : IAppService
    {
        private static IElasticClient _client;

        public AppService(IElasticConfig _config)
        {
            _client = _config.GetClient();
        }
        
        public void IndexManagement()
        {
            try
            {
            
                var mgtData = File.ReadAllText("../Infrastructure/Data/SeedData/mgmt.json");
                var mgts = JsonSerializer.Deserialize<List<MgtObj>>(mgtData);

                // var response = _client.Indices.Create("mgmt2", 
                //     index => index.Map<List<MgtObj>>(x => x.AutoMap()));

                //var res = _client.IndexDocument(mgts);

                //var res = _client.IndexMany(mgts);

                var mgmtIndexResponse = _client.Bulk(b => b
                    .Index("mgmt")
                    .IndexMany(mgts)
                );

                // var resMgmt = _client.Indices.Delete("mgmt");
                
            }
            catch (Exception ex)
            {                
                throw;
            }
        }
        
        public void IndexProperty()
        {
            try
            {
            
                var propData = File.ReadAllText("../Infrastructure/Data/SeedData/properties.json");
                var props = JsonSerializer.Deserialize<List<PropertyObject>>(propData);

                var propIndexResponse = _client.Bulk(b => b
                    .Index("prop")
                    .IndexMany(props)
                );

                // var resProp = _client.Indices.Delete("prop");
                
            }
            catch (Exception ex)
            {                
                throw;
            }
        }
    }
}