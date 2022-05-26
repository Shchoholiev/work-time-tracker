using Microsoft.AspNetCore.Mvc;
using TimeTracker.API.Mapping;
using TimeTracker.Application.DTO;
using TimeTracker.Application.IRepositories;
using TimeTracker.Core.Entities;

namespace TimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/records")]
    public class RecordsController : Controller
    {
        private readonly IRecordsRepository _recordsRepository;

        private readonly Mapper _mapper = new();

        public RecordsController(IRecordsRepository recordsRepository)
        {
            this._recordsRepository = recordsRepository;
        }

        [HttpGet("time-tracked-for-day")]
        public async Task<ActionResult<int>> GetTimeTrackedForDay([FromQuery] int employeeId,
            [FromQuery] int year, [FromQuery] int month, [FromQuery] int day)
        {
            var date = new DateOnly(year, month, day);
            return await this._recordsRepository.GetTrackedTime(employeeId, date);
        }

        [HttpGet("time-tracked-for-week")]
        public async Task<ActionResult<int>> GetTimeTrackedForWeek([FromQuery] int employeeId,
            [FromQuery] int year, [FromQuery] int weekOfYear)
        {
            return await this._recordsRepository.GetTrackedTime(employeeId, year, weekOfYear);
        }

        [HttpGet("for-day")]
        public async Task<ActionResult<IEnumerable<RecordDTO>>> GetRecordsForDay([FromQuery] int projectId,
            [FromQuery] int year, [FromQuery] int month, [FromQuery] int day)
        {
            var date = new DateOnly(year, month, day);
            var records = await this._recordsRepository.GetAllAsync(r => r.Project.Id == projectId
                && r.Date.ToShortDateString() == date.ToShortDateString());
            var recordDTOs = this._mapper.Map(records);
            return Ok(records);
        }

        [HttpGet("for-month")]
        public async Task<ActionResult<IEnumerable<RecordDTO>>> GetRecordsForMonth([FromQuery] int projectId,
            [FromQuery] int year, [FromQuery] int month)
        {
            var records = await this._recordsRepository.GetAllAsync(r => r.Project.Id == projectId
                && r.Date.Year == year && r.Date.Month == month);
            var recordDTOs = this._mapper.Map(records);
            return Ok(records);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Record>> GetRecord(int id)
        {
            var record = await this._recordsRepository.GetAsync(id);
            if (record == null)
            {
                return NotFound();
            }

            return record;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecordDTO recordDTO)
        {
            if (ModelState.IsValid)
            {
                var record = this._mapper.Map(recordDTO);
                await this._recordsRepository.AddAsync(record);
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
                    return NotFound();
                }

                this._mapper.Map(recordDTO, record);

                await this._recordsRepository.UpdateAsync(record);
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
                return NotFound();
            }

            await this._recordsRepository.DeleteAsync(record);
            return NoContent();
        }
    }
}
