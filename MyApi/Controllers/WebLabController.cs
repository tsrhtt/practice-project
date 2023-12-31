using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebLabController : ControllerBase
    {
        private readonly ILogger<WebLabController> _logger;
        private readonly IWebLabService _webLabService;

        public WebLabController(ILogger<WebLabController> logger, IWebLabService webLabService)
        {
            _logger = logger;
            _webLabService = webLabService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var labData = await _webLabService.GetLabData();
                return Ok(labData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting lab data");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
