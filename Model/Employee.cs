using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDEmployee.Model
{
    public class Employee
    {
        public string EmployeeID { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
