namespace MicroORMWithDapper
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The Employee Information entities class.
    /// </summary>
    public class Employee : EntityBase
    {
        /// <summary>
        /// Gets or sets Represents entity Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Represents entity Designation.
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// Gets or sets Represents entity status.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or Sets represents Address information.
        /// </summary>
        [DBIgnore]
        public IEnumerable<AddressInformation> Address { get; set; }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        protected override void Validate()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                this.ValidationErrors.Add("Name", "Name is required");
            }
        }
    }
}