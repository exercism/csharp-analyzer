using System.Threading.Tasks;
using Exercism.Analyzers.CSharp.Analysis.Analyzers;
using Microsoft.AspNetCore.Mvc;

namespace Exercism.Analyzers.CSharp.Analysis
{
    // TODO: consider including version of CLI in project
    // TODO: consider using internal classes where possible
    // TODO: test using NDependµ
    [Route("api/analyze")]
    [ApiController]
    public class AnalyzeController : ControllerBase
    {
        [HttpGet("{slug}/{uuid}")]
        public async Task<ActionResult<Diagnostic[]>> Analyze(
            [FromRoute]string slug,
            [FromRoute]string uuid,
            [FromServices]Analyzer analyzer) 
            => await analyzer.Analyze(slug, uuid);
    }
}
