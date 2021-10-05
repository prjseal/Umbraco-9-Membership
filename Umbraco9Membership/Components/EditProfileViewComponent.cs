using Microsoft.AspNetCore.Mvc;
using Umbraco9Membership.Models.ViewModels;

namespace Umbraco9Membership.Components
{
    [ViewComponent(Name = "EditProfile")]
    public class EditProfileViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(EditProfileViewModel model)
        {
            return View(model);
        }
    }
}
