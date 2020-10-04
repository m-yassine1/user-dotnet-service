using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace user_service.Annotations
{
    public class CountryAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value == null || (value is string country && Constant.Countries.Any(c => c.Equals(country)));
        }
    }
}
