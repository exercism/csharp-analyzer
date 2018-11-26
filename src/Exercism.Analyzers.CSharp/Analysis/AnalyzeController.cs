using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Analyzers;
using Microsoft.AspNetCore.Mvc;

namespace Exercism.Analyzers.CSharp.Analysis
{
    // TODO: analyze using NDepend
    // TODO: performance analysis
    [Route("api/analyze")]
    [ApiController]
    public class AnalyzeController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<Diagnostic[]>> Analyze([FromRoute]string id, [FromServices]Analyzer analyzer) 
            => await analyzer.Analyze(id);
    }
}
