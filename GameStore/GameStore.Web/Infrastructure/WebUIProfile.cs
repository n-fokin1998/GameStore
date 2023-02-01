using AutoMapper;
using GameStore.BusinessLogicLayer.Domain;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.Areas.Admin.ViewModels;
using GameStore.Web.Areas.Manager.ViewModels;
using GameStore.Web.Areas.Manager.ViewModels.Order;
using GameStore.Web.ViewModels;
using GameStore.Web.ViewModels.Account;

namespace GameStore.Web.Infrastructure
{
    public class WebUiProfile : Profile
    {
        public WebUiProfile()
        {
            CreateMap<CommentViewModel, CommentDTO>();
            CreateMap<GameDTO, GameViewModel>();
            CreateMap<AddGameViewModel, GameDTO>()
                .ForMember(s => s.Genres, opt => { opt.Ignore(); })
                .ForMember(s => s.PlatformTypes, opt => { opt.Ignore(); });
            CreateMap<GameDTO, AddGameViewModel>()
                 .ForMember(s => s.Genres, opt => { opt.Ignore(); })
                .ForMember(s => s.PlatformTypes, opt => { opt.Ignore(); });
            CreateMap<PaymentViewModel, PaymentInfo>();
            CreateMap<PublisherViewModel, PublisherDTO>();
            CreateMap<OrderDTO, OrderHistoryViewModel>();
            CreateMap<OrderDetailsDTO, UpdateOrderDetailViewModel>().ReverseMap();
            CreateMap<PublisherViewModel, PublisherDTO>().ReverseMap();
            CreateMap<GenreDTO, GenreViewModel>().ReverseMap();
            CreateMap<PlatformTypeDTO, PlatformTypeViewModel>().ReverseMap();
            CreateMap<CatalogViewModel, GameFilterDTO>();
            CreateMap<RegisterViewModel, UserDTO>()
                .ForMember(s => s.Roles, opt => { opt.Ignore(); });
            CreateMap<RoleViewModel, RoleDTO>()
                .ForMember(s => s.Users, opt => { opt.Ignore(); }).ReverseMap();
            CreateMap<UserDTO, UserViewModel>()
                .ForMember(s => s.Roles, opt => { opt.Ignore(); });
            CreateMap<UserViewModel, UserDTO>()
                .ForMember(s => s.Roles, opt => { opt.Ignore(); });
        }
    }
}