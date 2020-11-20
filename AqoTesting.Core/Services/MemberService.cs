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
        IWorkContext _workContext;
        ITokenGeneratorService _tokenGeneratorService;

        public MemberService(IRoomRepository roomRespository, IMemberRepository memberRepository, IWorkContext workContext, ITokenGeneratorService tokenGeneratorService)
        {
            _memberRepository = memberRepository;
            _roomRepository = roomRespository;
            _workContext = workContext;
            _tokenGeneratorService = tokenGeneratorService;
        }

        #region User API
        public async Task<(OperationErrorMessages, object)> UserAPI_GetMemberById(ObjectId memberId)
        {
            var member = await _memberRepository.GetMemberById(memberId);
            if(member == null)
                return (OperationErrorMessages.MemberNotFound, null);

            var getMemberDTO = Mapper.Map<UserAPI_GetMember_DTO>(member);

            return (OperationErrorMessages.NoError, getMemberDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetMemberById(CommonAPI_MemberId_DTO memberIdDTO) =>
            await this.UserAPI_GetMemberById(ObjectId.Parse(memberIdDTO.MemberId));

        public async Task<(OperationErrorMessages, object)> UserAPI_GetMembersByRoomId(ObjectId roomId)
        {
            var roomExists = await _roomRepository.GetRoomById(roomId);
            if(roomExists == null)
                return (OperationErrorMessages.RoomNotFound, null);

            var members = await _memberRepository.GetMembersByRoomId(roomId);
            var getMembersItemDTOs = Mapper.Map<UserAPI_GetMembersItem_DTO[]>(members);

            return (OperationErrorMessages.NoError, getMembersItemDTOs);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetMembersByRoomId(CommonAPI_RoomId_DTO roomIdDTO) =>
            await this.UserAPI_GetMembersByRoomId(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<(OperationErrorMessages, object)> UserAPI_ManualMemberAdd(ObjectId roomId, UserAPI_PostMember_DTO postMemberDTO)
        {
            var room = await _roomRepository.GetRoomById(roomId);
            if(room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            // Нельзя добавить потому что включена самостоятельная регистрация
            if (room.IsRegistrationEnabled)
                return (OperationErrorMessages.RoomRegistrationEnabled, null);

            // Fields validation
            (var fieldsValid, var errorCode, var response) = FieldsValidator.Validate(room.Fields, postMemberDTO.Fields);
            if(!fieldsValid)
                return (errorCode, response);

            // Check fields hash exists
            var fieldsHash = FieldsHashGenerator.Generate(postMemberDTO.Fields);
            var alreadyExists = await _memberRepository.GetMemberByFieldsHash(roomId, fieldsHash);
            if(alreadyExists != null)
                return (OperationErrorMessages.FieldsAlreadyExists, null);

            var newMember = Mapper.Map<MembersDB_Member_DTO>(postMemberDTO);
            newMember.IsApproved = !room.IsApproveManually;
            newMember.OwnerId = room.OwnerId;
            newMember.RoomId = roomId;
            newMember.FieldsHash = fieldsHash;

            var newMemberId = await _memberRepository.InsertMember(newMember);
            var newMemberIdDTO = new CommonAPI_MemberId_DTO { MemberId = newMemberId.ToString() };

            return (OperationErrorMessages.NoError, newMemberIdDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_ManualMemberAdd(CommonAPI_RoomId_DTO roomIdDTO, UserAPI_PostMember_DTO postMemberDTO) =>
            await this.UserAPI_ManualMemberAdd(ObjectId.Parse(roomIdDTO.RoomId), postMemberDTO);

        public async Task<(OperationErrorMessages, object)> UserAPI_Unregister(ObjectId memberId)
        {
            var member = await _memberRepository.GetMemberById(memberId);
            if (member == null)
                return (OperationErrorMessages.MemberNotFound, null);

            if (member.OwnerId != _workContext.UserId)
                return (OperationErrorMessages.MemberAccessError, null);

            var unregistered = await _memberRepository.SetIsRegistered(memberId, false);
            if (!unregistered)
                return (OperationErrorMessages.MemberIsNotRegistered, null);

            return (OperationErrorMessages.NoError, null);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_Unregister(CommonAPI_MemberId_DTO memberIdDTO) =>
            await this.UserAPI_Unregister(ObjectId.Parse(memberIdDTO.MemberId));

        public async Task<(OperationErrorMessages, object)> UserAPI_Approve(ObjectId memberId)
        {
            var member = await _memberRepository.GetMemberById(memberId);
            if (member == null)
                return (OperationErrorMessages.MemberNotFound, null);

            if (member.OwnerId != _workContext.UserId)
                return (OperationErrorMessages.MemberAccessError, null);

            var changed = await _memberRepository.SetIsApproved(memberId, true);
            if (!changed)
                return (OperationErrorMessages.MemberIsApproved, null);

            return (OperationErrorMessages.NoError, null);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_Approve(CommonAPI_MemberId_DTO memberIdDTO) =>
            await this.UserAPI_Approve(ObjectId.Parse(memberIdDTO.MemberId));
        #endregion

        #region Member API
        public async Task<(OperationErrorMessages, object)> MemberAPI_SignIn(MemberAPI_SignIn_DTO signInDTO)
        {
            var room = await _roomRepository.GetRoomById(ObjectId.Parse(signInDTO.RoomId));
            if(room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            var member = await _memberRepository.GetMemberByAuthData(ObjectId.Parse(signInDTO.RoomId), signInDTO.Login, Sha256.Compute(signInDTO.Password));

            if(member == null || !member.IsRegistered)
                return (OperationErrorMessages.WrongAuthData, null);

            var memberToken = _tokenGeneratorService.GenerateToken(member.Id, Role.Member, room.Id, member.IsApproved);
            var memberTokenDTO = new CommonAPI_Token_DTO { Token = memberToken };

            return (OperationErrorMessages.NoError, memberTokenDTO);
        }

        public async Task<(OperationErrorMessages, object)> MemberAPI_SignUp(MemberAPI_SignUp_DTO signUpDTO)
        {
            var roomId = ObjectId.Parse(signUpDTO.RoomId);

            var room = await _roomRepository.GetRoomById(roomId);
            if(room == null)
                return (OperationErrorMessages.RoomNotFound, null);
            
            // Fields validation
            (var fieldsValid, var errorCode, var response) = FieldsValidator.Validate(room.Fields, signUpDTO.Fields);
            if(!fieldsValid)
                return (errorCode, response);

            // Check login or email taken
            var loginTaken = await _memberRepository.CheckLoginTaken(roomId ,signUpDTO.Login);
            if(loginTaken)
                return (OperationErrorMessages.LoginAlreadyTaken, null);
            var emailTaken = await _memberRepository.CheckEmailTaken(roomId, signUpDTO.Email);
            if(emailTaken)
                return (OperationErrorMessages.EmailAlreadyTaken, null);

            // Check fields hash exists
            var fieldsHash = FieldsHashGenerator.Generate(signUpDTO.Fields);
            var member = await _memberRepository.GetMemberByFieldsHash(roomId, fieldsHash);
            
            ObjectId memberId;

            if (room.IsRegistrationEnabled)
            {
                if(member != null)
                    return (OperationErrorMessages.MemberAlreadyExists, null);

                member = Mapper.Map<MembersDB_Member_DTO>(signUpDTO);
                member.PasswordHash = Sha256.Compute(signUpDTO.Password);
                member.OwnerId = room.OwnerId;

                member.IsRegistered = true;
                member.IsApproved = !room.IsApproveManually;

                member.FieldsHash = fieldsHash;

                memberId = await _memberRepository.InsertMember(member);
            }
            else
            {
                if (member == null)
                    return (OperationErrorMessages.MemberNotFound, null);

                member.Login = signUpDTO.Login;
                member.PasswordHash = Sha256.Compute(signUpDTO.Password);
                member.Email = signUpDTO.Email;

                member.IsRegistered = true;
                member.IsApproved = !room.IsApproveManually;

                await _memberRepository.ReplaceMember(member);

                memberId = member.Id;
            }

            var memberToken = _tokenGeneratorService.GenerateToken(memberId, Role.Member, roomId, member.IsApproved);
            var memberTokenDTO = new CommonAPI_Token_DTO { Token = memberToken };

            return (OperationErrorMessages.NoError, memberTokenDTO);
        }

        public async Task<(OperationErrorMessages, object)> MemberAPI_GetMemberById(ObjectId memberId)
        {
            var member = await _memberRepository.GetMemberById(memberId);
            if(member == null)
                return (OperationErrorMessages.MemberNotFound, null);

            var getProfileDTO = Mapper.Map<MemberAPI_GetProfile_DTO>(member);

            return (OperationErrorMessages.NoError, getProfileDTO);
        }
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetMemberById(CommonAPI_MemberId_DTO memberIdDTO) =>
            await this.MemberAPI_GetMemberById(ObjectId.Parse(memberIdDTO.MemberId));
        #endregion
    }
}