using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OG.AIFileAnalyzer.Business.Analyzer;

namespace OG.AIFileAnalyzer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyzerController(IAnalyzerBusiness analyzerBusiness) : ControllerBase
    {
        private readonly IAnalyzerBusiness _analyzerBusiness = analyzerBusiness;

        [HttpPost("Analyze")]
        public async Task<IActionResult> Analyze([FromBody] string base64String)
        {
            var result = await _analyzerBusiness.Analyze(base64String);
            return Ok(result);
        }
    }
}
