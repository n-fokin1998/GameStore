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
    public class PlatformTypeService : IPlatformTypeService
    {
        private const string TypeEn = "TypeEn";
        private const string TypeRu = "TypeRu";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlatformTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<PlatformTypeDTO> GetList()
        {
            var platformTypes = _unitOfWork.PlatformTypes.GetList().Where(p => !p.IsDeleted).ToList();

            return _mapper.Map<IEnumerable<PlatformType>, List<PlatformTypeDTO>>(platformTypes);
        }

        public PlatformTypeDTO GetById(int id)
        {
            var platformType = _unitOfWork.PlatformTypes.GetItem(id);
            platformType = platformType != null && !platformType.IsDeleted ? platformType : null;

            return _mapper.Map<PlatformType, PlatformTypeDTO>(platformType);
        }

        public OperationDetails Add(PlatformTypeDTO platformTypeDto)
        {
            var result = IsValidPlatformType(platformTypeDto);
            if (result != null)
            {
                return result;
            }

            var platformType = _mapper.Map<PlatformTypeDTO, PlatformType>(platformTypeDto);
            _unitOfWork.PlatformTypes.Add(platformType);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        public OperationDetails Update(PlatformTypeDTO platformTypeDto)
        {
            if (platformTypeDto == null)
            {
                return new OperationDetails(false, BLLRes.PlatformTypeNotFound, null);
            }

            var platformTypeRu = _unitOfWork.PlatformTypes.GetList()
                .FirstOrDefault(p => p.TypeRu == platformTypeDto.TypeRu);
            if (platformTypeRu != null && platformTypeRu.Id != platformTypeDto.Id)
            {
                return new OperationDetails(false, BLLRes.PlatformTypeExists, TypeRu);
            }

            var platformTypeEn = _unitOfWork.PlatformTypes.GetList()
                .FirstOrDefault(p => p.TypeEn == platformTypeDto.TypeEn);
            if (platformTypeEn != null && platformTypeEn.Id != platformTypeDto.Id)
            {
                return new OperationDetails(false, BLLRes.PlatformTypeExists, TypeEn);
            }

            var platformType = _mapper.Map<PlatformTypeDTO, PlatformType>(platformTypeDto);
            _unitOfWork.PlatformTypes.Update(platformType, platformType.Id);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        public OperationDetails Delete(PlatformTypeDTO platformTypeDto)
        {
            if (platformTypeDto == null)
            {
                return new OperationDetails(false, BLLRes.PlatformTypeNotFound, null);
            }

            var platformType = _unitOfWork.PlatformTypes.GetItem(platformTypeDto.Id);
            if (platformType == null)
            {
                return new OperationDetails(false, BLLRes.PlatformTypeNotFound, null);
            }

            platformType.IsDeleted = true;
            _unitOfWork.PlatformTypes.Update(platformType, platformType.Id);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        private OperationDetails IsValidPlatformType(PlatformTypeDTO platformType)
        {
            if (platformType == null)
            {
                return new OperationDetails(false, BLLRes.PlatformTypeNotFound, null);
            }

            if (_unitOfWork.PlatformTypes.GetList().FirstOrDefault(p => p.TypeRu == platformType.TypeRu) != null)
            {
                return new OperationDetails(false, BLLRes.PlatformTypeExists, TypeRu);
            }

            return _unitOfWork.PlatformTypes.GetList().FirstOrDefault(p => p.TypeEn == platformType.TypeEn) != null 
                ? new OperationDetails(false, BLLRes.PlatformTypeExists, TypeEn) 
                : null;
        }
    }
}