using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;
using Umbraco9Membership.Helpers;
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
            IMediaUploadService mediaUploadService, IMemberService memberService, 
            IUmbracoContextFactory umbracoContextFactory)
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
                Email = member.Value<string>("email"),
                JobTitle = member.JobTitle,
                FavouriteColour = member.FavouriteColour,
                Skills = member.Skills,
                Avatar = member.Avatar,
                Gallery = member.Gallery
            };
            
            return profile;
        }

        public IMember GetMemberFromUser(MemberIdentityUser user)
        {
            return user != null ? _memberService.GetByUsername(user.UserName) : null;
        }

        public ContentModels.Member GetMemberModelFromUser(MemberIdentityUser user)
        {
            using (var umbracoContext = _umbracoContextFactory.EnsureUmbracoContext())
            {
                var member = user != null ? _memberService.GetByUsername(user.UserName) : null;
                if (member == null) return null;
                return umbracoContext.UmbracoContext.PublishedSnapshot.Members.Get(member) as ContentModels.Member;
            }
        }

        public ContentModels.Member GetMemberModelFromMember(IMember member)
        {
            using (var umbracoContext = _umbracoContextFactory.EnsureUmbracoContext())
            {
                if (member == null) return null;
                return umbracoContext.UmbracoContext.PublishedSnapshot.Members.Get(member) as ContentModels.Member;
            }
        }

        public void UpdateProfile(EditProfileViewModel model, ContentModels.Member memberModel, IMember member)
        {
            member.SetValue("firstName", model.FirstName);
            member.SetValue("lastName", model.LastName);

            member.SetValue("jobTitle",
                JsonConvert.SerializeObject(new[] { model.JobTitle }));

            member.SetValue("skills",
                JsonConvert.SerializeObject(model.Skills));

            member.SetValue("favouriteColour", model.FavouriteColour);

            if (model.Avatar != null)
            {
                var avatarUdi = _mediaUploadService.CreateMediaItemFromFileUpload(model.Avatar, 1126, "Image");
                member.SetValue("avatar", avatarUdi);
            }

            List<string> galleryUdis = new List<string>();

            var galleryValue = member.GetValue<string>("gallery");

            if (!string.IsNullOrWhiteSpace(galleryValue))
            {
                JArray galleryArray = JsonConvert.DeserializeObject<JArray>(galleryValue);

                var sortOrderArray =
                model.GallerySortOrder.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.Parse(x)).ToArray();

                var sortedArray = new JArray();

                var numberOfItems = galleryArray.Count;
                foreach (var index in sortOrderArray)
                {
                    if (index < numberOfItems)
                    {
                        sortedArray.Add(galleryArray[index]);
                    }
                }

                var json = JsonConvert.SerializeObject(sortedArray);

                member.SetValue("gallery", json);
                galleryValue = json;
            }

            if (model.Gallery != null && model.Gallery.Any())
            {
                JArray galleryArray = null;
                if(!string.IsNullOrWhiteSpace(galleryValue))
                {
                    galleryArray = JsonConvert.DeserializeObject<JArray>(galleryValue);
                }
                else
                {
                    galleryArray = new JArray();
                }

                foreach (var item in model.Gallery.Where(x => x != null))
                {
                    var mediaKey = _mediaUploadService.CreateMediaItemFromFileUpload(item, 1126, "Image", returnUdi: false);

                    if(!string.IsNullOrWhiteSpace(mediaKey))
                    {
                        JObject galleryItem = new JObject();
                        galleryItem.Add("key", Guid.NewGuid().ToString());
                        galleryItem.Add("mediaKey", mediaKey);
                        galleryItem.Add("crops", null);
                        galleryItem.Add("focalPoint", null);

                        galleryArray.Add(galleryItem);
                    }
                }

                member.SetValue("gallery", galleryArray);
            }

            _memberService.Save(member);
        }
    }
}
