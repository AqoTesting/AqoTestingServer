using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.API.Users.Test;
using AqoTesting.Shared.DTOs.API.Users.Tests;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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

        public async Task<GetTestsItemDTO[]> GetTestsByRoomId(ObjectId roomId)
        {
            var room = await _roomRepository.GetRoomById(roomId);

            if (room == null)
                throw new ResultException(OperationErrorMessages.RoomNotFound);

            var tests = await _testRepository.GetTestsByIds(room.TestIds);

            var responseTests = Mapper.Map<GetTestsItemDTO[]>(tests);

            return responseTests;
        }
        public async Task<GetTestsItemDTO[]> GetTestsByRoomId(RoomIdDTO roomIdDTO) =>
            await GetTestsByRoomId(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<GetTestDTO> GetTestById(ObjectId testId)
        {
            var test = await _testRepository.GetTestById(testId);

            if (test == null)
                throw new ResultException(OperationErrorMessages.TestNotFound);

            var responseTest = Mapper.Map<GetTestDTO>(test);

            return responseTest;
        }
        public async Task<GetTestDTO> GetTestById(TestIdDTO testIdDTO) =>
            await GetTestById(ObjectId.Parse(testIdDTO.TestId));
    }
}