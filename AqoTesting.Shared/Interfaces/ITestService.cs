using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface ITestService
    {
        Task<(OperationErrorMessages, object)> UserAPI_GetTestsByRoomId(ObjectId roomId);
        Task<(OperationErrorMessages, object)> UserAPI_GetTestsByRoomId(RoomId_DTO roomIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_GetTestById(ObjectId testId);
        Task<(OperationErrorMessages, object)> UserAPI_GetTestById(TestId_DTO testIdDTO);


        Task<(OperationErrorMessages, object)> MemberAPI_GetTestsByRoomId(ObjectId roomId);
        Task<(OperationErrorMessages, object)> MemberAPI_GetTestsByRoomId(RoomId_DTO roomIdDTO);

        Task<(OperationErrorMessages, object)> MemberAPI_GetTestById(ObjectId testId);
        Task<(OperationErrorMessages, object)> MemberAPI_GetTestById(TestId_DTO testIdDTO);
    }
}
