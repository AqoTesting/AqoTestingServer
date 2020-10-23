using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Rooms;
using AqoTesting.Shared.DTOs.DB.Rooms;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    public class RoomController : Controller
    {
        IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Authorize]
        [HttpGet("/rooms")]
        public async Task<IActionResult> GetRooms()
        {
            ObjectId ownerId = ObjectId.Parse(User.FindFirst("Id").Value);

            GetRoomsItemDTO[] rooms = await _roomService.GetRoomsByOwnerId(ownerId);

            return this.ResultResponse(OperationErrorMessages.NoError, rooms);
        }

        [Authorize]
        [HttpPost("/room")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDTO newRoom)
        {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            Room domainAlreadyTaken = await _roomService.GetRoomByDomain(newRoom.Domain);

            if (domainAlreadyTaken != null)
            {
                return this.ResultResponse<object>(OperationErrorMessages.DomainAlreadyTaken);
            }

            string ownerId = User.FindFirst("Id").Value;
            string roomId = await _roomService.InsertRoom(newRoom, ownerId);

            return this.ResultResponse(OperationErrorMessages.NoError, roomId);
        }

        [Authorize]
        [HttpDelete("/room")]
        public async Task<IActionResult> DeleteRoom([FromBody] DeleteRoomDTO oldRoom)
        {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            await _roomService.DeleteRoomById(oldRoom.Id);

            return this.ResultResponse<object>(OperationErrorMessages.NoError);
        }

        [HttpGet("/roomTest")]
        public async Task<IActionResult> RoomTest()
        {
            Room room = await _roomService.GetRoomByDomain("hall");

            return this.ResultResponse(OperationErrorMessages.NoError, room);
        }
    }
}