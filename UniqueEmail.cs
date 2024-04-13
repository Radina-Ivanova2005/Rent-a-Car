using Rent_A_Car.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Rent_A_Car.Models.Attributes
{
    /// <summary>
    /// Validation attribute to ensure uniqueness of email addresses.
    /// </summary>
    public class UniqueEmail : ValidationAttribute
    {
        /// <summary>
        /// Validates the uniqueness of the email address.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the value is valid.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Retrieve the database context from the validation context
            RentACarDbContext dbContext = (RentACarDbContext)validationContext.GetService(typeof(RentACarDbContext));

            // Check if there is any user with the given email address
            User user = dbContext.Users.SingleOrDefault(x => x.Email == (string)value);

            // If a user with the email address already exists, return an error message
            if (user != null)
            {
                return new ValidationResult("There is already a user with this email.");
            }

            // If the email address is unique, return success
            return ValidationResult.Success;
        }
    }
}
