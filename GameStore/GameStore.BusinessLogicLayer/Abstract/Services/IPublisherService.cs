using System.Collections.Generic;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Infrastructure;

namespace GameStore.BusinessLogicLayer.Abstract.Services
{
    public interface IPublisherService
    {
        IEnumerable<PublisherDTO> GetList();

        PublisherDTO GetById(int id);

        OperationDetails Add(PublisherDTO publisherDto);

        OperationDetails Update(PublisherDTO publisherDto);

        OperationDetails Delete(PublisherDTO publisherDto);
    }
}