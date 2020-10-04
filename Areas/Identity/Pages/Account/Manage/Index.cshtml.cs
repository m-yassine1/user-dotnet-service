using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using user_service.Annotations;
using user_service.Models;
using user_service.Service.Implementation;

namespace user_service.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationService _applicationService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationService applicationService,
            ILogger<IndexModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _applicationService = applicationService;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            [RegularExpression(@"^\+?\d{8,14}$", ErrorMessage = "Invalid Mobile Number")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Display Name")]
            public string DisplayName { get; set; }
            [Display(Name = "Date of Birth")]
            [DateOfBirth(ErrorMessage = "Invalid date of birth selected")]
            public DateTime? DateOfBirth { get; set; }
            [Display(Name = "Address")]
            public string Address { get; set; }
            [Display(Name = "Country")]
            [Country]
            public string Country { get; set; }
            [Display(Name = "Salary")]
            [Range(0, double.MaxValue, ErrorMessage = "Invalid salary value set")]
            public double? Salary { get; set; }
            [Display(Name = "Profile Picture")]
            public IFormFile ProfilePicture { get; set; }
            [IgnoreMap]
            public string ProfilePictureUrl { set; get; }

            [IgnoreMap]
            public List<SelectListItem> Countries { get; } = Constant.Countries;
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Address = user.Address,
                Country = user.Country,
                DateOfBirth = user.DateOfBirth,
                DisplayName = user.DisplayName,
                ProfilePictureUrl = user.ProfilePicture == null ? null : $"/images/{user.ProfilePicture}",
                Salary = user.Salary
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            user.Country = Input.Country ?? user.Country;
            user.PhoneNumber = Input.PhoneNumber ?? user.PhoneNumber;
            user.Salary = Input.Salary ?? user.Salary;
            user.DisplayName = Input.DisplayName ?? user.DisplayName;
            user.DateOfBirth = Input.DateOfBirth ?? user.DateOfBirth;
            user.Address = Input.Address ?? user.Address;
            user.ProfilePicture = _applicationService.StoreUploadedImage(Input.ProfilePicture) ?? user.ProfilePicture;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to update user.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
