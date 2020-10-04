﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace AqoTesting.DAL
{
    public static class BaseController
    {
        public static string connString;
        public static MySqlConnection connection;
        public static MySqlConnection ConnectToDB(string host, int port, string database, string username, string password)
        {
            // Connection String.
            connString = "Server=" + host + ";Database=" + database + ";port=" + port + ";User Id=" + username + ";password=" + password;

            connection = new MySqlConnection(connString);
            connection.Open();
            return connection;
        }

        public static MySqlCommand CreateQuery(string queryString)
        {
            var query = new MySqlCommand(queryString) { Connection = connection };
            return query;
        }

        public static int ExecuteQuery(string queryString)
        {
            var query = CreateQuery(queryString);
            return query.ExecuteNonQuery();
        }

        public static bool IsTableExist(string tableName)
        {
            var query = CreateQuery($"(Select 1 From information_schema.tables where table_schema = 'aqo' and table_name = '{tableName}') union (select 0) limit 1");
            using (DbDataReader reader = query.ExecuteReader())
            {
                reader.Read();
                return reader.GetBoolean(0);
            }
        }
    }
}