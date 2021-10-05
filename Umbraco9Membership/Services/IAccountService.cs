using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Security;
using Umbraco9Membership.Models.ViewModels;
using ContentModels = Umbraco.Cms.Web.Common.PublishedModels;

namespace Umbraco9Membership.Services
{
    public interface IAccountService
    {
        ProfileViewModel GetEnrichedProfile(ContentModels.Member member);

        ContentModels.Member GetMemberFromUser(MemberIdentityUser user);
    }
}
