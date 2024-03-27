using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OG.AIFileAnalyzer.Business.Historical;
using OG.AIFileAnalyzer.Common.Entities;

namespace OG.AIFileAnalyzer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricalController(IHistoricalBusiness historicalBusiness) : ControllerBase
    {
        private readonly IHistoricalBusiness _historicalBusiness = historicalBusiness;

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll() 
        {
            var result = await _historicalBusiness.GetHistorical();
            return Ok(result);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] LogEntity log)
        {
            await _historicalBusiness.AddHistorical(log);
            return Ok();
        }
    }
}
