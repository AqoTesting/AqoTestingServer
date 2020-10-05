using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AqoTesting.DAL.Utils
{
    public static class DataReaderExtensions
    {
        #region GetStringOrNull
        public static string GetStringOrNull(this IDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }

        public static string GetStringOrNull(this IDataReader reader, string columnName)
        {
            return reader.GetStringOrNull(reader.GetOrdinal(columnName));
        }
        #endregion

        #region GetDateTimeOrNull
        public static DateTime? GetDateTimeOrNull(this IDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? (DateTime?)null : reader.GetDateTime(ordinal);
        }

        public static DateTime? GetDateTimeOrNull(this IDataReader reader, string columnName)
        {
            return reader.GetDateTimeOrNull(reader.GetOrdinal(columnName));
        }
        #endregion
    }
}
