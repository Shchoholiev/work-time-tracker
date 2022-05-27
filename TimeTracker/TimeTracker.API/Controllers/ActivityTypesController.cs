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
    [Route("api/activity-types")]
    public class ActivityTypesController : Controller
    {
        private readonly IGenericRepository<ActivityType> _activityTypesRepository;

        private readonly ILogger<ActivityTypesController> _logger;

        private readonly Mapper _mapper = new();

        public ActivityTypesController(IGenericRepository<ActivityType> activityTypesRepository,
                                       ILogger<ActivityTypesController> logger)
        {
            this._activityTypesRepository = activityTypesRepository;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityTypeDTO>>> GetPage([FromQuery] PageParameters pageParameters)
        {
            var roles = await this._activityTypesRepository.GetPageAsync(pageParameters);
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

            this._logger.LogInformation($"Returned activity type page {roles.PageNumber} from database.");

            return Ok(rolesDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityTypeDTO>> GetActivityType(int id)
        {
            var activityType = await this._activityTypesRepository.GetAsync(id);
            if (activityType == null)
            {
                this._logger.LogInformation($"Activity type with id: {id} was not found in db.");
                return NotFound();
            }
            var activityTypeDTO = this._mapper.Map(activityType);

            this._logger.LogInformation($"Returned activity type with id: {id}.");

            return activityTypeDTO;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActivityTypeDTO activityTypeDTO)
        {
            if (ModelState.IsValid)
            {
                var activityType = this._mapper.Map(activityTypeDTO);
                await this._activityTypesRepository.AddAsync(activityType);

                this._logger.LogInformation($"Created activity type with id: {activityType.Id}.");

                return CreatedAtAction("GetActivityType", new { Id = activityType.Id }, activityType);
            }
            this._logger.LogInformation($"ActivityTypeDTO is not valid.");

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActivityTypeDTO activityTypeDTO)
        {
            if (ModelState.IsValid)
            {
                var activityType = await this._activityTypesRepository.GetAsync(id);
                if (activityType == null)
                {
                    this._logger.LogInformation($"Activity type with id: {id} was not found in db.");
                    return NotFound();
                }

                this._mapper.Map(activityTypeDTO, activityType);
                await this._activityTypesRepository.UpdateAsync(activityType);

                this._logger.LogInformation($"Updated activity type with id: {id}.");

                return NoContent();
            }
            this._logger.LogInformation($"ActivityTypeDTO is not valid.");

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var activityType = await this._activityTypesRepository.GetAsync(id);
            if (activityType == null)
            {
                this._logger.LogInformation($"Activity type with id: {id} was not found in db.");
                return NotFound();
            }

            await this._activityTypesRepository.DeleteAsync(activityType);

            this._logger.LogInformation($"Deleted activity type with id: {id}.");

            return NoContent();
        }
    }
}
