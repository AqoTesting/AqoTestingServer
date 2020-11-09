using System.Threading.Tasks;
using AqoTesting.Shared.DTOs.API.Members.Rooms;
using AqoTesting.Shared.DTOs.API.Users.Rooms;
using AqoTesting.Shared.Interfaces;
using AutoMapper;

namespace AqoTesting.Core.Services
{
    public class MemberService : ServiceBase, IMemberService
    {
        IRoomRepository _roomRepository;

        public MemberService(IRoomRepository roomRespository)
        {
            _roomRepository = roomRespository;
        }
    }
}