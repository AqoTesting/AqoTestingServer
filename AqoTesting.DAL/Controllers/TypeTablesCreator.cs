using System;
using System.Collections.Generic;
using System.Text;

namespace AqoTesting.DAL.Controllers
{
    class TypeTablesCreator
    {
        public void CreateUsersTable()
        {
            BaseController.ExecuteQuery(@"CREATE TABLE `aqo`.`Users` (
              `Id` INT NOT NULL,
              `Login` TEXT NULL,
              `Name` TEXT NULL,
              `RegistrationDate` DATETIME NULL,
              PRIMARY KEY (`Id`));
            ");
        }

        public void CreateTestsTable()
        {
            BaseController.ExecuteQuery(@"CREATE TABLE `aqo`.`Tests` (
              `Id` INT NOT NULL,
              `Title` TEXT NULL DEFAULT NULL,
              `UserId` INT NULL DEFAULT NULL,
              `CreationDate` DATETIME NULL DEFAULT NULL,
              `ActivationDate` DATETIME NULL DEFAULT NULL,
              `DeactivationDate` DATETIME NULL DEFAULT NULL,
              `Shuffle` TINYINT NULL DEFAULT NULL,
              PRIMARY KEY (`Id`),
              INDEX `UserId` (`UserId` ASC) INVISIBLE);
            ");
        }

        public void CreateSectionsTable()
        {
            BaseController.ExecuteQuery(@"CREATE TABLE `aqo`.`Sections` (
              `Id` INT NOT NULL,
              `TestId` INT NULL,
              PRIMARY KEY (`Id`),
              INDEX `TestId` (`TestId` ASC) INVISIBLE);
            ");
        }

        public void CreateQuestionsTable()
        {
            BaseController.ExecuteQuery(@"CREATE TABLE `aqo`.`Questions` (
              `Id` INT NOT NULL,
              `SectionId` INT NULL,
              `Type` SMALLINT NULL,
              `Text` TEXT NULL,
              `OptionsJson` JSON NULL,
              `Shuffle` TINYINT NULL,
              PRIMARY KEY (`Id`),
              INDEX `SectionId` (`SectionId` ASC) INVISIBLE);
            ");
        }
    }
}
