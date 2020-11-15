using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.UserAPI.Tests;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AutoMapper;
using MongoDB.Bson;

namespace AqoTesting.Core.Services
{
    public class TestService : ServiceBase, ITestService
    {
        IRoomRepository _roomRepository;
        ITestRepository _testRepository;
        IWorkContext _workContext;

        public TestService(IRoomRepository roomRespository, ITestRepository testRepository, IWorkContext workContext)
        {
            _roomRepository = roomRespository;
            _testRepository = testRepository;
            _workContext = workContext;
        }

        public async Task<UserAPI_GetTestsItem_DTO[]> GetTestsByRoomId(ObjectId roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);

            if(room == null)
                throw new ResultException(OperationErrorMessages.RoomNotFound);

            var tests = await _testRepository.GetTestsByRoomId(room.Id);

            var responseTests = Mapper.Map<UserAPI_GetTestsItem_DTO[]>(tests);

            return responseTests;
        }
        public async Task<UserAPI_GetTestsItem_DTO[]> GetTestsByRoomId(RoomId_DTO roomIdDTO) =>
            await GetTestsByRoomId(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<UserAPI_GetTest_DTO> GetTestById(ObjectId testId)
        {
            var test = await _testRepository.GetTestById(testId);

            if(test == null)
                throw new ResultException(OperationErrorMessages.TestNotFound);

            var responseTest = Mapper.Map<UserAPI_GetTest_DTO>(test);

            return responseTest;
        }
        public async Task<UserAPI_GetTest_DTO> GetTestById(TestId_DTO testIdDTO) =>
            await GetTestById(ObjectId.Parse(testIdDTO.TestId));
    }
}