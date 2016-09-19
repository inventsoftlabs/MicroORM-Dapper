namespace MicroORMWithDapper
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using Dapper;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Repository to interact with the database, referenced from
    /// http://www.content ed coder.com/2012/12/creating-data-repository-using-dapper.html
    /// </summary>
    /// <typeparam name="T">represents entity object</typeparam>
    public abstract class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly string tableName;
        private readonly IConfiguration configuration;

        public Repository(string tableName, IConfiguration config)
        {
            this.tableName = tableName;
            this.configuration = config;
        }

        internal DbConnection Connection
        {
            get
            {
                return new SqlConnection(Convert.ToString(this.configuration["Data:ConnectionString"]));
            }
        }

        public virtual IEnumerable<T> GetAll()
        {
            IEnumerable<T> items = null;
            using (DbConnection cn = this.Connection)
            {
                cn.Open();
                items = cn.Query<T>(this.tableName.ConstructInlineQuery<T>());
                items = items.Where(item => item != null && item.IsValid);
            }

            return items;
        }

        public virtual long Add(T item)
        {
            using (DbConnection cn = this.Connection)
            {
                var parameters = (object)this.Mapping(item);
                cn.Open();
                item.Id = cn.Insert<long>(this.tableName, parameters);
                return item.Id;
            }
        }

        public virtual void Update(T item)
        {
            if (item.ValidateConcurrentViolation())
            {
                using (DbConnection cn = this.Connection)
                {
                    item.Modified = DateTime.Now;
                    var parameters = (object)this.Mapping(item);
                    cn.Open();
                    cn.Update(this.tableName, parameters);
                }
            }
        }

        public virtual void Remove(T item)
        {
            if (item.ValidateConcurrentViolation())
            {
                this.Delete(item.Id);
            }
        }

        public virtual void Delete(long id)
        {
            using (DbConnection cn = this.Connection)
            {
                cn.Open();
                cn.Execute("DELETE FROM dbo." + this.tableName + " WHERE Id=@ID", new { ID = id });
            }
        }

        public virtual void DeleteByColumn(long id, string columnName)
        {
            using (DbConnection cn = this.Connection)
            {
                cn.Open();
                var idsToDelete = cn.Query<T>(this.tableName.ConstructInlineQuery<T>(columnName) + "=@ID", new { ID = id }).Select(item => item.Id);
                cn.Execute("DELETE FROM dbo." + this.tableName + " WHERE " + columnName + "=@ID", new { ID = id });
            }
        }

        public virtual void DeleteByIds(long[] ids, string columnName = "Id")
        {
            using (DbConnection cn = this.Connection)
            {
                cn.Open();
                cn.Execute("DELETE FROM dbo." + this.tableName + " WHERE " + columnName + " IN @Ids", new { Ids = ids });
            }
        }

        public virtual T FindById(long id)
        {
            T item = default(T);

            using (DbConnection cn = this.Connection)
            {
                cn.Open();
                item = cn.Query<T>(this.tableName.ConstructInlineQuery<T>("Id") + "=@ID", new { ID = id }).SingleOrDefault();
            }

            return item;
        }

        public virtual IEnumerable<T> FindByQuery(string query)
        {
            IEnumerable<T> items = null;

            using (DbConnection cn = this.Connection)
            {
                cn.Open();
                items = cn.Query<T>(query);
            }

            return items;
        }

        public virtual IEnumerable<T> FindByIds(IEnumerable<long> ids, string columnName = "Id")
        {
            IEnumerable<T> items = null;

            using (DbConnection cn = this.Connection)
            {
                cn.Open();
                items = cn.Query<T>(this.tableName.ConstructInlineQuery<T>(columnName) + " IN @Ids", new { Ids = ids });
            }

            if (items != null && items.Count() > 0 && columnName.Equals("Id"))
            {
                items = items.Where(item => ids.Contains(item.Id));
            }

            return items;
        }

        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> items = null;

            if (items == null || items.Count() == 0)
            {
                // extract the dynamic sql query and parameters from predicate
                QueryResult result = DynamicQuery.GetDynamicQuery<T>(this.tableName, predicate);
                using (DbConnection cn = this.Connection)
                {
                    cn.Open();
                    items = cn.Query<T>(result.Sql, (object)result.Param);
                }
            }

            if (items != null && items.Count() > 0)
            {
                items = items.Where(predicate.Compile());
            }

            return items;
        }

        public virtual PagedList<T> GetAll(
                                        Expression<Func<T, bool>> predicate,
                                        SortExpression<T>[] sortExpressions,
                                        IEnumerable<string> searchColumns,
                                        string searcValue,
                                        int startIndex = 1,
                                        int itemsCount = 10,
                                        bool paging = true)
        {
            IEnumerable<T> items = null;
            int pageCount = default(int);

            // extract the dynamic sql query and parameters from predicate
            QueryResult result = DynamicQuery.GetDynamicPagedQuery<T>(this.tableName, predicate, sortExpressions, searchColumns, searcValue, startIndex, itemsCount, paging);
            QueryResult countResult = DynamicQuery.GetDynamicQuery<T>(this.tableName, predicate, countQuery: true);
            using (DbConnection cn = this.Connection)
            {
                cn.Open();
                items = cn.Query<T>(result.Sql, (object)result.Param);
                pageCount = cn.Query<int>(countResult.Sql, (object)result.Param).SingleOrDefault();
            }

            return new PagedList<T>(items, startIndex, pageCount, itemsCount);
        }

        public virtual PagedList<TOut> ExecPagedViewResult<TOut>(
                                                    string viewName,
                                                    Expression<Func<TOut, bool>> predicate,
                                                    SortExpression<TOut>[] sortExpressions,
                                                    IEnumerable<string> searchColumns,
                                                    string searcValue,
                                                    int startIndex = 1,
                                                    int itemsCount = 10,
                                                    bool paging = true)
        {
            IEnumerable<TOut> items = null;
            int pageCount = default(int);

            // extract the dynamic sql query and parameters from predicate
            QueryResult result = DynamicQuery.GetDynamicPagedQuery<TOut>(viewName, predicate, sortExpressions, searchColumns, searcValue, startIndex, itemsCount, paging);
            QueryResult countResult = DynamicQuery.GetDynamicPagedQuery<TOut>(viewName, predicate, sortExpressions, searchColumns, searcValue, startIndex, itemsCount, paging, true);
            using (DbConnection cn = this.Connection)
            {
                cn.Open();
                items = cn.Query<TOut>(result.Sql, (object)result.Param);
                pageCount = cn.Query<int>(countResult.Sql, (object)result.Param).SingleOrDefault();
            }

            return new PagedList<TOut>(items, startIndex, pageCount, itemsCount);
        }

        public virtual PagedList<TOut> ExecPagedStoredProcedure<TOut>(string procedureName, dynamic item)
        {
            using (DbConnection cn = this.Connection)
            {
                var parameters = (object)item;
                cn.Open();
                var result = cn.Query<TOut>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                var pageCount = result.FirstOrDefault()?.GetPropertyValue("TotalRowsCount");
                return new PagedList<TOut>(result, 0, Convert.ToInt32(pageCount)); // assigning default start Index as params have the index details.
            }
        }

        public virtual IEnumerable<TOut> ExecStoredProcedure<TOut>(string procedureName, dynamic item)
        {
            using (DbConnection cn = this.Connection)
            {
                var parameters = (object)item;
                cn.Open();
                var result = cn.Query<TOut>(procedureName, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public virtual IEnumerable<TOut> ExecViewResult<TOut>(string procedureName, Expression<Func<TOut, bool>> predicate)
        {
            IEnumerable<TOut> items = null;
            QueryResult result = DynamicQuery.GetDynamicQuery<TOut>(procedureName, predicate);
            using (DbConnection cn = this.Connection)
            {
                cn.Open();
                items = cn.Query<TOut>(result.Sql, (object)result.Param);
            }

            return items;
        }

        public virtual TEntity ExecStoredProcedureQueryMultiple<TEntity, TMapper>(string procedureName, dynamic item)
                                                                                                                    where TMapper : IMapper<TEntity>
        {
            using (DbConnection cn = this.Connection)
            {
                var parameters = (object)item;
                cn.Open();
                var result = cn.QueryMultiple(procedureName, parameters, commandType: CommandType.StoredProcedure);
                var mappedResult = Activator.CreateInstance<TMapper>().Map(result);

                return mappedResult;
            }
        }

        internal virtual dynamic Mapping(T item)
        {
            return item;
        }
    }
}