using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Exercism.Analyzers.CSharp.Analysis
{
    // TODO: analyze using NDepend
    // TODO: performance analysis
    // TODO: remove Syntax postfix
    [Route("api/analyze")]
    [ApiController]
    public class AnalyzeController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<string[]>> Analyze([FromRoute]string id, [FromServices]Analyzer analyzer) 
            => await analyzer.Analyze(id).ConfigureAwait(false);
    }
}
