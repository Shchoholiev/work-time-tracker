using Microsoft.AspNetCore.Mvc;
using TimeTracker.API.Mapping;
using TimeTracker.Application.DTO;
using TimeTracker.Application.IRepositories;
using TimeTracker.Core.Entities;

namespace TimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectsController : Controller
    {
        private readonly IGenericRepository<Project> _projectsRepository;

        private readonly Mapper _mapper = new();

        public ProjectsController(IGenericRepository<Project> projectsRepository)
        {
            this._projectsRepository = projectsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetAllProjects()
        {
            var projects = await this._projectsRepository.GetAllAsync();
            var projectDTOs = this._mapper.Map(projects);
            return Ok(projectDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await this._projectsRepository.GetAsync(id, p => p.Records);
            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectDTO projectDTO)
        {
            if (ModelState.IsValid)
            {
                var project = this._mapper.Map(projectDTO);
                await this._projectsRepository.AddAsync(project);
                return CreatedAtAction("GetProject", new { Id = project.Id }, project);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProjectDTO projectDTO)
        {
            if (ModelState.IsValid)
            {
                var project = await this._projectsRepository.GetAsync(id);
                if (project == null)
                {
                    return NotFound();
                }

                this._mapper.Map(projectDTO, project);

                await this._projectsRepository.UpdateAsync(project);
                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await this._projectsRepository.GetAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            await this._projectsRepository.DeleteAsync(project);
            return NoContent();
        }
    }
}
