using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDEmployee.Model;
using CRUDEmployee.DTO;

namespace CRUDEmployee.Services
{
    public interface IEmployeeService
    {
        void AddEmployee(Employee employee);
        Employee GetEmployee(string employeeId);
        List<EmployeeDTO> GetAllEmployees();
        void UpdateEmployee(Employee updatedEmployee);
        void DeleteEmployee(string employeeId);
    }
}
