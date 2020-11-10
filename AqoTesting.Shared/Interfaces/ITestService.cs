using AqoTesting.Shared.DTOs.API;
using AqoTesting.Shared.DTOs.API.Users.Test;
using AqoTesting.Shared.DTOs.API.Users.Tests;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface ITestService
    {
        Task<GetUserTestsItemDTO[]> GetTestsByRoomId(RoomIdDTO roomIdDTO);

        Task<GetUserTestDTO> GetTestById(TestIdDTO testIdDTO);
    }
}
