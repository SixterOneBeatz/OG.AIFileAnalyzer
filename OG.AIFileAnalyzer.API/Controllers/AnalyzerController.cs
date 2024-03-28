using Microsoft.AspNetCore.Mvc;
using OG.AIFileAnalyzer.Business.Analyzer;

namespace OG.AIFileAnalyzer.API.Controllers
{
    /// <summary>
    /// Controller for handling file analysis operations.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AnalyzerController"/> class.
    /// </remarks>
    /// <param name="analyzerBusiness">The analyzer business service.</param>
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyzerController(IAnalyzerBusiness analyzerBusiness) : ControllerBase
    {
        private readonly IAnalyzerBusiness _analyzerBusiness = analyzerBusiness;

        /// <summary>
        /// Analyzes the provided content asynchronously.
        /// </summary>
        /// <param name="base64String">The content to analyze encoded as a Base64 string.</param>
        /// <returns>The action result containing the analysis response.</returns>
        [HttpPost("Analyze")]
        public async Task<IActionResult> Analyze([FromBody] string base64String)
        {
            var result = await _analyzerBusiness.Analyze(base64String);
            return Ok(result);
        }
    }
}

