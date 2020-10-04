using System.ComponentModel.DataAnnotations;

namespace user_service.Models
{
    public class ApplicationUserRegisterRequest : ApplicationUserUpdateRequest
    {
        [EmailAddress(ErrorMessage = "Invalid email address put")]
        public string Email { set; get; }

        [RegularExpression(Constant.UsernameRegex, ErrorMessage = "Invalid username")]
        [Required(ErrorMessage = "username is required")]
        public string Username { set; get; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { set; get; }
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { set; get; }
        public override string ToString()
        {
            return Util.FormatToJsonBody(this);
        }
    }
}
