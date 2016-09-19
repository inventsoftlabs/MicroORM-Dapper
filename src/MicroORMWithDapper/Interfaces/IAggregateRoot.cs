namespace MicroORMWithDapper
{
    using System;

    public interface IAggregateRoot
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        long Id { get; set; }
    }
}
