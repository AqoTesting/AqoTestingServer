using System.Collections.Generic;
using System.Threading.Tasks;
using AqoTesting.Domain.Workers;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Interfaces;
using MongoDB.Bson;

namespace AqoTesting.Core.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        Dictionary<ObjectId, RoomsDB_Room_DTO> _internalByIdCache;
        Dictionary<string, ObjectId?> _internalByDomainCache;

        ICacheRepository _redisCache;
        public RoomRepository(ICacheRepository cache)
        {
            _redisCache = cache;
        }

        public async Task<RoomsDB_Room_DTO> GetRoomById(ObjectId roomId)
        {
            RoomsDB_Room_DTO room;

            if(_internalByIdCache.ContainsKey(roomId))
                room = _internalByIdCache[roomId];
            else
            {
                room = await _redisCache.Get($"Room:{roomId}",
                    async () => await RoomWorker.GetRoomById(roomId));

                _internalByIdCache.Add(roomId, room);
            }

            return room;
        }

        public async Task<RoomsDB_Room_DTO> GetRoomByDomain(string roomDomain)
        {
            RoomsDB_Room_DTO room;

            if(!_internalByDomainCache.ContainsKey(roomDomain) || _internalByDomainCache[roomDomain] != null && !_internalByIdCache.ContainsKey(_internalByDomainCache[roomDomain].Value))
            {
                room = await RoomWorker.GetRoomByDomain(roomDomain);

                if(room != null)
                {
                    _internalByDomainCache.TryAdd(roomDomain, room.Id);
                    _internalByIdCache.TryAdd(room.Id, room);
                }
                else
                    _internalByDomainCache.TryAdd(roomDomain, null);
            }
            else
                room = _internalByDomainCache[roomDomain] != null ?
                    _internalByIdCache[_internalByDomainCache[roomDomain].Value] :
                null;

            return room;
        }

        public async Task<RoomsDB_Room_DTO[]> GetRoomsByUserId(ObjectId userId) =>
            await RoomWorker.GetRoomsByUserId(userId);

        public async Task<ObjectId> InsertRoom(RoomsDB_Room_DTO newRoom) =>
            await RoomWorker.InsertRoom(newRoom);

        public async Task ReplaceRoom(RoomsDB_Room_DTO update)
        {
            await _redisCache.Del($"Room:{update.Id}");
            _internalByIdCache.Remove(update.Id);
            _internalByDomainCache.Remove(update.Domain);
            await RoomWorker.ReplaceRoom(update);
        }

        public async Task<bool> SetProperty(ObjectId roomId, string propertyName, object newPropertyValue)
        {
            await _redisCache.Del($"Room:{roomId}");
            _internalByIdCache.Remove(roomId);
            _internalByDomainCache = new Dictionary<string, ObjectId?>();

            return await RoomWorker.SetProperty(roomId, propertyName, newPropertyValue);
        }

        public async Task<bool> SetProperties(ObjectId roomId, Dictionary<string, object> properties)
        {
            await _redisCache.Del($"Room:{roomId}");

            return await RoomWorker.SetProperties(roomId, properties);
        }

        public async Task<bool> DeleteRoomById(ObjectId roomId)
        {
            var response = await RoomWorker.DeleteRoomById(roomId);
            await _redisCache.Del($"Room:{roomId}");

            return response;
        }
    }
}