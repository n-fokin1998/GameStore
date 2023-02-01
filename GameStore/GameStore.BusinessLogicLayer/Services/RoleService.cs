using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.App_LocalResources;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.BusinessLogicLayer.Services
{
    public class RoleService : IRoleService
    {
        private const string Name = "Name";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public OperationDetails Add(RoleDTO roleDto)
        {
            var result = IsValidRole(roleDto);
            if (result != null)
            {
                return result;
            }

            var role = _mapper.Map<RoleDTO, Role>(roleDto);
            _unitOfWork.Roles.Add(role);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        public OperationDetails Delete(RoleDTO roleDto)
        {
            if (roleDto == null)
            {
                return new OperationDetails(false, BLLRes.RoleNotFound, null);
            }

            var role = _unitOfWork.Roles.GetItem(roleDto.Id);
            if (role == null)
            {
                return new OperationDetails(false, BLLRes.RoleExists, null);
            }

            role.IsDeleted = true;
            _unitOfWork.Roles.Update(role, role.Id);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        public RoleDTO GetById(int id)
        {
            var role = _unitOfWork.Roles.GetItem(id);
            role = role != null && !role.IsDeleted ? role : null;

            return _mapper.Map<Role, RoleDTO>(role);
        }

        public IEnumerable<RoleDTO> GetList()
        {
            var roles = _unitOfWork.Roles.GetList().Where(r => !r.IsDeleted).ToList();

            return _mapper.Map<IEnumerable<Role>, List<RoleDTO>>(roles);
        }

        public OperationDetails Update(RoleDTO roleDto)
        {
            if (roleDto == null)
            {
                return new OperationDetails(false, BLLRes.RoleNotFound, null);
            }

            var role = _unitOfWork.Roles.GetList().FirstOrDefault(p => p.Name == roleDto.Name);
            if (role != null && role.Id != roleDto.Id)
            {
                return new OperationDetails(false, BLLRes.RoleExists, Name);
            }

            role = _unitOfWork.Roles.GetItem(roleDto.Id);
            if (role == null)
            {
                return new OperationDetails(false, BLLRes.RoleNotFound, null);
            }

            role = _mapper.Map<RoleDTO, Role>(roleDto);
            _unitOfWork.Roles.Update(role, role.Id);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        private OperationDetails IsValidRole(RoleDTO role)
        {
            if (role == null)
            {
                return new OperationDetails(false, BLLRes.RoleNotFound, null);
            }

            return _unitOfWork.Roles.GetList().FirstOrDefault(p => p.Name == role.Name) != null ?
                new OperationDetails(false, BLLRes.RoleExists, Name) : null;
        }
    }
}