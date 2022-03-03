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
        
        public async Task<BulkResponse> IndexManagementAsync(IEnumerable<Mgmt> mgts)
        {

            if(_client.Indices.Exists("mgmt").Exists)
            {
                _client.Indices.Delete("mgmt");
            }

            var mgmtIndexResponse = await _client.BulkAsync(b => b
                .Index("mgmt")
                .IndexMany(mgts)
            );


            return mgmtIndexResponse;

        }
        
        public async Task<BulkResponse> IndexPropertyAsync(IEnumerable<Property> properties)
        {
            // _client.Indices.Create("smart",
            //         index => index.Map<Property>(x => x.AutoMap()));

            if(_client.Indices.Exists("prop").Exists)
            {
                _client.Indices.Delete("prop");
            }

            var propIndexResponse = await _client.BulkAsync(b => b
                            .Index("prop")
                            .IndexMany(properties)
                        );


            return propIndexResponse;
        }
        
        public async Task<string> SearchAsync(SearchQuery searchQuery)
        {
            var query = new BoolQuery();

            query.Must = new QueryContainer[] { new MultiMatchQuery
                {
                    Query = searchQuery.SearchPhrase,
                    Fields = Infer.Field("name").And("formerName").And("streetAddress")
                }
            };

            if(searchQuery.Markets != null && searchQuery.Markets.Length > 0)
            {
                query.Filter = new QueryContainer[] { new MultiMatchQuery
                { 
                    Query =  string.Join(" ", searchQuery.Markets),
                    Fields = Infer.Field("market")
                }};
            }


            var searchResult = await _client.SearchAsync<dynamic>(s => s
                                    .Size(searchQuery.Limit)
                                    .AllIndices()
                                    .Query(c => query)
                                );               


            var resultData = searchResult.Documents?.ToList();

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var jsonData = JsonSerializer.Serialize(resultData, options);


            return jsonData;
        }
    }
}