using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.DB.Tests;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using MongoDB.Bson;

namespace AqoTesting.Shared.Interfaces
{
    public interface ITestRepository
    {
        Task<Test[]> GetTestsByIds(ObjectId[] testIds);

        Task<Test> GetTestById(ObjectId testId);

        //Task SetTestTitle(ObjectId testId, string newTitle);
        //Task SetTestIsActive(ObjectId testId, bool newIsActive);
        //Task SetTestSections(ObjectId testId, Section[] newSections);
        //Task SetTestCreationDate(ObjectId testId, DateTime newCreationDate);
        //Task SetTestActivationDate(ObjectId testId, DateTime newActivationDate);
        //Task SetTestDeactivationDate(ObjectId testId, DateTime newDeactivationDate);

        //Task<bool> DeleteTestById(ObjectId testId);
    }
}
