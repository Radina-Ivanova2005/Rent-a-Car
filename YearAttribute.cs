using System;
using System.ComponentModel.DataAnnotations;

namespace Rent_A_Car.Models.Attributes
{
    /// <summary>
    /// Validation attribute to ensure that a year value is not in the future.
    /// </summary>
    public class YearAttribute : ValidationAttribute
    {
        /// <summary>
        /// Determines whether the specified value is a valid year value.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns><c>true</c> if the value is a valid year or <c>null</c>; otherwise, <c>false</c>.</returns>
        public override bool IsValid(object value)
        {
            // If the value is null, it's considered valid
            if (value == null)
            {
                return true;
            }

            // Convert the value to an integer
            int year = (int)value;

            // Check if the year is less than or equal to the current year
            return year <= DateTime.Now.Year;
        }
    }
}
