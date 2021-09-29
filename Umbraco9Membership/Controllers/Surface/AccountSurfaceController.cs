using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Security;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco9Membership.Models.ViewModels;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Umbraco9Membership.Controllers.Surface
{
    public class AccountSurfaceController : SurfaceController
    {
        private readonly IMemberSignInManager signInManager;

        public AccountSurfaceController(IMemberSignInManager signInManager, IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services, AppCaches appCaches,
            IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            SignInResult result = await signInManager.PasswordSignInAsync(
                model.Username, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: true);

            return RedirectToCurrentUmbracoUrl();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToCurrentUmbracoUrl();
        }
    }
}
