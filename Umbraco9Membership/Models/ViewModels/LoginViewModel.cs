using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Umbraco9Membership.Models.ViewModels
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
