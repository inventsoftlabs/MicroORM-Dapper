namespace MicroORMWithDapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public interface IReadOnlyRepository<T> where T : EntityBase
    {
        /// <summary>
        /// The find by id.
        /// </summary>
        /// <param name="id">identify parameter which needs to be find</param>
        /// <returns>List of founded item</returns>
        T FindById(long id);

        /// <summary>
        /// The find by id.
        /// </summary>
        /// <param name="query">SQL query parameter which needs to be find</param>
        /// <returns>List of founded item</returns>
        IEnumerable<T> FindByQuery(string query);

        /// <summary>
        /// The find an item based on the predicator.
        /// </summary>
        /// <param name="predicate">identify parameter which needs to be find</param>
        /// <returns>List of founded item</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }
}
