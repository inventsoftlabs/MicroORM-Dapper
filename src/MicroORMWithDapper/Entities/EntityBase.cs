namespace MicroORMWithDapper
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The base class for entities.
    /// </summary>
    public class EntityBase : KeywordSearcher, IValidatable, IAggregateRoot
    {
        /// <summary>
        /// The validation errors
        /// </summary>
        private readonly ValidationErrors validationErrors;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBase" /> class.
        /// </summary>
        protected EntityBase()
        {
            this.validationErrors = new ValidationErrors();
        }

        /// <summary>
        /// Gets or sets Entity Id that represents unique value.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets Represents entity modified date.
        /// </summary>
        public DateTime Modified { get; set; }

        /// <summary>
        /// Gets or sets Represents who modified the entity.
        /// </summary>
        public long ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets Pagged Records Count.
        /// </summary>
        [DBIgnore]
        public int TotalRecords { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        [DBIgnore]
        public virtual bool IsValid
        {
            get
            {
                this.validationErrors.Clear();
                this.Validate();
                return this.ValidationErrors.Items.Count == 0;
            }
        }

        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        [DBIgnore]
        public virtual ValidationErrors ValidationErrors
        {
            get { return this.validationErrors; }
        }

        /// <summary>
        /// Search the instance member based on the keyword.
        /// </summary>
        /// <param name="adminApproved">admin approved items search</param>
        /// <returns>Returns true if matches found.</returns>
        public virtual bool SearchByKeyword(bool? adminApproved)
        {
            return true;
        }

        /// <summary>
        /// Search keyword in collection of entities.
        /// </summary>
        /// <typeparam name="T">Entity base parameter</typeparam>
        /// <param name="items">Collection of entities to be searched.</param>
        /// <param name="keyword">Keyword to search throw entities.</param>
        /// <returns>returns true if keyword exist in the entity collection.</returns>
        public virtual bool SearchByKeywordCollection<T>(IEnumerable<T> items, string keyword)
                                                        where T : EntityBase
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (item.IsValid && item.SearchByKeyword(keyword))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Validates this instance.
        /// </summary>
        protected virtual void Validate()
        {
        }
    }
}