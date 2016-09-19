namespace MicroORMWithDapper
{
    /// <summary>
    /// Validation error.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="propertyName">string type propertyName parameter</param>
        /// <param name="message">string type message parameter</param>
        public ValidationError(string propertyName, string message)
        {
            this.PropertyName = propertyName;
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }
    }
}
