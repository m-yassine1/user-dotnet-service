using System;
using System.ComponentModel.DataAnnotations;

namespace user_service.Annotations
{
    public class DateOfBirthAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value == null || (value is DateTime date && DateTime.Now <= date);
        }
    }
}
