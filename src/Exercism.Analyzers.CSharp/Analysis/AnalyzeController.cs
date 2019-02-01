using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Solutions;
using Microsoft.AspNetCore.Mvc;

namespace Exercism.Analyzers.CSharp.Analysis
{
    [Route("api/analyze")]
    [ApiController]
    public class AnalyzeController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<AnalysisResult>> Analyze([FromRoute]string id, [FromServices]Analyzer analyzer) =>
            await analyzer.Analyze(id);
    }
}
