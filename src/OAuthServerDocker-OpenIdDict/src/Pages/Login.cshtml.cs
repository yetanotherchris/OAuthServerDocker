using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAuthServerDocker.OpenIdDict.Pages
{
    public class Login : PageModel
    {
        private readonly SignInManager<ApplicationUser> _manager;

        [BindProperty]
        public string ReturnUrl { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string OriginalQuery { get; set; }

        public Login(SignInManager<ApplicationUser> manager)
        {
            _manager = manager;
        }

        public void OnGet()
        {
            ReturnUrl = Request.Query["returnUrl"];
            OriginalQuery = Request.QueryString.Value;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Login loginModel = this;
            //string returnUrl = Request.Form["returnUrl"];
            //string email = Request.Form["email"];
            //string password = Request.Form["password"];

            var result = await _manager.PasswordSignInAsync(loginModel.Email, loginModel.Password, true, true);
            return new RedirectResult(loginModel.ReturnUrl + "" + loginModel.OriginalQuery);
        }
    }
}