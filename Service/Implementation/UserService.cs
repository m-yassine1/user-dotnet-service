using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using user_service.Models;
using user_service.Repository;

namespace user_service.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IApplicationService _applicationService;
        private readonly ILogger<UserService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UserService(IConfiguration configuration,
            IUserRepository userRepository,
            ILogger<UserService> logger,
            IApplicationService applicationService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationService = applicationService;
        }

        public async Task<ActionResult<MessageResponse>> CreateUser(ApplicationUserRegisterRequest user)
        {
            try
            {
                if (_userRepository.GetUser(user.Username) != null)
                {
                    return new BadRequestObjectResult(new MessageResponse { Message = $"User {user.Username} already exists" });
                }

                ApplicationUser registeredUser = new ApplicationUser
                {
                    Salary = user.Salary,
                    ProfilePicture = _applicationService.StoreUploadedImage(user.ProfilePicture),
                    Address = user.Address,
                    Country = user.Country,
                    DateOfBirth = user.DateOfBirth,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    UserName = user.Username
                };

                await _userManager.CreateAsync(registeredUser, user.Password);
                if (_configuration.GetValue<bool?>("signInUserOnregister") != null && _configuration.GetValue<bool?>("signInUserOnregister").Value)
                {
                    await _signInManager.SignInAsync(registeredUser, isPersistent: false);
                }

                return new CreatedResult($"/usernames{user.Username}", registeredUser);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new BadRequestObjectResult(_applicationService.GetExceptionMessage(e));
            }
        }

        public async Task<ActionResult<MessageResponse>> DeleteUser(string username)
        {
            try
            {
                ApplicationUser registeredUser = new ApplicationUser
                {
                    UserName = username
                };
                await _userManager.DeleteAsync(registeredUser);
                return new OkObjectResult(new MessageResponse { Message = $"User {username} deleted successfully" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new BadRequestObjectResult(_applicationService.GetExceptionMessage(e));
            }
        }

        public ActionResult<ApplicationUserResponse> GetUser(string username)
        {
            try
            {
                ApplicationUser user = _userRepository.GetUser(username);
                if (user == null)
                {
                    return new NotFoundObjectResult(new MessageResponse { Message = $"User {username} does not exist" });
                }

                return new OkObjectResult(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new BadRequestObjectResult(_applicationService.GetExceptionMessage(e));
            }
        }

        public async Task<ActionResult<MessageResponse>> LoginUser(LoginRequest request)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, false);

                if (result.Succeeded)
                {
                    return new OkObjectResult(new MessageResponse { Message = $"User {request.Username} has logged in successfully" });
                }

                return new UnauthorizedObjectResult(new MessageResponse { Message = $"User {request.Username} put invalid credentials" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new BadRequestObjectResult(_applicationService.GetExceptionMessage(e));
            }
        }

        public async Task<ActionResult<MessageResponse>> UpdateUser(string username, ApplicationUserUpdateRequest user)
        {
            try
            {
                ApplicationUser registeredUser = _userRepository.GetUser(username);
                if (registeredUser == null)
                {
                    return new NotFoundObjectResult(new MessageResponse { Message = $"User {username} does not exist" });
                }

                registeredUser = new ApplicationUser
                {
                    Salary = user.Salary ?? registeredUser.Salary,
                    ProfilePicture = _applicationService.StoreUploadedImage(user.ProfilePicture) ?? registeredUser.ProfilePicture,
                    Address = user.Address ?? registeredUser.Address,
                    Country = user.Country ?? registeredUser.Country,
                    DateOfBirth = user.DateOfBirth ?? registeredUser.DateOfBirth,
                    DisplayName = user.DisplayName ?? registeredUser.DisplayName
                };

                await _userManager.UpdateAsync(registeredUser);

                return new OkObjectResult(new MessageResponse { Message = $"User {username} has been updated successfully" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new BadRequestObjectResult(_applicationService.GetExceptionMessage(e));
            }
        }
    }
}
