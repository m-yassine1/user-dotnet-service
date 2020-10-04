using System;
using System.ComponentModel.DataAnnotations;
using user_service.Annotations;

namespace user_service.Models
{
    public class ApplicationUserUpdateRequest
    {
        public string DisplayName { set; get; }
        [DateOfBirth(ErrorMessage = "Invalid date of birth")]
        public DateTime? DateOfBirth { set; get; }
        [Country(ErrorMessage = "Invalid country selected")]
        public string Country { set; get; }
        [Range(0, double.MaxValue, ErrorMessage = "Invalid salary value set")]
        public double? Salary { set; get; }
        public string Address { set; get; }
        [Base64(ErrorMessage = "Invalid base64 string for image upload")]
        public string ProfilePicture { set; get; }
        public override string ToString()
        {
            return Util.FormatToJsonBody(this);
        }
    }
}
