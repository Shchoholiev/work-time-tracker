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
    [Route("api/sexes")]
    public class SexesController : Controller
    {
        private readonly IGenericRepository<Sex> _sexesRepository;

        private readonly ILogger<SexesController> _logger;

        private readonly Mapper _mapper = new();

        public SexesController(IGenericRepository<Sex> sexesRepository,
                               ILogger<SexesController> logger)
        {
            this._sexesRepository = sexesRepository;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SexDTO>>> GetPage([FromQuery] PageParameters pageParameters)
        {
            var sexes = await this._sexesRepository.GetPageAsync(pageParameters);
            var sexesDTOs = this._mapper.Map(sexes);

            var metadata = new
            {
                sexes.PageSize,
                sexes.PageNumber,
                sexes.TotalPages,
                sexes.HasNextPage,
                sexes.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            this._logger.LogInformation($"Returned sexes page {sexes.PageNumber} from database.");

            return Ok(sexesDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SexDTO>> GetSex(int id)
        {
            var sex = await this._sexesRepository.GetAsync(id);
            if (sex == null)
            {
                this._logger.LogInformation($"Sex with id: {id} was not found in db.");
                return NotFound();
            }
            var sexDTO = this._mapper.Map(sex);

            this._logger.LogInformation($"Returned sex with id: {id}.");

            return sexDTO;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SexDTO sexDTO)
        {
            if (ModelState.IsValid)
            {
                var sex = this._mapper.Map(sexDTO);
                await this._sexesRepository.AddAsync(sex);

                this._logger.LogInformation($"Created sex with id: {sex.Id}.");

                return CreatedAtAction("GetSex", new { Id = sex.Id }, sex);
            }
            this._logger.LogInformation($"SexDTO is not valid.");

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SexDTO sexDTO)
        {
            if (ModelState.IsValid)
            {
                var sex = await this._sexesRepository.GetAsync(id);
                if (sex == null)
                {
                    this._logger.LogInformation($"Sex with id: {id} was not found in db.");
                    return NotFound();
                }

                this._mapper.Map(sexDTO, sex);
                await this._sexesRepository.UpdateAsync(sex);

                this._logger.LogInformation($"Updated sex with id: {id}.");

                return NoContent();
            }
            this._logger.LogInformation($"SexDTO is not valid.");

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sex = await this._sexesRepository.GetAsync(id);
            if (sex == null)
            {
                this._logger.LogInformation($"Sex with id: {id} was not found in db.");
                return NotFound();
            }

            await this._sexesRepository.DeleteAsync(sex);

            this._logger.LogInformation($"Deleted sex with id: {id}.");

            return NoContent();
        }
    }
}
