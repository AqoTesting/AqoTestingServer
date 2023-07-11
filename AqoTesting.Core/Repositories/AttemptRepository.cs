using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using AqoTesting.Shared.DTOs.DB.Attempts;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Domain.Workers;

namespace AqoTesting.Core.Repositories
{
    public class AttemptRepository : IAttemptRepository
    {
        IWorkContext _workContext;
        public AttemptRepository(IWorkContext workContext)
        {
            _workContext = workContext;
        }

        public async Task<AttemptsDB_AttemptDTO> GetAttemptById(ObjectId attemptId) =>
            await AttemptWorker.GetAttemptById(attemptId);

        public async Task<AttemptsDB_AttemptDTO> GetActiveAttemptByMemberId(ObjectId memberId) =>
            await AttemptWorker.GetActiveAttemptByMemberId(memberId);

        public async Task<AttemptsDB_AttemptDTO[]> GetAttemptsByTestId(ObjectId testId) =>
            await AttemptWorker.GetAttemptsByTestId(testId);

        public async Task<AttemptsDB_AttemptDTO[]> GetAttemptsByMemberId(ObjectId memberId) =>
            await AttemptWorker.GetAttemptsByMemberId(memberId);
        

        public async Task<AttemptsDB_AttemptDTO[]> GetAttemptsByTestIdAndMemberId (ObjectId testId, ObjectId memberId) =>
            await AttemptWorker.GetAttemptsByTestIdAndMemberId(testId, memberId);


        public async Task<AttemptsDB_AttemptDTO[]> GetAttemptsByRoomId(ObjectId roomId) =>
            await AttemptWorker.GetAttemptsByRoomId(roomId);

        public async Task<ObjectId> InsertAttempt(AttemptsDB_AttemptDTO newAttempt) =>
            await AttemptWorker.InsertAttempt(newAttempt);

        public async Task<bool> SetProperty(ObjectId attemptId, string propertyName, object newPropertyValue)
        {
            // Не знаю зачем это было написано 3 года назад, страшно
            /*
            if (memberId == null)
                if(_workContext.MemberId != null)
                    memberId = _workContext.MemberId.Value;
                else
                {
                    var attempt = await AttemptWorker.GetAttemptById(attemptId);
                    if(attempt == null)
                        return false;

                    memberId = attempt.MemberId;
                }
            */

            return await AttemptWorker.SetProperty(attemptId, propertyName, newPropertyValue);
        }

        public async Task<bool> SetProperties(ObjectId attemptId, Dictionary<string, object> properties)
        {
            // Не знаю зачем это было написано 3 года назад, страшно
            /*
            if(memberId == null)
                if(_workContext.MemberId != null)
                    memberId = _workContext.MemberId.Value;

                else
                    memberId = (await AttemptWorker.GetAttemptById(attemptId))?.MemberId;

            if(memberId == null)
                return false;
            */

            return await AttemptWorker.SetProperties(attemptId, properties);
        }

        public async Task<bool> Delete(ObjectId attemptId) =>
            await AttemptWorker.DeleteAttempt(attemptId);

        public async Task<long> DeleteAttemptsByMemberId(ObjectId memberId) =>
            await AttemptWorker.DeleteAttemptsByMemberId(memberId);

        public async Task<long> DeleteAttemptsByRoomId(ObjectId roomId) =>
            await AttemptWorker.DeleteAttemptsByRoomId(roomId);
    }
}