using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using user_service.Models;

namespace user_service.Service
{
    public interface IUserService
    {
        public ActionResult<ApplicationUserResponse> GetUser(string username);
        public Task<ActionResult<MessageResponse>> CreateUser(ApplicationUserRegisterRequest user);
        public Task<ActionResult<MessageResponse>> UpdateUser(string username, ApplicationUserUpdateRequest user);
        public Task<ActionResult<MessageResponse>> DeleteUser(string username);
        public Task<ActionResult<MessageResponse>> LoginUser(LoginRequest request);
    }
}
