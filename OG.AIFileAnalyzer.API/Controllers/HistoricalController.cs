using Microsoft.AspNetCore.Mvc;
using OG.AIFileAnalyzer.Business.Historical;
using OG.AIFileAnalyzer.Common.DTOs;
using OG.AIFileAnalyzer.Common.Entities;

namespace OG.AIFileAnalyzer.API.Controllers
{
    /// <summary>
    /// Controller for handling historical data operations.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="HistoricalController"/> class.
    /// </remarks>
    /// <param name="historicalBusiness">The historical business service.</param>
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricalController(IHistoricalBusiness historicalBusiness) : ControllerBase
    {
        private readonly IHistoricalBusiness _historicalBusiness = historicalBusiness;

        /// <summary>
        /// Retrieves historical data based on the specified filter.
        /// </summary>
        /// <param name="filter">The filter to apply to the historical data.</param>
        /// <returns>The action result containing the historical data result.</returns>
        [HttpPost("GetQueryable")]
        public async Task<IActionResult> GetQueryable([FromBody] HistoricalFilterDTO filter)
        {
            var result = await _historicalBusiness.GetHistorical(filter);
            return Ok(result);
        }

        /// <summary>
        /// Adds a log entity to the historical data.
        /// </summary>
        /// <param name="log">The log entity to add.</param>
        /// <returns>The action result indicating the success of the operation.</returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] LogEntity log)
        {
            await _historicalBusiness.AddHistorical(log);
            return Ok();
        }

        /// <summary>
        /// Retrieves the analysis result for the specified hash.
        /// </summary>
        /// <param name="hash">The hash value used to retrieve the analysis result.</param>
        /// <returns>The action result containing the analysis result.</returns>
        [HttpGet("GetAnalysisResult/{hash}")]
        public async Task<IActionResult> GetAnalysisResult(string hash)
        {
            var result = await _historicalBusiness.GetAnalysisResult(hash);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a report containing historical data.
        /// </summary>
        /// <returns>The action result containing the report as a file.</returns>
        [HttpGet("GetReport")]
        public async Task<IActionResult> GetReport()
        {
            var result = await _historicalBusiness.GetReport();
            return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }
}

