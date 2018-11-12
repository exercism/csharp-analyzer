using System;
using Microsoft.AspNetCore.Mvc;

namespace Exercism.Analyzers.CSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyzeController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<string> Get(Guid id)
        {
            return "value";
        }
    }
}
