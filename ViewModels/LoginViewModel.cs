using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace TaskMangementSystem.ViewModels
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            ExternalLogins = [];
        }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
