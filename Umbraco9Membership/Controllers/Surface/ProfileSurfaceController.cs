using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Mail;
using Umbraco.Cms.Core.Models.Email;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Extensions;
using Umbraco9Membership.Models.ViewModels;
using Umbraco9Membership.Services;

namespace Umbraco9Membership.Controllers.Surface
{
    public class ProfileSurfaceController : SurfaceController
    {
        private readonly IMemberManager _memberManager;
        private readonly IMemberService _memberService;
        private readonly ILogger<ProfileSurfaceController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly GlobalSettings _globalSettings;
        private readonly IAccountService _accountService;

        public ProfileSurfaceController(
            //these are required by the base controller
            IUmbracoContextAccessor umbracoContextAccessor, 
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services, 
            AppCaches appCaches,
            IProfilingLogger profilingLogger, 
            IPublishedUrlProvider publishedUrlProvider,
            //these are dependencies we've added
            IMemberManager memberManager,
            IMemberService memberService,
            ILogger<ProfileSurfaceController> logger,
            IEmailSender emailSender,
            IOptions<GlobalSettings> globalSettings,
            IAccountService accountService
            ) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _memberManager = memberManager ?? throw new ArgumentNullException(nameof(memberManager));
            _memberService = memberService ?? throw new ArgumentNullException(nameof(memberService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _globalSettings = globalSettings?.Value ?? throw new ArgumentNullException(nameof(globalSettings));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
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
                await SendAlreadyRegisteredEmail(model);
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

        public async Task<bool> SendAlreadyRegisteredEmail(RegisterViewModel model)
        {
            try
            {
                var body =
                    "Somebody tried to register an account using your email address, but you are already registered.";

                var fromAddress = _globalSettings.Smtp.From;

                var subject = string.Format("Already Registered");
                var message = new EmailMessage(fromAddress, model.Email, subject, body, false);
                await _emailSender.SendAsync(message, emailType: "AlreadyRegistered");

                _logger.LogInformation("Already Registered Email Sent Successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error When Trying To Send Already Registered Email");
                return false;
            }
        }

        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToCurrentUmbracoPage();

            var user = await _memberManager.GetCurrentMemberAsync();

            var member = _accountService.GetMemberFromUser(await _memberManager.GetCurrentMemberAsync());
            
            if (member == null) return RedirectToCurrentUmbracoPage();

            var memberModel = _accountService.GetMemberModelFromMember(member);

            if (memberModel == null) return RedirectToCurrentUmbracoPage();

            _accountService.UpdateProfile(model, memberModel, member);

            TempData["EditProfileSuccess"] = true;

            var profilePage = CurrentPage.Parent;

            return RedirectToUmbracoPage(profilePage);
        }
    }
}
