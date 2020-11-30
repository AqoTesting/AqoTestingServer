using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests.Sections;
using AqoTesting.Shared.Enums;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface ITestService
    {
        Task<(OperationErrorMessages, object)> UserAPI_GetTestsByRoomId(ObjectId roomId);
        Task<(OperationErrorMessages, object)> UserAPI_GetTestsByRoomId(CommonAPI_RoomIdDTO roomIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_GetTestById(ObjectId testId);
        Task<(OperationErrorMessages, object)> UserAPI_GetTestById(CommonAPI_TestIdDTO testIdDTO);

        Task<(OperationErrorMessages, object)> UserAPI_CreateTest(CommonAPI_RoomIdDTO roomIdDTO, UserAPI_PostTestDTO postTestDTO);

        Task<(OperationErrorMessages, object)> UserAPI_EditTest(ObjectId testId, UserAPI_PostTestDTO postTestDTO);
        Task<(OperationErrorMessages, object)> UserAPI_EditTest(CommonAPI_TestIdDTO testIdDTO, UserAPI_PostTestDTO postTestDTO);

        Task<(OperationErrorMessages, object)> UserAPI_EditSections(ObjectId testId, UserAPI_PostTestSectionsDTO postSectionsDTO);
        Task<(OperationErrorMessages, object)> UserAPI_EditSections(CommonAPI_TestIdDTO testIdDTO, UserAPI_PostTestSectionsDTO postSectionsDTO);

        Task<(OperationErrorMessages, object)> UserAPI_DeleteTest(ObjectId testId);
        Task<(OperationErrorMessages, object)> UserAPI_DeleteTest(CommonAPI_TestIdDTO testIdDTO);


        Task<(OperationErrorMessages, object)> MemberAPI_GetTestsByRoomId(ObjectId roomId);
        Task<(OperationErrorMessages, object)> MemberAPI_GetTestsByRoomId(CommonAPI_RoomIdDTO roomIdDTO);

        Task<(OperationErrorMessages, object)> MemberAPI_GetTestById(ObjectId testId);
        Task<(OperationErrorMessages, object)> MemberAPI_GetTestById(CommonAPI_TestIdDTO testIdDTO);

        Task<(OperationErrorMessages, object)> MemberAPI_BeginTest(ObjectId testId);
        Task<(OperationErrorMessages, object)> MemberAPI_BeginTest(CommonAPI_TestIdDTO testIdDTO);
    }
}
