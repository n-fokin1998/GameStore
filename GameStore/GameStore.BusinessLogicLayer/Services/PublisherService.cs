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
    public class PublisherService : IPublisherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PublisherService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<PublisherDTO> GetList()
        {
            var publishers = _unitOfWork.Publishers.GetList().Where(p => !p.IsDeleted).ToList();

            return _mapper.Map<IEnumerable<Publisher>, List<PublisherDTO>>(publishers);
        }

        public PublisherDTO GetById(int id)
        {
            var publisher = _unitOfWork.Publishers.GetItem(id);
            publisher = publisher != null && !publisher.IsDeleted ? publisher : null;
            return _mapper.Map<Publisher, PublisherDTO>(publisher);
        }

        public OperationDetails Add(PublisherDTO publisherDto)
        {
            var result = IsValidPublisher(publisherDto);
            if (result != null)
            {
                return result;
            }

            var publisher = _mapper.Map<PublisherDTO, Publisher>(publisherDto);
            _unitOfWork.Publishers.Add(publisher);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        public OperationDetails Update(PublisherDTO publisherDto)
        {
            if (publisherDto == null)
            {
                return new OperationDetails(false, BLLRes.PublisherNotFound, null);
            }

            var publisher = _unitOfWork.Publishers.GetList()
                .FirstOrDefault(p => p.CompanyName == publisherDto.CompanyName);
            if (publisher != null && publisher.Id != publisherDto.Id)
            {
                return new OperationDetails(false, BLLRes.PublisherExists, null);
            }

            publisher = _unitOfWork.Publishers.GetItem(publisherDto.Id);
            if (publisher == null)
            {
                return new OperationDetails(false, BLLRes.PublisherNotFound, null);
            }

            publisher = _mapper.Map<PublisherDTO, Publisher>(publisherDto);
            _unitOfWork.Publishers.Update(publisher, publisher.Id);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        public OperationDetails Delete(PublisherDTO publisherDto)
        {
            if (publisherDto == null)
            {
                return new OperationDetails(false, BLLRes.PublisherNotFound, null);
            }

            var publisher = _unitOfWork.Publishers.GetItem(publisherDto.Id);
            if (publisher == null)
            {
                return new OperationDetails(false, BLLRes.PublisherNotFound, null);
            }

            publisher.IsDeleted = true;
            _unitOfWork.Publishers.Update(publisher, publisher.Id);
            _unitOfWork.Commit();

            return new OperationDetails(true);
        }

        private OperationDetails IsValidPublisher(PublisherDTO publisher)
        {
            if (publisher == null)
            {
                return new OperationDetails(false, BLLRes.PublisherNotFound, null);
            }

            return _unitOfWork.Publishers.GetList().FirstOrDefault(p => p.CompanyName == publisher.CompanyName) != null 
                ? new OperationDetails(false, BLLRes.PublisherExists, null) 
                : null;
        }
    }
}