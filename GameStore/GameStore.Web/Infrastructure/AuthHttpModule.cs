using System;
using System.Web;
using System.Web.Mvc;
using GameStore.BusinessLogicLayer.Abstract.Auth;

namespace GameStore.Web.Infrastructure
{
    public class AuthHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += Authenticate;
        }

        public void Dispose()
        {
        }

        private void Authenticate(object source, EventArgs e)
        {
            var app = (HttpApplication)source;
            var context = app.Context;
            var auth = DependencyResolver.Current.GetService<IAuthentication>();
            auth.HttpContext = context;
            context.User = auth.CurrentUser;
        }
    }
}