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

        private readonly ILogger<ProjectsController> _logger;

        private readonly Mapper _mapper = new();

        public ProjectsController(IGenericRepository<Project> projectsRepository,
                                  ILogger<ProjectsController> logger)
        {
            this._projectsRepository = projectsRepository;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetAllProjects()
        {
            var projects = await this._projectsRepository.GetAllAsync();
            var projectDTOs = this._mapper.Map(projects);

            this._logger.LogInformation("Returned all projects from database.");

            return Ok(projectDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await this._projectsRepository.GetAsync(id, p => p.Records);
            if (project == null)
            {
                this._logger.LogInformation($"Project with id: {id} was not found in db.");
                return NotFound();
            }
            this._logger.LogInformation($"Returned project with id: {id}.");

            return project;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectDTO projectDTO)
        {
            if (ModelState.IsValid)
            {
                var project = this._mapper.Map(projectDTO);
                await this._projectsRepository.AddAsync(project);

                this._logger.LogInformation($"Created project with id: {project.Id}.");

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
                    this._logger.LogInformation($"Project with id: {id} was not found in db.");
                    return NotFound();
                }

                this._mapper.Map(projectDTO, project);
                await this._projectsRepository.UpdateAsync(project);

                this._logger.LogInformation($"Updated project with id: {id}.");

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
                this._logger.LogInformation($"Project with id: {id} was not found in db.");
                return NotFound();
            }

            await this._projectsRepository.DeleteAsync(project);

            this._logger.LogInformation($"Deleted project with id: {id}.");

            return NoContent();
        }
    }
}
