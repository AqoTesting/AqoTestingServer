using AqoTesting.Core.Utils;
using AqoTesting.Shared.DTOs.API.CommonAPI.Identifiers;
using AqoTesting.Shared.DTOs.API.MemberAPI.Account;
using AqoTesting.Shared.DTOs.API.UserAPI.Members;
using AqoTesting.Shared.DTOs.DB.Members;
using AqoTesting.Shared.Enums;
using AqoTesting.Shared.Infrastructure;
using AqoTesting.Shared.Interfaces;
using AutoMapper;
using MongoDB.Bson;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace AqoTesting.Core.Services
{
    public class MemberService : ServiceBase, IMemberService
    {
        IMemberRepository _memberRepository;
        IAttemptRepository _attemptRepository;
        IRoomRepository _roomRepository;
        ITokenGeneratorService _tokenGeneratorService;
        ITokenRepository _tokenRepository;
        ICacheRepository _cacheRepository;

        public MemberService(IRoomRepository roomRespository, IAttemptRepository attemptRepository, IMemberRepository memberRepository, ITokenGeneratorService tokenGeneratorService, ITokenRepository tokenRepository, ICacheRepository cacheRepository)
        {
            _memberRepository = memberRepository;
            _attemptRepository = attemptRepository;
            _roomRepository = roomRespository;
            _tokenGeneratorService = tokenGeneratorService;
            _tokenRepository = tokenRepository;
            _cacheRepository = cacheRepository;
        }

        #region User API
        public async Task<(OperationErrorMessages, object)> UserAPI_GetMemberById(ObjectId memberId)
        {
            var member = await _memberRepository.GetMemberById(memberId);
            var getMemberDTO = Mapper.Map<UserAPI_GetMemberDTO>(member);

            return (OperationErrorMessages.NoError, getMemberDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetMemberById(CommonAPI_MemberIdDTO memberIdDTO) =>
            await this.UserAPI_GetMemberById(ObjectId.Parse(memberIdDTO.MemberId));

        public async Task<(OperationErrorMessages, object)> UserAPI_GetMembersByRoomId(ObjectId roomId)
        {
            var members = await _memberRepository.GetMembersByRoomId(roomId);
            var getMembersItemDTOs = Mapper.Map<UserAPI_GetMembersItemDTO[]>(members);

            return (OperationErrorMessages.NoError, getMembersItemDTOs);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_GetMembersByRoomId(CommonAPI_RoomIdDTO roomIdDTO) =>
            await this.UserAPI_GetMembersByRoomId(ObjectId.Parse(roomIdDTO.RoomId));

        public async Task<(OperationErrorMessages, object)> UserAPI_ManualMemberAdd(ObjectId roomId, UserAPI_PostMemberDTO postMemberDTO)
        {
            var room = await _roomRepository.GetRoomById(roomId);

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

            var newMember = Mapper.Map<MembersDB_MemberDTO>(postMemberDTO);
            newMember.IsApproved = !room.IsApproveManually;
            newMember.UserId = room.UserId;
            newMember.RoomId = roomId;
            newMember.FieldsHash = fieldsHash;

            var newMemberId = await _memberRepository.InsertMember(newMember);
            var newMemberIdDTO = new CommonAPI_MemberIdDTO { MemberId = newMemberId.ToString() };

            return (OperationErrorMessages.NoError, newMemberIdDTO);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_ManualMemberAdd(CommonAPI_RoomIdDTO roomIdDTO, UserAPI_PostMemberDTO postMemberDTO) =>
            await this.UserAPI_ManualMemberAdd(ObjectId.Parse(roomIdDTO.RoomId), postMemberDTO);

        public async Task<(OperationErrorMessages, object)> UserAPI_SetMemberTags(ObjectId memberId, UserAPI_MemberTagDTO[] memberTagsDTO)
        {
            var tags = Mapper.Map<MembersDB_TagDTO[]>(memberTagsDTO);
            await _memberRepository.SetTags(memberId, tags);

            return (OperationErrorMessages.NoError, null);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_SetMemberTags(CommonAPI_MemberIdDTO memberIdDTO, UserAPI_PostMemberTagsDTO postMemberTagsDTO) =>
            await this.UserAPI_SetMemberTags(ObjectId.Parse(memberIdDTO.MemberId), postMemberTagsDTO.Tags);

        public async Task<(OperationErrorMessages, object)> UserAPI_Approve(ObjectId memberId)
        {
            var changed = await _memberRepository.SetProperty(memberId, "IsApproved", true);
            if (!changed)
                return (OperationErrorMessages.MemberIsApproved, null);

            return (OperationErrorMessages.NoError, null);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_Approve(CommonAPI_MemberIdDTO memberIdDTO) =>
            await this.UserAPI_Approve(ObjectId.Parse(memberIdDTO.MemberId));

        public async Task<(OperationErrorMessages, object)> UserAPI_Unregister(ObjectId memberId)
        {
            var unregistered = await _memberRepository.SetProperty(memberId, "IsRegistered", false);
            if(!unregistered)
                return (OperationErrorMessages.MemberIsNotRegistered, null);

            await _cacheRepository.DelAll(await _cacheRepository.Keys($"Member:{memberId}*"));

            return (OperationErrorMessages.NoError, null);
        }
        public async Task<(OperationErrorMessages, object)> UserAPI_Unregister(CommonAPI_MemberIdDTO memberIdDTO) =>
            await this.UserAPI_Unregister(ObjectId.Parse(memberIdDTO.MemberId));

        public async Task<OperationErrorMessages> UserAPI_Delete(ObjectId memberId)
        {
            var deleted = await _memberRepository.Delete(memberId);

            if(deleted)
            {
                await _attemptRepository.DeleteAttemptsByMemberId(memberId);
                await _tokenRepository.DelAll(Role.Member, memberId);
            }

            else
                return OperationErrorMessages.MemberNotFound;

            return OperationErrorMessages.NoError;
        }
        public async Task<OperationErrorMessages> UserAPI_Delete(CommonAPI_MemberIdDTO memberIdDTO) =>
            await this.UserAPI_Delete(ObjectId.Parse(memberIdDTO.MemberId));
        #endregion

        #region Member API
        public async Task<(OperationErrorMessages, object)> MemberAPI_SignIn(MemberAPI_SignInDTO signInDTO)
        {
            var room = await _roomRepository.GetRoomById(ObjectId.Parse(signInDTO.RoomId));
            if(room == null)
                return (OperationErrorMessages.RoomNotFound, null);

            var member = await _memberRepository.GetMemberByAuthData(ObjectId.Parse(signInDTO.RoomId), signInDTO.Login, Sha256.Compute(signInDTO.Password));

            if(member == null || !member.IsRegistered)
                return (OperationErrorMessages.WrongAuthData, null);

            var memberToken = _tokenGeneratorService.GenerateToken(member.Id, Role.Member, room.Id);
            var memberTokenDTO = new CommonAPI_TokenDTO { Token = memberToken };

            await _tokenRepository.Add(Role.Member, member.Id, new JwtSecurityToken(memberToken), AuthOptions.LIFETIME);

            return (OperationErrorMessages.NoError, memberTokenDTO);
        }

        public async Task<(OperationErrorMessages, object)> MemberAPI_SignUp(MemberAPI_SignUpDTO signUpDTO)
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
                    return (OperationErrorMessages.MemberAlreadyRegistered, null);

                member = Mapper.Map<MembersDB_MemberDTO>(signUpDTO);
                member.PasswordHash = Sha256.Compute(signUpDTO.Password);
                member.UserId = room.UserId;

                member.IsRegistered = true;
                member.IsApproved = !room.IsApproveManually;

                member.FieldsHash = fieldsHash;

                memberId = await _memberRepository.InsertMember(member);
            }
            else
            {
                if (member == null)
                    return (OperationErrorMessages.MemberNotFound, null);

                if(member.IsRegistered)
                    return (OperationErrorMessages.MemberAlreadyRegistered, null);

                member.Login = signUpDTO.Login;
                member.PasswordHash = Sha256.Compute(signUpDTO.Password);
                member.Email = signUpDTO.Email;

                member.IsRegistered = true;
                member.IsApproved = !room.IsApproveManually;

                await _memberRepository.ReplaceMember(member);

                memberId = member.Id;
            }

            var memberToken = _tokenGeneratorService.GenerateToken(memberId, Role.Member, roomId);
            var memberTokenDTO = new CommonAPI_TokenDTO { Token = memberToken };

            await _tokenRepository.Add(Role.Member, member.Id, new JwtSecurityToken(memberToken), AuthOptions.LIFETIME);

            return (OperationErrorMessages.NoError, memberTokenDTO);
        }

        public async Task<(OperationErrorMessages, object)> MemberAPI_GetMemberById(ObjectId memberId)
        {
            var member = await _memberRepository.GetMemberById(memberId);
            if(member == null)
                return (OperationErrorMessages.MemberNotFound, null);

            var getProfileDTO = Mapper.Map<MemberAPI_GetProfileDTO>(member);

            return (OperationErrorMessages.NoError, getProfileDTO);
        }
        public async Task<(OperationErrorMessages, object)> MemberAPI_GetMemberById(CommonAPI_MemberIdDTO memberIdDTO) =>
            await this.MemberAPI_GetMemberById(ObjectId.Parse(memberIdDTO.MemberId));
        #endregion
    }
}