using System.Security.Principal;

namespace GameStore.BusinessLogicLayer.Services.Auth
{
    public class UserProvider : IPrincipal
    {
        private readonly UserIndentity _userIdentity;

        public UserProvider(UserIndentity userIdentity)
        {
            _userIdentity = userIdentity;
        }

        public IIdentity Identity => _userIdentity;

        public bool IsInRole(string role)
        {
            return _userIdentity.User != null && _userIdentity.User.InRoles(role);
        }

        public override string ToString()
        {
            return _userIdentity.Name;
        }

        public void InitIdentity(string name)
        {
            _userIdentity.Init(name);
        }
    }
}