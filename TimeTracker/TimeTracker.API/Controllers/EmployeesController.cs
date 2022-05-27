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
    [Route("api/employees")]
    public class EmployeesController : Controller
    {
        private readonly IGenericRepository<Employee> _employeesRepository;

        private readonly ILogger<EmployeesController> _logger;

        private readonly Mapper _mapper = new();

        public EmployeesController(IGenericRepository<Employee> employeesRepository,
                                   ILogger<EmployeesController> logger)
        {
            this._employeesRepository = employeesRepository;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetPage([FromQuery] PageParameters pageParameters)
        {
            var employees = await this._employeesRepository.GetPageAsync(pageParameters);
            var employeeDTOs = this._mapper.Map(employees);

            var metadata = new
            {
                employees.PageSize,
                employees.PageNumber,
                employees.TotalPages,
                employees.HasNextPage,
                employees.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            this._logger.LogInformation($"Returned employees page {employees.PageNumber} from database.");

            return Ok(employeeDTOs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await this._employeesRepository.GetAsync(id, e => e.Sex, e => e.Records);
            if (employee == null)
            {
                this._logger.LogInformation($"Employee with id: {id} was not found in db.");
                return NotFound();
            }
            this._logger.LogInformation($"Returned employee with id: {id}.");

            return employee;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeDTO employeeDTO)
        {
            if (ModelState.IsValid)
            {
                var employee = this._mapper.Map(employeeDTO);
                this._employeesRepository.Attach(employee);
                await this._employeesRepository.AddAsync(employee);

                this._logger.LogInformation($"Created employee with id: {employee.Id}.");

                return CreatedAtAction("GetEmployee", new { Id = employee.Id }, employee);
            }
            this._logger.LogInformation($"EmployeeDTO is not valid.");

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeDTO employeeDTO)
        {
            if (ModelState.IsValid)
            {
                var employee = await this._employeesRepository.GetAsync(id);
                if (employee == null)
                {
                    this._logger.LogInformation($"Employee with id: {id} was not found in db.");
                    return NotFound();
                }

                this._mapper.Map(employeeDTO, employee);
                this._employeesRepository.Attach(employee);
                await this._employeesRepository.UpdateAsync(employee);

                this._logger.LogInformation($"Updated employee with id: {id}.");

                return NoContent();
            }
            this._logger.LogInformation($"EmployeeDTO is not valid.");

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await this._employeesRepository.GetAsync(id);
            if (employee == null)
            {
                this._logger.LogInformation($"Employee with id: {id} was not found in db.");
                return NotFound();
            }

            await this._employeesRepository.DeleteAsync(employee);

            this._logger.LogInformation($"Deleted employee with id: {id}.");

            return NoContent();
        }
    }
}
