namespace MicroORMWithDapper
{
    using System;

    /// <summary>
    /// The repository interface.
    /// </summary>
    /// <typeparam name="T">type of value to be processed</typeparam>
    public interface IRepository<T> : IReadOnlyRepository<T> where T : EntityBase
    {
        /// <summary>
        /// The Add
        /// </summary>
        /// <param name="item">item parameter which needs to be added</param>
        /// <returns>item parameter</returns>
        long Add(T item);

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">item parameter which needs to be remove</param>
        void Remove(T item);

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="item">item parameter which needs to be updated</param>
        void Update(T item);

        /// <summary>
        /// Deletes the record by the ID
        /// </summary>
        /// <param name="id">identity of item which needs to be deleted</param>
        void Delete(long id);
    }
}