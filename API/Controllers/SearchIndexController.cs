using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using System.Text.Json;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchIndexController : ControllerBase
    {
        private IAppService _appService;

        public SearchIndexController(IAppService appService)
        {
            _appService = appService;
        }

        [HttpGet]
        [Route("IndexManagement")]
        public async Task<IActionResult> IndexManagement(IFormFile file)
        {
            if(file == null)
            {
                return BadRequest();
            }

            IEnumerable<Mgmt> mgts;

            using (var stream = file.OpenReadStream())
            using(var streamReader = new StreamReader(stream))
            using (JsonReader reader = new JsonTextReader(streamReader))
            {
                JsonSerializer serializer = new JsonSerializer();

                mgts = serializer.Deserialize<List<Dictionary<string, Mgmt>>>(reader)
                    .SelectMany(x => x.Values);
                    
            }
            
            await _appService.IndexManagementAsync(mgts);

            return Ok();
        }

        [HttpPost]
        [Route("IndexProperty")]
        public async Task<IActionResult> IndexProperty(IFormFile file)
        {
            if(file == null)
            {
                return BadRequest();
            }

            IEnumerable<Property> properties;

            using (var stream = file.OpenReadStream())
            using(var streamReader = new StreamReader(stream))
            using (JsonReader reader = new JsonTextReader(streamReader))
            {
                JsonSerializer serializer = new JsonSerializer();

                properties = serializer.Deserialize<List<Dictionary<string, Property>>>(reader)
                    .SelectMany(x => x.Values);
                    
            }

            await _appService.IndexPropertyAsync(properties);

            return Ok();
        }

        [HttpPost]
        [Route("Search")]
        public async Task<IActionResult> Search([FromBody] SearchQuery searchQuery)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _appService.SearchAsync(searchQuery);

            return Ok(result);
        }
        
    }
}