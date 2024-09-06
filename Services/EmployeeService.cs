using System;
using System.Collections.Generic;
using System.Linq;
using CRUDEmployee.Model;
using CRUDEmployee.DTO;

namespace CRUDEmployee.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly List<Employee> _employees = new List<Employee>();
        public EmployeeService()
        {
            _employees = new List<Employee>()
            {
                new Employee()
                {
                    EmployeeID = "1001",
                    FullName = "Restu Triwahyuny",
                    BirthDate = new DateTime(2002, 11, 2),
                }
            };
        }

        // Add new employee
        public void AddEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException("Employee cannot null.");

            if (_employees.Any(e => e.EmployeeID == employee.EmployeeID))
                throw new InvalidOperationException("Employee with the same ID already exists.");

            if (string.IsNullOrEmpty(employee.FullName))
                throw new ArgumentException("FullName is required.");

            if (employee.BirthDate == DateTime.MinValue)
                throw new ArgumentException("Valid BirthDate is required.");

            _employees.Add(employee);
        }

        // Get all employees with formatted BirthDate (menggunakan DTO)
        public List<EmployeeDTO> GetAllEmployees()
        {
            return _employees.Select(e => new EmployeeDTO
            {
                EmployeeID = e.EmployeeID,
                FullName = e.FullName,
                BirthDate = e.BirthDate.ToString("dd-MMM-yy")  // format BirthDate
            }).ToList();
        }

        // Get employee by ID
        public Employee GetEmployee(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
                throw new ArgumentException("EmployeeID is required.");

            var employee = _employees.FirstOrDefault(e => e.EmployeeID == employeeId);
            if (employee == null)
                throw new KeyNotFoundException("Employee not found.");

            return employee;
        }

        // Update employee
        public void UpdateEmployee(Employee updatedEmployee)
        {
            if (updatedEmployee == null)
                throw new ArgumentNullException(nameof(updatedEmployee));

            if (string.IsNullOrWhiteSpace(updatedEmployee.EmployeeID))
                throw new ArgumentException("EmployeeID is required.");

            if (string.IsNullOrWhiteSpace(updatedEmployee.FullName))
                throw new ArgumentException("FullName cannot be null or empty.");

            var employee = _employees.FirstOrDefault(e => e.EmployeeID == updatedEmployee.EmployeeID);
            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found.");
            }

            employee.FullName = updatedEmployee.FullName;
            employee.BirthDate = updatedEmployee.BirthDate;
        }



        // Delete employee by ID
        public void DeleteEmployee(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
                throw new ArgumentException("EmployeeID is required.");

            var employee = _employees.FirstOrDefault(e => e.EmployeeID == employeeId);
            if (employee == null)
                throw new KeyNotFoundException("Employee not found.");

            _employees.Remove(employee);
        }
    }
}
