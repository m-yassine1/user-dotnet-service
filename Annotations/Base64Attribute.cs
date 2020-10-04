using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace user_service.Annotations
{
    public class Base64Attribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            try
            {
                if(value == null)
                {
                    return true;
                }
                if (!(value is string base64Value) || string.IsNullOrEmpty(base64Value))
                {
                    return false;
                }
                // If no exception is caught, then it is possibly a base64 encoded string
                // The part that checks if the string was properly padded to the
                // correct length was borrowed from d@anish's solution
                var base64 = base64Value.Split(",");
                return Convert.FromBase64String(base64.Length > 1 ? base64.Last() : base64.First()) != null;
            }
            catch
            {
                // If exception is caught, then it is not a base64 encoded string
                return false;
            }
        }
    }
}
