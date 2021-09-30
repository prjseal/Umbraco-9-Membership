using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Security;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Extensions;
using Umbraco9Membership.Models.ViewModels;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Umbraco9Membership.Controllers.Surface
{
    public class AccountSurfaceController : SurfaceController
    {
        private readonly IMemberSignInManager _memberSignInManager;
        private readonly IMemberManager _memberManager;
        private readonly IMemberService _memberService;
        private readonly ILogger<AccountSurfaceController> _logger;

        public AccountSurfaceController(
            //these are required by the base controller
            IUmbracoContextAccessor umbracoContextAccessor, 
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services, 
            AppCaches appCaches,
            IProfilingLogger profilingLogger, 
            IPublishedUrlProvider publishedUrlProvider,
            //these are dependencies we've added
            IMemberSignInManager memberSignInManager,
            IMemberManager memberManager,
            IMemberService memberService,
            ILogger<AccountSurfaceController> logger
            ) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _memberSignInManager = memberSignInManager ?? throw new ArgumentNullException(nameof(memberSignInManager));
            _memberManager = memberManager ?? throw new ArgumentNullException(nameof(memberManager));
            _memberService = memberService ?? throw new ArgumentNullException(nameof(memberService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            SignInResult result = await _memberSignInManager.PasswordSignInAsync(
                model.Username, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: true);

            return RedirectToCurrentUmbracoUrl();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _memberSignInManager.SignOutAsync();

            return RedirectToCurrentUmbracoUrl();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            var existingMember = _memberService.GetByEmail(model.Email);

            if (existingMember != null)
            {
                _logger.LogInformation("Register: Member has already been registered");
            }
            else
            {
                if (!ModelState.IsValid) return RedirectToCurrentUmbracoPage();


                var fullName = $"{model.FirstName} {model.LastName} {model.Email}";

                var memberTypeAlias = CurrentPage.HasValue("memberType")
                    ? CurrentPage.Value<string>("memberType")
                    : Constants.Security.DefaultMemberTypeAlias;

                var identityUser = MemberIdentityUser.CreateNew(model.Email, model.Email, memberTypeAlias, isApproved: true, fullName);
                IdentityResult identityResult = await _memberManager.CreateAsync(
                    identityUser,
                    model.Password);

                var member = _memberService.GetByEmail(identityUser.Email);

                _logger.LogInformation("Register: Member created successfully");

                member.SetValue("firstName", model.FirstName);
                member.SetValue("lastName", model.LastName);
                member.IsApproved = true;

                _memberService.Save(member);

                _memberService.AssignRoles(new[] { member.Username }, new[] {"Member"});
            }

            TempData["Success"] = true;
            return RedirectToCurrentUmbracoPage();
        }
    }
}
