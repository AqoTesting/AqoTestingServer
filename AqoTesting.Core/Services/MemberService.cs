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
        public async Task<(OperationErrorMessages, object)> UserAPI_GetMemberById(ObjectId memberId)
        {
            var member = await _memberRepository.GetMemberById(memberId);
            if(member == null)
                return (OperationErrorMessages.MemberNotFound, null);

            var getMemberDTO = Mapper.Map<UserAPI_GetMember_DTO>(member);

            return (OperationErrorMessages.NoError, getMemberDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetMemberById(MemberId_DTO memberIdDTO) =>
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
        public async Task<(OperationErrorMessages, object)> UserAPI_GetMembersByRoomId(RoomId_DTO roomIdDTO) =>
            await this.UserAPI_GetMembersByRoomId(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<(OperationErrorMessages, object)> UserAPI_ManualMemberAdd(ObjectId roomId, UserAPI_PostMember_DTO postMemberDTO)
        {
            var room = await _roomRepository.GetRoomById(roomId);
            if(room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            // Fields validation
            (var fieldsValid, var errorCode, var response) = FieldsValidator.Validate(room.Fields, postMemberDTO.Fields);
            if(!fieldsValid)
                return (errorCode, response);

            // Check fields hash exists
            var fieldsHash = FieldsHashGenerator.Generate(postMemberDTO.Fields);
            var alreadyExists = await _memberRepository.GetMemberByFieldsHash(roomId, fieldsHash);
            if(alreadyExists != null)
                return (OperationErrorMessages.FieldsAlreadyExists, null);

            var newMember = Mapper.Map<Member>(postMemberDTO);
            newMember.IsChecked = true;
            newMember.RoomId = roomId;
            newMember.FieldsHash = fieldsHash;

            var newMemberId = await _memberRepository.InsertMember(newMember);
            var newMemberIdDTO = new MemberId_DTO { MemberId = newMemberId.ToString() };

            return (OperationErrorMessages.NoError, newMemberIdDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_ManualMemberAdd(RoomId_DTO roomIdDTO, UserAPI_PostMember_DTO postMemberDTO) =>
            await this.UserAPI_ManualMemberAdd(ObjectId.Parse(roomIdDTO.RoomId), postMemberDTO);
        #endregion

        #region Member API
        public async Task<(OperationErrorMessages, object)> MemberAPI_SignIn(MemberAPI_SignIn_DTO signInDTO)
        {
            var room = await _roomRepository.GetRoomById(ObjectId.Parse(signInDTO.RoomId));
            if(room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            var member = await _memberRepository.GetMemberByAuthData(ObjectId.Parse(signInDTO.RoomId), signInDTO.Login, Sha256.Compute(signInDTO.Password));

            if(member == null)
                return (OperationErrorMessages.WrongAuthData, null);

            var memberToken = TokenGenerator.GenerateToken(member.Id, Role.Member, room.Id, member.IsChecked);
            var memberTokenDTO = new Token_DTO { Token = memberToken };

            return (OperationErrorMessages.NoError, memberTokenDTO);
        }

        public async Task<(OperationErrorMessages, object)> MemberAPI_SignUpByFields(MemberAPI_SignUpByFields_DTO signUpDTO)
        {
            var roomId = ObjectId.Parse(signUpDTO.RoomId);

            var room = await _roomRepository.GetRoomById(roomId);
            if(room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            // Попытка зарегаться по полям с отключённой регистрацией
            if(!room.IsRegistrationEnabled)
                return (OperationErrorMessages.RegistrationDisabled, null);
            
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
            var alreadyExists = await _memberRepository.GetMemberByFieldsHash(roomId, fieldsHash);
            if(alreadyExists != null)
                return (OperationErrorMessages.MemberAlreadyExists, null);

            var newMember = Mapper.Map<Member>(signUpDTO);
            newMember.IsChecked = !room.IsApproveManually;
            newMember.FieldsHash = fieldsHash;

            var newMemberId = await _memberRepository.InsertMember(newMember);

            var newMemberToken = TokenGenerator.GenerateToken(newMemberId, Role.Member, roomId, newMember.IsChecked);
            var newMemberTokenDTO = new Token_DTO { Token = newMemberToken };

            return (OperationErrorMessages.NoError, newMemberTokenDTO);
        }

        public async Task<(OperationErrorMessages, object)> MemberAPI_SignUpByFieldsHash(MemberAPI_SignUpByFieldsHash_DTO signUpDTO)
        {
            var roomId = ObjectId.Parse(signUpDTO.RoomId);

            var room = await _roomRepository.GetRoomById(roomId);
            if(room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            // Попытка зарегаться по хешу со включённой регистрацией
            if(!room.IsRegistrationEnabled)
                return (OperationErrorMessages.RegistrationEnabled, null);

            // Check login or email taken
            var loginTaken = await _memberRepository.CheckLoginTaken(roomId, signUpDTO.Login);
            if(loginTaken)
                return (OperationErrorMessages.LoginAlreadyTaken, null);
            var emailTaken = await _memberRepository.CheckEmailTaken(roomId, signUpDTO.Email);
            if(emailTaken)
                return (OperationErrorMessages.EmailAlreadyTaken, null);

            // Добавил ли юзер мембера с такими полями
            var fieldsHash = Hex.StringToBytes(signUpDTO.FieldsHash);
            var member = await _memberRepository.GetMemberByFieldsHash(roomId, fieldsHash);
            if(member == null)
                return (OperationErrorMessages.MemberNotFound, null);

            member.Login = signUpDTO.Login;
            member.PasswordHash = Sha256.Compute(signUpDTO.Password);
            member.Email = signUpDTO.Email;
            member.IsRegistered = true;

            await _memberRepository.ReplaceMember(member);

            var newMemberToken = TokenGenerator.GenerateToken(member.Id, Role.Member, roomId, true);
            var newMemberTokenDTO = new Token_DTO { Token = newMemberToken };

            return (OperationErrorMessages.NoError, newMemberTokenDTO);
        }

        public async Task<(OperationErrorMessages, object)> MemberAPI_GetMemberById(ObjectId memberId)
        {
            var member = await _memberRepository.GetMemberById(memberId);
            if(member == null)
                return (OperationErrorMessages.MemberNotFound, null);

            var getProfileDTO = Mapper.Map<MemberAPI_GetProfile_DTO>(member);

            return (OperationErrorMessages.NoError, getProfileDTO);
        }
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetMemberById(MemberId_DTO memberIdDTO) =>
            await this.MemberAPI_GetMemberById(ObjectId.Parse(memberIdDTO.MemberId));
        #endregion
    }
}