using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface ITestService
    {
        Task<UserAPI_GetTestsItem_DTO[]> GetTestsByRoomId(RoomId_DTO roomIdDTO);

        Task<UserAPI_GetTest_DTO> GetTestById(TestId_DTO testIdDTO);
    }
}
