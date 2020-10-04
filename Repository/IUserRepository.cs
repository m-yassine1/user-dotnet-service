using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using user_service.Models;

namespace user_service.Repository
{
    public interface IUserRepository
    {
        public void AddUser(ApplicationUser user);
        public void UpdateUser(ApplicationUser user);
        public ApplicationUser GetUser(string username);
        public void DeleteUser(ApplicationUser user);
    }
}
