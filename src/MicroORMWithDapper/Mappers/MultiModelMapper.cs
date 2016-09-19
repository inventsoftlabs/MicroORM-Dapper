namespace MicroORMWithDapper
{
    using System.Linq;
    using Dapper;

    public class MultiModelMapper : IMapper<Employee>
    {
        public Employee Map(SqlMapper.GridReader reader)
        {
            var employee = reader.Read<Employee>().Where(items => items.Id > 0)?.FirstOrDefault();
            if (employee != null)
            {
                employee.Address = reader.Read<AddressInformation>().Where(item => item.Id > 0);
            }

            return employee;
        }
    }
}
