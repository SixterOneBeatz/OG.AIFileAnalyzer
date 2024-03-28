using Microsoft.AspNetCore.Mvc;
using OG.AIFileAnalyzer.Business.Historical;
using OG.AIFileAnalyzer.Common.DTOs;
using OG.AIFileAnalyzer.Common.Entities;

namespace OG.AIFileAnalyzer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricalController(IHistoricalBusiness historicalBusiness) : ControllerBase
    {
        private readonly IHistoricalBusiness _historicalBusiness = historicalBusiness;

        [HttpPost("GetQueryable")]
        public async Task<IActionResult> GetQueryable([FromBody] HistoricalFilterDTO filter)
        {
            var result = await _historicalBusiness.GetHistorical(filter);
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
