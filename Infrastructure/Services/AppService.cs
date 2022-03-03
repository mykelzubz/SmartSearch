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
            
            _client.Indices.Create("mgmt", c => c
                            .Settings(s => s
                                .Analysis(a => a
                                    .Analyzers(az => az
                                        .Standard("standard_english", sa => sa
                                            .StopWords("_english_") 
                                        )
                                    ).Tokenizers(tk => tk
                                        .EdgeNGram("autocomplete", e => e
                                            .MinGram(2)
                                            .MaxGram(25)
                                            .TokenChars(TokenChar.Letter, TokenChar.Digit)
                                        )
                                    )
                                )
                            )
                            .Map<Mgmt>(mm => mm
                                .Properties(p => p
                                    .Text(t => t
                                        .Name(n => n.name)
                                        .Analyzer("standard_english")
                                        .Analyzer("standard_english")
                                    )
                                )
                            )
                );

            var mgmtIndexResponse = await _client.BulkAsync(b => b
                .Index("mgmt")
                .IndexMany(mgts)
            );


            return mgmtIndexResponse;

        }
        
        public async Task<BulkResponse> IndexPropertyAsync(IEnumerable<Property> properties)
        {

            if(_client.Indices.Exists("prop").Exists)
            {
                _client.Indices.Delete("prop");
            }

            _client.Indices.Create("prop", c => c
                            .Settings(s => s
                                .Analysis(a => a
                                    .Analyzers(aa => aa
                                        .Standard("standard_english", sa => sa
                                            .StopWords("_english_") 
                                        )
                                    ).Tokenizers(tk => tk
                                        .EdgeNGram("autocomplete", e => e
                                            .MinGram(2)
                                            .MaxGram(25)
                                            .TokenChars(TokenChar.Letter, TokenChar.Digit)
                                        )
                                    )
                                )
                            )
                            .Map<Property>(mm => mm
                                .Properties(p => p
                                    .Text(t => t
                                        .Name(n => n.name)
                                        .Analyzer("standard_english")
                                        .Analyzer("autocomplete")
                                    )
                                    .Text(t => t
                                        .Name(n => n.formerName)
                                        .Analyzer("standard_english")
                                        .Analyzer("autocomplete")
                                    )
                                    .Text(t => t
                                        .Name(n => n.streetAddress)
                                        .Analyzer("standard_english")
                                        .Analyzer("autocomplete")
                                    )
                                )
                            )
                );

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