using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Security;
using Umbraco9Membership.Models.ViewModels;
using Umbraco9Membership.Services;

namespace Umbraco9Membership.Components
{
    [ViewComponent(Name = "Profile")]
    public class ProfileViewComponent : ViewComponent
    {
        private readonly IAccountService _accountService;

        public ProfileViewComponent(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IViewComponentResult Invoke(MemberIdentityUser user)
        {
            var member = _accountService.GetMemberModelFromUser(user); 

            var enrichedProfile = _accountService.GetEnrichedProfile(member);

            return View(enrichedProfile);
        }
    }
}
