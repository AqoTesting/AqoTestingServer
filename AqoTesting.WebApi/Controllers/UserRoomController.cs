using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.DTOs.DB.Users.Rooms;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AqoTesting.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace AqoTesting.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("/user/room")]
    public class UserRoomController : Controller
    {
        IRoomService _roomService;

        public UserRoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }


        [HttpGet("/{roomId}")]
        public async Task<IActionResult> GetRoom()
        {
            Room room = await _roomService.GetRoomByDomain("hall");

            return this.ResultResponse(OperationErrorMessages.NoError, room);
        }

        [Authorize]
        [HttpGet("s")]
        public async Task<IActionResult> GetRooms()
        {
            ObjectId ownerId = ObjectId.Parse(User.FindFirst("Id").Value);

            GetRoomsItemDTO[] rooms = await _roomService.GetRoomsByOwnerId(ownerId);

            return this.ResultResponse(OperationErrorMessages.NoError, rooms);
        }

        [Authorize]
        [HttpPost]
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
        [HttpDelete]
        public async Task<IActionResult> DeleteRoom([FromBody] RoomIdDTO oldRoom)
        {
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            await _roomService.DeleteRoomById(oldRoom.Id);

            return this.ResultResponse<object>(OperationErrorMessages.NoError);
        }
    }
}