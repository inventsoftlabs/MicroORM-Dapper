namespace MicroORMWithDapper
{
    using System;

    /// <summary>
    /// The class represents the DB Ignore attribute
    /// </summary>
    public class DBIgnoreAttribute : Attribute
    {
        public DBIgnoreAttribute()
        {
            this.IgnoreMember = true;
        }

        public bool IgnoreMember { get; set; }
    }
}
