using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.Common;
using AqoTesting.Shared.DTOs.API.MemberAPI.Account;
using AqoTesting.Shared.DTOs.API.UserAPI.Members;
using AqoTesting.Shared.DTOs.DB.Members;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Interfaces;
using AutoMapper;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace AqoTesting.Core.Services
{
    public class MemberService : ServiceBase, IMemberService
    {
        IMemberRepository _memberRepository;
        IRoomRepository _roomRepository;

        public MemberService(IRoomRepository roomRespository, IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
            _roomRepository = roomRespository;
        }

        #region User API
        public async Task<UserAPI_GetMember_DTO> UserAPI_GetMemberById(ObjectId memberId)
        {
            var member = await _memberRepository.GetMemberById(memberId);
            var responseMember = Mapper.Map<UserAPI_GetMember_DTO>(member);

            return responseMember;
        }
        public async Task<UserAPI_GetMember_DTO> UserAPI_GetMemberById(MemberId_DTO memberIdDTO) =>
            await this.UserAPI_GetMemberById(ObjectId.Parse(memberIdDTO.MemberId));

        public async Task<UserAPI_GetMembersItem_DTO> UserAPI_GetMembersByRoomId(ObjectId roomId)
        {
            var members = await _memberRepository.GetMembersByRoomId(roomId);
            var responseMembers = Mapper.Map<UserAPI_GetMembersItem_DTO>(members);

            return responseMembers;
        }
        public async Task<UserAPI_GetMembersItem_DTO> UserAPI_GetMembersByRoomId(RoomId_DTO roomIdDTO) =>
            await this.UserAPI_GetMembersByRoomId(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<(OperationErrorMessages, object)> UserAPI_MemberManualAdd(ObjectId roomId, UserAPI_PostMember_DTO postMemberDTO)
        {
            var room = await _roomRepository.GetRoomById(roomId);

            (var fieldsValid, var errorCode, var response) = FieldsValidator.Validate(room.Fields, postMemberDTO.Fields);
            if(!fieldsValid)
                return (errorCode, response);

            var fieldsHash = FieldsHashGenerator.Generate(postMemberDTO.Fields);
            var alreadyExists = await _memberRepository.CheckFieldsHashExists(roomId, fieldsHash);
            if(alreadyExists)
                return (OperationErrorMessages.FieldsAlreadyExists, null);

            var newMember = Mapper.Map<Member>(postMemberDTO);
            newMember.RoomId = roomId;
            newMember.FieldsHash = fieldsHash;

            var newMemberId = await _memberRepository.InsertMember(newMember);

            return (OperationErrorMessages.NoError, newMemberId.ToString());
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_MemberManualAdd(RoomId_DTO roomIdDTO, UserAPI_PostMember_DTO postMemberDTO) =>
            await this.UserAPI_MemberManualAdd(ObjectId.Parse(roomIdDTO.RoomId), postMemberDTO);
        #endregion

        #region Member API
        public async Task<(OperationErrorMessages, string)> MemberAPI_SignIn(MemberAPI_SignIn_DTO signInDTO)
        {
            var room = await _roomRepository.GetRoomById(ObjectId.Parse(signInDTO.RoomId));
            if(room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            (var valid, var memberId) = await _memberRepository.GetMemberIdByAuthData(signInDTO.Login, Sha256.Compute(signInDTO.Password));

            if(valid)
                return (OperationErrorMessages.NoError, TokenGenerator.GenerateToken(memberId, Role.Member, room.Id, room.IsApproveManually));
            else
                return (OperationErrorMessages.WrongAuthData, null);
        }

        public async Task<MemberAPI_GetProfile_DTO> MemberAPI_GetMemberById(ObjectId memberId)
        {
            var member = await _memberRepository.GetMemberById(memberId);
            var responseMember = Mapper.Map<MemberAPI_GetProfile_DTO>(member);

            return responseMember;
        }
        public async Task<MemberAPI_GetProfile_DTO> MemberAPI_GetMemberById(MemberId_DTO memberIdDTO) =>
            await this.MemberAPI_GetMemberById(ObjectId.Parse(memberIdDTO.MemberId));
        #endregion
    }
}