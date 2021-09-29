using Microsoft.AspNetCore.Mvc;
using Umbraco9Membership.Models.ViewModels;

namespace Umbraco9Membership.Components
{
    [ViewComponent(Name = "Login")]
    public class LoginViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(LoginViewModel model)
        {
            return View(model);
        }
    }
}
