namespace MicroORMWithDapper
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The class represents the Employee Address Information.
    /// </summary>
    public class AddressInformation : EntityBase
    {
        /// <summary>
        /// Gets or sets Entity Id that represents Employee Id.
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets Entity that represents Address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets Represents entity Street.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Gets or sets Represents entity City.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets Represents entity State.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets Represents entity Zip code.
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets Represents entity Status.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        protected override void Validate()
        {
            if (string.IsNullOrEmpty(this.Address))
            {
                this.ValidationErrors.Add("Address", "Address is required");
            }
        }
    }
}