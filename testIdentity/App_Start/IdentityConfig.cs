using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using testIdentity.Models;
using dataaccess;
using Twilio.Clients;
using Twilio.Http;

namespace testIdentity
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            ////Twilio Begin
            //var Twilio = new TwilioRestClient(
            //  System.Configuration.ConfigurationManager.AppSettings["SMSAccountIdentification"],
            //  System.Configuration.ConfigurationManager.AppSettings["SMSAccountPassword"]);
            //var result = await Twilio.RequestAsync(new Twilio.Http.Request(HttpMethod.Post, ))


            //    .SendMessage(
            //  System.Configuration.ConfigurationManager.AppSettings["SMSAccountFrom"],
            //  message.Destination, message.Body
            //);
            //Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
            // Trace.TraceInformation(result.Status);
            //Twilio doesn't currently have an async API, so return success.
            return Task.FromResult(0);

            //var twilio = new TwilioRestClient(TwilioSettings.AccountSID, TwilioSettings.AuthToken);

            //var result = twilio.SendMessage(TwilioSettings.PhoneNumber, message.Destination, message.Body);

            //Trace.TraceInformation(result.Status);

            // Twilio doesn't currently have an async API, so we return success.
            //return Task.FromResult(0);
            // Twilio End
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<User>
    {
        public ApplicationUserManager(IUserStore<User> store, 
            identitydb dbContext,
            IdentityFactoryOptions<ApplicationUserManager> options)
            : base(store)
        {
            UserValidator = new UserValidator<User>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User>
            {
                MessageFormat = "Your security code is {0}"
            });
            RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            EmailService = new EmailService();
            SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                UserTokenProvider =
                    new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }

        //public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        //{
        //    var manager = new ApplicationUserManager(new UserStore<User>(context.Get<identitydb>()));
        //    // Configure validation logic for usernames
        //    manager.UserValidator = new UserValidator<User>(manager)
        //    {
        //        AllowOnlyAlphanumericUserNames = false,
        //        RequireUniqueEmail = true
        //    };

        //    // Configure validation logic for passwords
        //    manager.PasswordValidator = new PasswordValidator
        //    {
        //        RequiredLength = 6,
        //        RequireNonLetterOrDigit = true,
        //        RequireDigit = true,
        //        RequireLowercase = true,
        //        RequireUppercase = true,
        //    };

        //    // Configure user lockout defaults
        //    manager.UserLockoutEnabledByDefault = true;
        //    manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
        //    manager.MaxFailedAccessAttemptsBeforeLockout = 5;

        //    // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
        //    // You can write your own provider and plug it in here.
        //    manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User>
        //    {
        //        MessageFormat = "Your security code is {0}"
        //    });
        //    manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User>
        //    {
        //        Subject = "Security Code",
        //        BodyFormat = "Your security code is {0}"
        //    });
        //    manager.EmailService = new EmailService();
        //    manager.SmsService = new SmsService();
        //    var dataProtectionProvider = options.DataProtectionProvider;
        //    if (dataProtectionProvider != null)
        //    {
        //        manager.UserTokenProvider = 
        //            new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
        //    }
        //    return manager;
        //}
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<User, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, 
            IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {

        }
        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }
    }
}
