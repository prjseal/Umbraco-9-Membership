using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Security;
using Umbraco9Membership.Models.ViewModels;
using ContentModels = Umbraco.Cms.Web.Common.PublishedModels;

namespace Umbraco9Membership.Services
{
    public interface IAccountService
    {
        ProfileViewModel GetEnrichedProfile(ContentModels.Member member);

        ContentModels.Member GetMemberModelFromUser(MemberIdentityUser user);

        ContentModels.Member GetMemberModelFromMember(IMember member);

        IMember GetMemberFromUser(MemberIdentityUser user);

        void UpdateProfile(EditProfileViewModel model, ContentModels.Member memberModel, IMember member);
    }
}
