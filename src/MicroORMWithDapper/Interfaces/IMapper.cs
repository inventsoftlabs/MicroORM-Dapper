namespace MicroORMWithDapper
{
    using Dapper;

    /// <summary>
    /// Interface to map multiple result set to single object.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IMapper<out TEntity>
    {
        TEntity Map(SqlMapper.GridReader reader);
    }
}
