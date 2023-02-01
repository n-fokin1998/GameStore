using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Infrastructure;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.App_LocalResources;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private const string User = "User";
        private const string Email = "Email";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashPasswordManager _hashPasswordManager;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IHashPasswordManager hashPasswordManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hashPasswordManager = hashPasswordManager;
        }

        public OperationDetails Register(UserDTO userDto)
        {
            if (userDto == null)
            {
                return new OperationDetails(false, BLLRes.UserNotFound, Email);
            }

            var userByName = _unitOfWork.Users.GetList().FirstOrDefault(p => p.Name == userDto.Name);
            if (userByName != null)
            {
                return new OperationDetails(false, BLLRes.UserExists, Email);
            }

            var userByEmail = _unitOfWork.Users.GetList().FirstOrDefault(p => p.Email == userDto.Email);
            if (userByEmail != null)
            {
                return new OperationDetails(false, BLLRes.UserExists, Email);
            }

            var user = _mapper.Map<UserDTO, User>(userDto);
            user.Name = user.Name ?? user.Email;
            user.PasswordHash = _hashPasswordManager.EncryptPassword(userDto.Password);
            if (userDto.Roles != null && !userDto.Roles.Any())
            {
                user.Roles.Add(_unitOfWork.Roles.GetList().FirstOrDefault(r => r.Name == User));
            }
            else
            {
                AttachRolesCollection(userDto.Roles, user);
            }

            _unitOfWork.Users.Register(user);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        public OperationDetails Delete(UserDTO userDto)
        {
            if (userDto == null)
            {
                return new OperationDetails(false, BLLRes.UserNotFound, null);
            }

            var user = _unitOfWork.Users.GetById(userDto.Id);
            if (user == null)
            {
                return new OperationDetails(false, BLLRes.UserNotFound, null);
            }

            user.IsDeleted = true;
            _unitOfWork.Users.Update(user);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        public UserDTO GetById(int id)
        {
            var user = _unitOfWork.Users.GetById(id);
            user = user != null && !user.IsDeleted ? user : null;
            user?.Roles.ToList().RemoveAll(r => r.IsDeleted);

            return _mapper.Map<User, UserDTO>(user);
        }

        public IEnumerable<UserDTO> GetList()
        {
            var users = _unitOfWork.Users.GetList().Where(u => !u.IsDeleted).ToList();

            return _mapper.Map<IEnumerable<User>, List<UserDTO>>(users);
        }

        public OperationDetails Update(UserDTO userDto)
        {
            if (userDto == null)
            {
                return new OperationDetails(false, BLLRes.UserNotFound, null);
            }

            var user = _unitOfWork.Users.GetById(userDto.Id);
            if (user == null)
            {
                return new OperationDetails(false, BLLRes.UserNotFound, null);
            }

            AttachRolesCollection(userDto.Roles, user);
            _unitOfWork.Users.Update(user);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        private void AttachRolesCollection(IEnumerable<RoleDTO> roles, User user)
        {
            user.Roles = new List<Role>();
            if (roles == null)
            {
                return;
            }

            var rolesIds = roles.Select(r => r.Id).ToList();
            var existingRoles = _unitOfWork.Roles.GetList().Where(r => rolesIds.Any(i => i == r.Id)).ToList();
            foreach (var role in existingRoles)
            {
                user.Roles.Add(role);
            }

            if (user.Roles.Count == 0)
            {
                user.Roles.Add(_unitOfWork.Roles.GetList().FirstOrDefault(g => g.Name == User));
            }
        }       
    }
}