using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDEmployee.DTO
{
    public class EmployeeDTO
    {
        public string EmployeeID { get; set; }
        public string FullName { get; set; }

        // Format string untuk request dan response body
        public string BirthDate { get; set; }
    }
}
