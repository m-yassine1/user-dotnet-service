using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace user_service
{
    public static class Constant
    {
        public enum ImageFormat
        {
            bmp,
            jpeg,
            gif,
            tiff,
            png,
            unknown
        }

        public const string UsernameRegex = @"^[a-zA-Z][\w-]{4,10}$";
        public static List<SelectListItem> Countries = new List<SelectListItem>
            {
                new SelectListItem { Value = "MX", Text = "Mexico" },
                new SelectListItem { Value = "CA", Text = "Canada" },
                new SelectListItem { Value = "US", Text = "USA"  },
            };
    }
}
