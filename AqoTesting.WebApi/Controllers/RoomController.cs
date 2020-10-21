using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Rooms;
using AqoTesting.Shared.DTOs.BD.Rooms;
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

        public RoomController(IRoomService roomService) {
            _roomService = roomService;
        }

        [Authorize]
        [HttpGet("/rooms")]
        public async Task<IActionResult> GetRooms() {
            ObjectId ownerId = ObjectId.Parse(User.FindFirst("Id").Value);

            GetRoomsItemDTO[] rooms = await _roomService.GetRoomsByOwnerId(ownerId);

            return this.ResultResponse(OperationErrorMessages.NoError, rooms);
        }

        [Authorize]
        [HttpPost("/room")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDTO newRoom) {
            Room domainTaken = await _roomService.GetRoomByDomain(newRoom.Domain);

            if(domainTaken != null) {
                return this.ResultResponse<object>(OperationErrorMessages.DomainAlreadyTaken);
            }

            ObjectId ownerId = ObjectId.Parse(User.FindFirst("Id").Value);
            ObjectId roomId = await _roomService.InsertRoom(newRoom, ownerId);

            return this.ResultResponse(OperationErrorMessages.NoError, roomId);
        }
    }
}