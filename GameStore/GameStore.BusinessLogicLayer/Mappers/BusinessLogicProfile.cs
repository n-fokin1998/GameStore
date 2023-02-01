using AutoMapper;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Domain.Entities;
using GameStore.Domain.Mongo.Entities;

namespace GameStore.BusinessLogicLayer.Mappers
{
    public class BusinessLogicProfile : Profile
    {
        public BusinessLogicProfile()
        {
            CreateMap<Game, GameDTO>().MaxDepth(1);
            CreateMap<GameDTO, Game>()
                .ForMember(s => s.PlatformTypes, opt => { opt.Ignore(); })
                .ForMember(s => s.Genres, opt => { opt.Ignore(); })
                .ForMember(s => s.Publisher, opt => { opt.Ignore(); });
            CreateMap<CommentDTO, Comment>();
            CreateMap<Comment, CommentDTO>();
            CreateMap<Genre, GenreDTO>()
                .ForMember(s => s.Games, opt => { opt.Ignore(); }).ReverseMap();
            CreateMap<OrderDTO, GameStore.Domain.Entities.Order>()
                .ForMember(s => s.OrderDetails, opt => { opt.Ignore(); });
            CreateMap<GameStore.Domain.Entities.Order, OrderDTO>().MaxDepth(1);
            CreateMap<OrderDetailsDTO, OrderDetails>();
            CreateMap<OrderDetails, OrderDetailsDTO>();
            CreateMap<PlatformType, PlatformTypeDTO>()
                .ForMember(s => s.Games, opt => { opt.Ignore(); }).ReverseMap();
            CreateMap<Publisher, PublisherDTO>()
                .ForMember(s => s.Games, opt => { opt.Ignore(); });
            CreateMap<PublisherDTO, Publisher>()
                .ForMember(s => s.Games, opt => { opt.Ignore(); });
            CreateMap<Shipper, ShipperDTO>();
            CreateMap<GameStore.Domain.Mongo.Entities.Order, OrderDTO>();
            CreateMap<User, UserDTO>()
                .ForMember(u => u.Password, opt => opt.MapFrom(src => src.PasswordHash)).MaxDepth(1);
            CreateMap<UserDTO, User>()
                .ForMember(u => u.PasswordHash, opt => opt.MapFrom(src => src.Password)).MaxDepth(1);
            CreateMap<Role, RoleDTO>().MaxDepth(1).ReverseMap();
            CreateMap<GameStore.Domain.Mongo.Entities.Order, OrderDTO>()
                .ForMember(s => s.Date, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(s => s.Id, opt => opt.MapFrom(src => src.OrderId));
            CreateMap<OrderDTO, GameStore.Domain.Mongo.Entities.Order>()
                .ForMember(s => s.OrderDate, opt => opt.MapFrom(src => src.Date))
                .ForMember(s => s.OrderId, opt => opt.MapFrom(src => src.Id));
        }
    }
}