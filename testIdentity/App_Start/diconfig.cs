using Autofac;
using Autofac.Integration.Mvc;
using dataaccess;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace testIdentity
{
    public class diconfig
    {
        public static void Run()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<identitydb>().AsSelf().InstancePerRequest();
            builder.Register(c => new UserStore<User>(c.Resolve<identitydb>())).AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().Inst‌​ancePerRequest();
            builder.Register(c => new IdentityFactoryOptions<ApplicationUserManager>()
            {
                DataProtectionProvider = new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider("ApplicationName")
            });
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication).As<IAuthenticationManager>();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}