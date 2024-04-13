using Rent_A_Car.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Rent_A_Car.Models.Attributes
{
    /// <summary>
    /// Validation attribute to ensure uniqueness of phone numbers.
    /// </summary>
    public class UniquePhoneNumber : ValidationAttribute
    {
        /// <summary>
        /// Validates the uniqueness of the phone number.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the value is valid.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Retrieve the database context from the validation context
            RentACarDbContext dbContext = (RentACarDbContext)validationContext.GetService(typeof(RentACarDbContext));

            // Check if there is any user with the given phone number
            User user = dbContext.Users.SingleOrDefault(x => x.PhoneNumber == (string)value);

            // If a user with the phone number already exists, return an error message
            if (user != null)
            {
                return new ValidationResult("There is already a user with this phone number.");
            }

            // If the phone number is unique, return success
            return ValidationResult.Success;
        }
    }
}
