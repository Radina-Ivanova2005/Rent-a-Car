using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Rent_A_Car.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Rent_A_Car.Models.Attributes;


namespace Rent_A_Car.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [MinLength(10, ErrorMessage = "Phone number must be 10 symbols long.")]
            [MaxLength(10, ErrorMessage = "Phone number must be 10 symbols long.")]
            [Display(Name = "Phone number")]
            [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must contain only numbers.")]
            [Phone]
            [UniquePhoneNumber]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [Display(Name = "First name")]
            [MinLength(3, ErrorMessage = "First name must be 3 symbols long.")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last name")]
            [MinLength(3, ErrorMessage = "Last name must be 3 symbols long.")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [UniqueEmail]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required]
            [MinLength(10, ErrorMessage = "EGN must be 10 symbols long.")]
            [MaxLength(10, ErrorMessage = "EGN must be 10 symbols long.")]
            [Display(Name = "EGN")]
            [RegularExpression("^[0-9]*$", ErrorMessage = "EGN must contain only numbers.")]
            [UniqueEGN]
            public string EGN { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new User {FirstName = Input.FirstName, LastName = Input.LastName, PhoneNumber = Input.PhoneNumber, EGN = Input.EGN, UserName = Input.Email, Email = Input.Email };
                user.EmailConfirmed = true;
                var result = await _userManager.CreateAsync(user, Input.Password);
                await _userManager.AddToRoleAsync(user, "Customer");
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");


                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
