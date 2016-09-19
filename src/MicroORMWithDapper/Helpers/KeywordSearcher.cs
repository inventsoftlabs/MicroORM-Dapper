namespace MicroORMWithDapper
{
    using System;

    public abstract class KeywordSearcher
    {
        // corrects upto 5 decimal places
        private static int precision = 100000;

        private static Func<DateTime, string> dateTimeString = (dateTime) =>
        {
            return dateTime.Ticks.ToString();
        };

        private static Func<decimal, string> getDecimalValue = (value) =>
        {
            return (value * precision).ToString();
        };

        /// <summary>
        /// Search the instance member based on the keyword.
        /// </summary>
        /// <param name="keyword">keyword to search</param>
        /// <returns>Returns true if matches found.</returns>
        public virtual bool SearchByKeyword(string keyword)
        {
            // if its empty then it matches with everything ;-)
            if (string.IsNullOrEmpty(keyword))
            {
                return true;
            }

            return false;
        }

        public virtual bool SearchByCategory(string category, string keyword)
        {
            // if its empty then it matches with everything ;-)
            if (string.IsNullOrEmpty(keyword))
            {
                return true;
            }

            return false;
        }
    }
}