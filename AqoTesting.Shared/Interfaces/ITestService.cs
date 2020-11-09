using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.API.Users.Test;
using AqoTesting.Shared.DTOs.API.Users.Tests;
using System.Threading.Tasks;

namespace AqoTesting.Shared.Interfaces
{
    public interface ITestService
    {
        Task<GetTestsItemDTO[]> GetTestsByRoomId(UserRoomIdDTO roomIdDTO);

        Task<GetTestDTO> GetTestById(TestIdDTO testIdDTO);
    }
}
