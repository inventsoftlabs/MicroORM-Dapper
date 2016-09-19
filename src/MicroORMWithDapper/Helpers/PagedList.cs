namespace MicroORMWithDapper
{
    using System;
    using System.Collections.Generic;

    public class PagedList<T> : List<T>
    {
        public PagedList(IEnumerable<T> list, int pageNumber, int itemCount, int pageSize = 10)
        {
            this.AddRange(list);

            this.PageNumber = pageNumber;
            this.PageCount = (pageSize > 0 && itemCount > 0) ? (int)Math.Ceiling((decimal)itemCount / pageSize) : 0; // todo
            this.RecordsCount = itemCount;
        }

        public int RecordsCount { get; set; }

        public int PageNumber { get; set; }

        public int PageCount { get; set; }

        public bool IsFirstPage
        {
            get { return this.PageNumber == 1; }
        }

        public bool HasPreviousPage
        {
            get { return this.PageNumber > 1; }
        }

        public bool HasNextPage
        {
            get { return this.PageNumber < this.PageCount; }
        }

        public bool IsLastPage
        {
            get { return this.PageCount == this.PageCount; }
        }
    }
}
