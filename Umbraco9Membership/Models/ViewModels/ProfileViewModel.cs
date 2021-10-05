using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Umbraco9Membership.Models.ViewModels
{
    public class ProfileViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string JobTitle { get; set; }
        public IEnumerable<string> Skills { get; set; }
        public string FavouriteColour { get; set; }
        public MediaWithCrops Avatar { get; set; }
        public IEnumerable<MediaWithCrops> Gallery { get; set; }
    }
}
