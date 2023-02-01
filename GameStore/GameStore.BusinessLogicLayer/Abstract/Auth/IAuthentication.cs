using System.Security.Principal;
using System.Web;
using GameStore.BusinessLogicLayer.DTO;

namespace GameStore.BusinessLogicLayer.Abstract.Auth
{
    public interface IAuthentication
    {
        HttpContext HttpContext { get; set; }

        IPrincipal CurrentUser { get; }

        UserDTO Login(string login, string password, bool isPersistent);

        void LogOut();
    }
}