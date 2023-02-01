using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using AutoMapper;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.BusinessLogicLayer.Abstract.Auth;
using GameStore.BusinessLogicLayer.Abstract.Infrastructure;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;

namespace GameStore.BusinessLogicLayer.Services.Auth
{
    public class AuthenticationService : IAuthentication
    {
        private const string CookieName = "AUTH";
        private const int CookieVersion = 1;
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;
        private readonly UserProvider _userProvider;
        private readonly IHashPasswordManager _hashPasswordManager;
        private IPrincipal _currentUser;

        public AuthenticationService(
            IUnitOfWork database,
            IMapper mapper,
            IHashPasswordManager hashPasswordManager,
            UserProvider userProvider)
        {
            _database = database;
            _mapper = mapper;
            _userProvider = userProvider;
            _hashPasswordManager = hashPasswordManager;
        }

        public HttpContext HttpContext { get; set; }

        public IPrincipal CurrentUser
        {
            get
            {
                if (_currentUser != null)
                {
                    return _currentUser;
                }

                var authCookie = HttpContext?.Request.Cookies.Get(CookieName);
                if (!string.IsNullOrEmpty(authCookie?.Value))
                {
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    _userProvider.InitIdentity(ticket?.Name);
                }

                _currentUser = _userProvider;

                return _currentUser;
            }
        }

        public UserDTO Login(string login, string password, bool isPersistent)
        {
            var user = _database.Users.GetUser(login);
            if (user == null || !_hashPasswordManager.VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            CreateCookie(login, isPersistent);

            return _mapper.Map<User, UserDTO>(user);
        }

        public void LogOut()
        {
            var httpCookie = HttpContext?.Response.Cookies[CookieName];
            if (httpCookie != null)
            {
                httpCookie.Value = string.Empty;
            }
        }

        private void CreateCookie(string userName, bool isPersistent = false)
        {
            var ticket = new FormsAuthenticationTicket(
                CookieVersion,
                userName,
                DateTime.Now,
                DateTime.Now.Add(FormsAuthentication.Timeout),
                isPersistent,
                string.Empty,
                FormsAuthentication.FormsCookiePath);
            var encTicket = FormsAuthentication.Encrypt(ticket);
            var authCookie = new HttpCookie(CookieName)
            {
                Value = encTicket,
                Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
            };
            HttpContext?.Response.Cookies.Set(authCookie);
        }
    }
}