using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Domain.Mongo.Abstract;
using GameStore.Domain.Mongo.Entities;

namespace GameStore.BusinessLogicLayer.Services
{
    public class ShipperService : IShipperService
    {
        private readonly IMongoUnitOfWork _mongoUnitOfWork;
        private readonly IMapper _mapper;

        public ShipperService(IMongoUnitOfWork mongoUnitOfWork, IMapper mapper)
        {
            _mongoUnitOfWork = mongoUnitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<ShipperDTO> GetList()
        {
            var shippers = _mongoUnitOfWork.Shippers.GetList().ToList();

            return _mapper.Map<IEnumerable<Shipper>, List<ShipperDTO>>(shippers);
        }
    }
}