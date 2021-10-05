using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Umbraco9Membership.Models.ViewModels
{
    public class EditProfileViewModel
    {
        [Display(Name = "Avatar")]
        public IFormFile Avatar { get; set; }

        public string AvatarUrl { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Display(Name = "Skills")]
        public IEnumerable<string> Skills { get; set; }

        [Display(Name = "Favourite Colour")]
        public string FavouriteColour { get; set; }

        public IEnumerable<SelectListItem> SkillsOptions { get; set; }

        public IEnumerable<SelectListItem> JobTitleOptions { get; set; }

        public IEnumerable<SelectListItem> FavouriteColourOptions { get; set; }

        [Display(Name = "Gallery")]
        public IFormFile[] Gallery { get; set; }

        public IEnumerable<IPublishedContent> CurrentGalleryItems { get; set; }

        [Display(Name = "Overwrite existing gallery images")]
        public bool OverwriteGalleryImages { get; set; }

        public string GallerySortOrder { get; set; }
    }
}
