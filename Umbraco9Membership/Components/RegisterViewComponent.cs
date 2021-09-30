using Microsoft.AspNetCore.Mvc;
using Umbraco9Membership.Models.ViewModels;

namespace Umbraco9Membership.Components
{
    [ViewComponent(Name = "Register")]
    public class RegisterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(RegisterViewModel model)
        {
            return View(model);
        }
    }
}
