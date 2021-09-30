using System;
using System.ComponentModel.DataAnnotations;

namespace Umbraco9Membership.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MustBeTrue : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && value is bool && (bool)value;
        }
    }

    /// <summary>
    /// Checks if the value is equal to null or is an empty string
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MustBeEmpty : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value == null || (value is string && (string)value == "");
        }
    }

    /// <summary>
    /// Checks if the value is not null and is an integer which is greater than zero
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class GreaterThanZero : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && value is int && (int)value > 0;
        }
    }
}
