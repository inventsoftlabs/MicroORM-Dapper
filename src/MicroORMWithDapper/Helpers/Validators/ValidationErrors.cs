namespace MicroORMWithDapper
{
    using System.Collections.Generic;

    /// <summary>
    /// Validation errors.
    /// </summary>
    public class ValidationErrors
    {
        /// <summary>
        /// The _errors
        /// </summary>
        private List<ValidationError> errors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationErrors" /> class.
        /// </summary>
        public ValidationErrors()
        {
            this.errors = new List<ValidationError>();
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public IList<ValidationError> Items
        {
            get { return this.errors; }
        }

        /// <summary>
        /// Adds the specified property name.
        /// </summary>
        /// <param name="propertyName">string type propertyName parameter</param>
        public void Add(string propertyName)
        {
            if (propertyName.IsNotEmpty())
            {
                this.errors.Add(new ValidationError(propertyName, propertyName + " is required."));
            }
        }

        /// <summary>
        /// Adds the specified property name.
        /// </summary>
        /// <param name="propertyName">string type propertyName parameter</param>
        /// <param name="errorMessage">string type errorMessage parameter</param>
        public void Add(string propertyName, string errorMessage)
        {
            if (propertyName.IsNotEmpty())
            {
                this.errors.Add(new ValidationError(propertyName, errorMessage));
            }
        }

        /// <summary>
        /// Adds the specified error.
        /// </summary>
        /// <param name="error">error parameter</param>
        public void Add(ValidationError error)
        {
            this.errors.Add(error);
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="errors">errors parameter</param>
        public void AddRange(IList<ValidationError> errors)
        {
            this.errors.AddRange(errors);
        }

        /// <summary>
        /// Clears the items.
        /// </summary>
        public void Clear()
        {
            this.errors.Clear();
        }
    }
}
