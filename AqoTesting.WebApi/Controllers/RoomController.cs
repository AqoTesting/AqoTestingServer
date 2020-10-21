using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            if (!ModelState.IsValid) return this.ResultResponse(OperationErrorMessages.InvalidModel, ModelState);

            ObjectId ownerId = ObjectId.Parse(User.FindFirst("Id").Value);

            Room[] rooms = await _roomService.GetRoomsByOwnerId(ownerId);

            return this.ResultResponse(OperationErrorMessages.NoError, rooms);
        }
    }
}