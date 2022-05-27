using Microsoft.AspNetCore.Mvc;
using TimeTracker.API.Mapping;
using TimeTracker.Application.DTO;
using TimeTracker.Application.DTO.Create;
using TimeTracker.Application.IRepositories;
using TimeTracker.Core.Entities;

namespace TimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/records")]
    public class RecordsController : Controller
    {
        private readonly IRecordsRepository _recordsRepository;

        private readonly ILogger<ProjectsController> _logger;

        private readonly Mapper _mapper = new();

        public RecordsController(IRecordsRepository recordsRepository, ILogger<ProjectsController> logger)
        {
            this._recordsRepository = recordsRepository;
            this._logger = logger;
        }

        [HttpGet("time-tracked-for-day")]
        public async Task<ActionResult<int>> GetTimeTrackedForDay([FromQuery] int employeeId,
            [FromQuery] int year, [FromQuery] int month, [FromQuery] int day)
        {
            var date = new DateOnly(year, month, day);
            var time = await this._recordsRepository.GetTrackedTime(employeeId, date);

            this._logger.LogInformation($"Returned time tracked for employee with id: {employeeId} on {date}.");

            return time;
        }

        [HttpGet("time-tracked-for-week")]
        public async Task<ActionResult<int>> GetTimeTrackedForWeek([FromQuery] int employeeId,
            [FromQuery] int year, [FromQuery] int weekOfYear)
        {
            var time = await this._recordsRepository.GetTrackedTime(employeeId, year, weekOfYear);

            this._logger.LogInformation($"Returned time tracked for employee with id: {employeeId} during " +
                                        $"{weekOfYear} week of {year}.");

            return time;
        }

        [HttpGet("for-day")]
        public async Task<ActionResult<IEnumerable<RecordDTO>>> GetRecordsForDay([FromQuery] int projectId,
            [FromQuery] int year, [FromQuery] int month, [FromQuery] int day)
        {
            var date = new DateOnly(year, month, day);
            var records = await this._recordsRepository.GetAllAsync(r => r.Project.Id == projectId
                && r.Date.ToShortDateString() == date.ToShortDateString());
            var recordDTOs = this._mapper.Map(records);

            this._logger.LogInformation($"Returned all records for project with id: {projectId} on {date}.");

            return Ok(records);
        }

        [HttpGet("for-month")]
        public async Task<ActionResult<IEnumerable<RecordDTO>>> GetRecordsForMonth([FromQuery] int projectId,
            [FromQuery] int year, [FromQuery] int month)
        {
            var records = await this._recordsRepository.GetAllAsync(r => r.Project.Id == projectId
                && r.Date.Year == year && r.Date.Month == month);
            var recordDTOs = this._mapper.Map(records);

            this._logger.LogInformation($"Returned all records for project with id: {projectId} " +
                                        $"during {month}/{year}.");

            return Ok(records);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Record>> GetRecord(int id)
        {
            var record = await this._recordsRepository.GetAsync(id);
            if (record == null)
            {
                this._logger.LogInformation($"Record with id: {id} was not found in db.");
                return NotFound();
            }
            this._logger.LogInformation($"Returned record with id: {id}.");

            return record;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecordCreateDTO recordDTO)
        {
            if (ModelState.IsValid)
            {
                var timeTracked = await this.GetTimeTrackedForDay(recordDTO.Employee.Id, recordDTO.Year,
                                                                  recordDTO.Month, recordDTO.Day);
                if (timeTracked.Value + recordDTO.HoursWorked > 24)
                {
                    throw new ArgumentOutOfRangeException("HoursWorked", 
                        "You can't work more than 24 hours a day.");
                }

                var record = this._mapper.Map(recordDTO);
                await this._recordsRepository.AddAsync(record);

                this._logger.LogInformation($"Created record with id: {record.Id}.");

                return CreatedAtAction("GetRecord", new { Id = record.Id }, record);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RecordDTO recordDTO)
        {
            if (ModelState.IsValid)
            {
                var record = await this._recordsRepository.GetAsync(id);
                if (record == null)
                {
                    this._logger.LogInformation($"Record with id: {id} was not found in db.");
                    return NotFound();
                }

                this._mapper.Map(recordDTO, record);
                await this._recordsRepository.UpdateAsync(record);

                this._logger.LogInformation($"Updated record with id: {id}.");

                return NoContent();
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await this._recordsRepository.GetAsync(id);
            if (record == null)
            {
                this._logger.LogInformation($"Record with id: {id} was not found in db.");
                return NotFound();
            }

            await this._recordsRepository.DeleteAsync(record);

            this._logger.LogInformation($"Deleted record with id: {id}.");

            return NoContent();
        }
    }
}
