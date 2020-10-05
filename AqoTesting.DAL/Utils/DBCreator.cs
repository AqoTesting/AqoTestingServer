using AqoTesting.DAL.Controllers;
using AqoTesting.DTOs.BDModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.DAL.Utils
{
    public class DBCreator
    {
        public void Init()
        {
            BaseController.ConnectToDB("localhost", 3306, "aqo", "aqoUser", "aqoUserPassword");
            CheckTables();
        }

        public void CheckTables()
        {
            string[] DBStoredTypes = { "Users", "Tests", "Sections", "Questions" };
            var typeTablesCreator = new TypeTablesCreator();
            foreach (var type in DBStoredTypes)
                if (!BaseController.IsTableExist(type))
                    if (type == "Users") {
                        typeTablesCreator.CreateUsersTable();
                    } else if (type == "Tests") {
                        typeTablesCreator.CreateTestsTable();
                    } else if (type == "Sections") {
                        typeTablesCreator.CreateSectionsTable();
                    } else if (type == "Questions")
                        typeTablesCreator.CreateQuestionsTable();
        }
    }
}
