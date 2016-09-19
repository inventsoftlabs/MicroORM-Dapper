namespace Sample.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MicroORMWithDapper;
    using Microsoft.Extensions.Configuration;

    public class AddressInformationModel : BaseModel<AddressInformation>
    {
        public AddressInformationModel(IConfiguration config) : base (config)
        {
        }
    }
}
