using Microsoft.AspNetCore.Mvc;
using NetCrudStarter.OrchestratorModule.Service;
using System.Threading.Tasks;

namespace NetCrudStarter.StudentModule.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrchestratorController : ControllerBase
    {
        private readonly OrchestratorService _orchestratorService;

        public OrchestratorController(OrchestratorService orchestratorService)
        {
            _orchestratorService = orchestratorService;
        }

        [HttpGet("courseWithStudents/{courseId}")]
        public async Task<IActionResult> GetCourseWithStudents(int courseId)
        {
            var result = await _orchestratorService.GetCourseWithStudentsAsync(courseId);
            return Ok(result);
        }
    }
}
