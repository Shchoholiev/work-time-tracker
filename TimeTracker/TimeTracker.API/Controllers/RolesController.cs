using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TimeTracker.API.Mapping;
using TimeTracker.Application.DTO;
using TimeTracker.Application.IRepositories;
using TimeTracker.Application.Paging;
using TimeTracker.Core.Entities;

namespace TimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RolesController : Controller
    {
        private readonly IGenericRepository<Role> _rolesRepository;

        private readonly ILogger<RolesController> _logger;

        private readonly Mapper _mapper = new();

        public RolesController(IGenericRepository<Role> rolesRepository,
                                   ILogger<RolesController> logger)
        {
            this._rolesRepository = rolesRepository;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetPage([FromQuery] PageParameters pageParameters)
        {
            var roles = await this._rolesRepository.GetPageAsync(pageParameters);
            var rolesDTOs = this._mapper.Map(roles);

            var metadata = new
            {
                roles.PageSize,
                roles.PageNumber,
                roles.TotalPages,
                roles.HasNextPage,
                roles.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            this._logger.LogInformation($"Returned roles page {roles.PageNumber} from database.");

            return Ok(rolesDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await this._rolesRepository.GetAsync(id, e => e.Records);
            if (role == null)
            {
                this._logger.LogInformation($"Role with id: {id} was not found in db.");
                return NotFound();
            }
            this._logger.LogInformation($"Returned role with id: {id}.");

            return role;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleDTO roleDTO)
        {
            if (ModelState.IsValid)
            {
                var role = this._mapper.Map(roleDTO);
                await this._rolesRepository.AddAsync(role);

                this._logger.LogInformation($"Created role with id: {role.Id}.");

                return CreatedAtAction("GetRole", new { Id = role.Id }, role);
            }
            this._logger.LogInformation($"RoleDTO is not valid.");

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoleDTO roleDTO)
        {
            if (ModelState.IsValid)
            {
                var role = await this._rolesRepository.GetAsync(id);
                if (role == null)
                {
                    this._logger.LogInformation($"Role with id: {id} was not found in db.");
                    return NotFound();
                }

                this._mapper.Map(roleDTO, role);
                await this._rolesRepository.UpdateAsync(role);

                this._logger.LogInformation($"Updated role with id: {id}.");

                return NoContent();
            }
            this._logger.LogInformation($"RoleDTO is not valid.");

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await this._rolesRepository.GetAsync(id);
            if (role == null)
            {
                this._logger.LogInformation($"Role with id: {id} was not found in db.");
                return NotFound();
            }

            await this._rolesRepository.DeleteAsync(role);

            this._logger.LogInformation($"Deleted role with id: {id}.");

            return NoContent();
        }
    }
}
