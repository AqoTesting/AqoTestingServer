using AqoTesting.Shared.DTOs.API.InternalAPI;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Core.Services
{
    public class ValidationService : ServiceBase, IValidationService
    {
        IRoomRepository _roomRepository;
        ITestRepository _testRepository;

        public ValidationService(IRoomRepository roomRepository, ITestRepository testRepository)
        {
            _roomRepository = roomRepository;
            _testRepository = testRepository;
        }

        public async Task<(OperationErrorMessages, object)> InternalAPI_FileUploadUserAccess(InternalAPI_FileUploadUserAccessDTO fileUploadUserAccessDTO)
        {
            var userId = ObjectId.Parse(fileUploadUserAccessDTO.UserId);
            var roomId = ObjectId.Parse(fileUploadUserAccessDTO.RoomId);
            var testId = ObjectId.Parse(fileUploadUserAccessDTO.TestId);

            var room = await _roomRepository.GetRoomById(roomId);
            if(room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            else if(room.UserId != userId)
                return (OperationErrorMessages.RoomAccessError, null);

            var test = await _testRepository.GetTestById(testId);
            if(test == null)
                return (OperationErrorMessages.TestNotFound, null);

            else if(test.UserId != userId)
                return (OperationErrorMessages.TestAccessError, null);

            return (OperationErrorMessages.NoError, null);
        }
    }
}
