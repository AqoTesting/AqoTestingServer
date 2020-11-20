using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface ITestService
    {
        Task<(OperationErrorMessages, object)> UserAPI_GetTestsByRoomId(ObjectId roomId);
        Task<(OperationErrorMessages, object)> UserAPI_GetTestsByRoomId(CommonAPI_RoomId_DTO roomIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_GetTestById(ObjectId testId);
        Task<(OperationErrorMessages, object)> UserAPI_GetTestById(CommonAPI_TestId_DTO testIdDTO);


        Task<(OperationErrorMessages, object)> MemberAPI_GetTestsByRoomId(ObjectId roomId);
        Task<(OperationErrorMessages, object)> MemberAPI_GetTestsByRoomId(CommonAPI_RoomId_DTO roomIdDTO);

        Task<(OperationErrorMessages, object)> MemberAPI_GetTestById(ObjectId testId);
        Task<(OperationErrorMessages, object)> MemberAPI_GetTestById(CommonAPI_TestId_DTO testIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_CreateTest(CommonAPI_RoomId_DTO roomIdDTO, UserAPI_PostTest_DTO postTestDTO);
    }
}
