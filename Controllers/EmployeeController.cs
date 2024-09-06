using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using CRUDEmployee.Model;
using CRUDEmployee.Services;
using CRUDEmployee.DTO;
using System.Globalization;

namespace CRUDEmployee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private static readonly string DateFormat = "dd-MMM-yy";

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // Get all employee
        [HttpGet]
        public ActionResult<IEnumerable<EmployeeDTO>> GetAllEmployees()
        {
            var employees = _employeeService.GetAllEmployees();
            return Ok(employees);
        }

        // Get employee by ID
        [HttpGet("{id}")]
        public ActionResult<EmployeeDTO> GetEmployee(string id)
        {
            try
            {
                var employee = _employeeService.GetEmployee(id);

                // Jika employee tidak ada, akan melempar KeyNotFoundException
                var employeeDTO = new EmployeeDTO
                {
                    EmployeeID = employee.EmployeeID,
                    FullName = employee.FullName,
                    BirthDate = employee.BirthDate.ToString(DateFormat)
                };

                return Ok(employeeDTO);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Employee not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "An error occurred: " + ex.Message });
            }
        }


        // Add a new employee
        [HttpPost]
        public ActionResult CreateEmployee([FromBody] EmployeeDTO addRequest)
        {
            try
            {
                // Validasi request data
                if (string.IsNullOrWhiteSpace(addRequest.EmployeeID))
                    return BadRequest(new { Message = "EmployeeID is required." });
                if (string.IsNullOrWhiteSpace(addRequest.FullName))
                    return BadRequest(new { Message = "FullName is required." });

                // Parsing data Birthdate dari string menjadi dateTime
                var parsedBirthDate = ParseDate(addRequest.BirthDate);

                // Create a new Employee object
                var newEmployee = new Employee
                {
                    EmployeeID = addRequest.EmployeeID,
                    FullName = addRequest.FullName,
                    BirthDate = parsedBirthDate
                };

                _employeeService.AddEmployee(newEmployee);

                return CreatedAtAction(nameof(GetEmployee), new { id = newEmployee.EmployeeID }, new { Message = "Employee added successfully." });
            }
            catch (FormatException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "An error occurred: " + ex.Message });
            }
        }

        // Update employee
        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(string id, [FromBody] UpdateEmployeeDTO updatedEmployeeRequest)
        {
            if (updatedEmployeeRequest == null)
                return BadRequest(new { Message = "Request body cannot be null." });

            if (string.IsNullOrWhiteSpace(updatedEmployeeRequest.FullName))
                return BadRequest(new { Message = "FullName cannot be null or empty." });

            try
            {
                // Check if employee exists
                var existingEmployee = _employeeService.GetEmployee(id);
                if (existingEmployee == null)
                    return NotFound(new { Message = "Employee not found." });

                // Update data
                existingEmployee.FullName = updatedEmployeeRequest.FullName;
                existingEmployee.BirthDate = !string.IsNullOrEmpty(updatedEmployeeRequest.BirthDate)
                    ? ParseDate(updatedEmployeeRequest.BirthDate)
                    : existingEmployee.BirthDate;

                _employeeService.UpdateEmployee(existingEmployee);

                return Ok(new { Message = "Employee updated successfully." });
            }
            catch (FormatException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "An error occurred: " + ex.Message });
            }
        }


        // Delete employee by ID
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(string id)
        {
            try
            {
                _employeeService.DeleteEmployee(id);
                return Ok(new { Message = "Employee deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "An error occurred: " + ex.Message });
            }
        }

        // Helper method untuk parsing birthDate dari format "dd-MMM-yy"
        private DateTime ParseDate(string dateString)
        {
            if (DateTime.TryParseExact(dateString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate;
            }
            else
            {
                throw new FormatException($"Invalid date format. Please use '{DateFormat}'.");
            }
        }
    }
}
