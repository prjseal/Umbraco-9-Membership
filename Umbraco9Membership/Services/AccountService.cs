using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;
using Umbraco9Membership.Models.ViewModels;
using ContentModels = Umbraco.Cms.Web.Common.PublishedModels;

namespace Umbraco9Membership.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMemberGroupService _memberGroupService;
        private readonly IMediaUploadService _mediaUploadService;
        private readonly IMemberService _memberService;
        private readonly IUmbracoContextFactory _umbracoContextFactory;

        public AccountService(IMemberGroupService memberGroupService,
            IMediaUploadService mediaUploadService, IMemberService memberService, IUmbracoContextFactory umbracoContextFactory)
        {
            _memberGroupService = memberGroupService;
            _mediaUploadService = mediaUploadService;
            _memberService = memberService;
            _umbracoContextFactory = umbracoContextFactory;
        }


        public ProfileViewModel GetEnrichedProfile(ContentModels.Member member)
        {
            if (member == null) return null;

            var profile = new ProfileViewModel
            {
                FirstName = member.FirstName,
                LastName = member.LastName,
                Email = member.Value<string>("email")
            };

            //var jobTitleJson = member.GetValue<string>("jobTitle");
            //var jobTitleArray = !string.IsNullOrWhiteSpace(jobTitleJson)
            //    ? JsonConvert.DeserializeObject<IEnumerable<string>>(jobTitleJson)
            //    : null;
            //profile.JobTitle = jobTitleArray?.FirstOrDefault();

            //var skillsJson = member.GetValue<string>("skills");
            //var skills = !string.IsNullOrWhiteSpace(skillsJson)
            //    ? JsonConvert.DeserializeObject<IEnumerable<string>>(skillsJson)
            //    : null;
            //profile.Skills = skills;

            //profile.FavouriteColour = member.GetValue<string>("favouriteColour");

            //var avatarValue = member.GetValue<string>("avatar");

            //var avatar = JsonConvert.DeserializeObject<IEnumerable<MediaWithCrops>>(avatarValue);
            profile.Avatar = member.Avatar;

            //var galleryStringValue = member.GetValue<string>("gallery");

            //List<MediaWithCrops> gallery = new List<MediaWithCrops>();
            //if (!string.IsNullOrWhiteSpace(galleryStringValue))
            //{
            //    var galleryUdis = galleryStringValue.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            //    foreach (var galleryUdi in galleryUdis)
            //    {
            //        var udi = Udi.Create(Constants.UdiEntityType.Media, galleryUdi);
            //        var media = _umbracoHelper.Media(udi);
            //        if (media != null)
            //        {
            //            gallery.Add(media);
            //        }
            //    }

            //    profile.Gallery = gallery;
            //}

            return profile;
        }

        public ContentModels.Member GetMemberFromUser(MemberIdentityUser user)
        {
            using (var umbracoContext = _umbracoContextFactory.EnsureUmbracoContext())
            {
                var member = user != null ? _memberService.GetByUsername(user.UserName) : null;
                if (member == null) return null;
                return umbracoContext.UmbracoContext.PublishedSnapshot.Members.Get(member) as ContentModels.Member;
            }
        }
    }
}
