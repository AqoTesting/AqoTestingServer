using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface ITestService
    {
        Task<UserAPI_GetTestsItem_DTO[]> UserAPI_GetTestsByRoomId(RoomId_DTO roomIdDTO);

        Task<UserAPI_GetTest_DTO> UserAPI_GetTestById(TestId_DTO testIdDTO);
    }
}
