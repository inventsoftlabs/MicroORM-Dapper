namespace MicroORMWithDapper
{
    /// <summary>
    /// Interface which enforces all POCO objects to include is valid instance.
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        ValidationErrors ValidationErrors { get; }
    }
}
