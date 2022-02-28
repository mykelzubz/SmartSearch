using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> IndexManagement()
        {
            _appService.IndexManagement();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> IndexProperty()
        {
            _appService.IndexProperty();

            return Ok();
        }
        
    }
}