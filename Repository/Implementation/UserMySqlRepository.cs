using System;
using user_service.Data;
using user_service.Models;

namespace user_service.Repository.Implementation
{
    public class UserMySqlRepository : IUserRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public UserMySqlRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public void AddUser(ApplicationUser user)
        {
            _applicationDbContext.Add(user);
        }

        public void DeleteUser(ApplicationUser user)
        {
            _applicationDbContext.Remove(user);
        }

        public ApplicationUser GetUser(string username)
        {
            return (ApplicationUser)_applicationDbContext.Find(typeof(ApplicationUser), username);
        }

        public void UpdateUser(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
    }
}
