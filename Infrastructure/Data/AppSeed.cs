using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using Core.Models;
using Nest;
using Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class AppSeed
    {
        private static IElasticClient _client;

        public AppSeed(IElasticConfig _config)
        {
            _client = _config.GetClient();
        }

        public static async Task SeedAsync(ILoggerFactory loggerFactory)
        {
            try
            {
                var mgtData = File.ReadAllText("../Infrastructure/Data/SeedData/mgmt.json");
                var mgts = JsonSerializer.Deserialize<List<MgtObj>>(mgtData);

                var propData = File.ReadAllText("../Infrastructure/Data/SeedData/properties.json");
                var props = JsonSerializer.Deserialize<List<PropertyObject>>(propData);

                //var response = _client.Indices.Create("mgmt2", 
                   // index => index.Map<List<MgtObj>>(x => x.AutoMap()));

                var res = await _client.IndexDocumentAsync(mgts);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<AppSeed>();
                logger.LogError(ex.Message);
            }
        }

    }
}