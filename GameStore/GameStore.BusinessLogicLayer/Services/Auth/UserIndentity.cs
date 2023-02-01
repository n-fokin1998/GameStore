using System.Security.Principal;
using AutoMapper;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Abstract.Auth;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.BusinessLogicLayer.Services.Auth
{
    public class UserIndentity : IIdentity, IUserIdentity
    {
        private const string GuestName = "Guest";
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;

        public UserIndentity(IUnitOfWork database, IMapper mapper)
        {
            _database = database;
            _mapper = mapper;
        }

        public UserDTO User { get; set; }

        public string AuthenticationType => typeof(UserDTO).ToString();

        public bool IsAuthenticated => User != null;

        public string Name => User != null ? User.Name : GuestName;

        public void Init(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return;
            }

            var user = _database.Users.GetUser(email);
            User = _mapper.Map<User, UserDTO>(user);
            User.Roles.RemoveAll(r => r.IsDeleted);
        }
    }
}