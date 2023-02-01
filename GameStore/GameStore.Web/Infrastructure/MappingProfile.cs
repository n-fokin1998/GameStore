using AutoMapper;
using GameStore.BusinessLogicLayer.Mappers;

namespace GameStore.Web.Infrastructure
{
    public static class MappingProfile
    {
        public static MapperConfiguration InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BusinessLogicProfile());
                cfg.AddProfile(new WebUiProfile());
            });
            return config;
        }
    }
}