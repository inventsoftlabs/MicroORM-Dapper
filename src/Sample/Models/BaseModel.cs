namespace Sample.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MicroORMWithDapper;
    using Microsoft.Extensions.Configuration;

    public class BaseModel<T> : Repository<T> where T : EntityBase
    {
        public BaseModel(IConfiguration config)
                            : base(typeof(T).Name, config)
        {
        }
    }
}
