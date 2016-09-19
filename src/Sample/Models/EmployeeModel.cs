namespace Sample.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MicroORMWithDapper;
    using Microsoft.Extensions.Configuration;

    public class EmployeeModel : BaseModel<Employee>
    {
        public EmployeeModel(IConfiguration config) : base (config)
        {
        }
    }
}
