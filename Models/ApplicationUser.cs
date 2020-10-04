using Microsoft.AspNetCore.Identity;
using System;

namespace user_service.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { set; get; }
        public DateTime? DateOfBirth { set; get; }
        public string Country { set; get; }
        public bool? IsActive { set; get; }
        public double? Salary { set; get; }
        public string Address { set; get; }
        public string ProfilePicture { set; get; }
        public override string ToString()
        {
            return Util.FormatToJsonBody(this);
        }
    }
}
