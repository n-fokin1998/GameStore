using GameStore.BusinessLogicLayer.DTO;

namespace GameStore.BusinessLogicLayer.Abstract.Auth
{
    public interface IUserIdentity
    {
        UserDTO User { get; set; }
    }
}