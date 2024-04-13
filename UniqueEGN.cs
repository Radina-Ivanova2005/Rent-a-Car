using Rent_A_Car.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Rent_A_Car.Models.Attributes
{
    /// <summary>
    /// Validation attribute to ensure uniqueness of the EGN (Unique Civil Registration Number).
    /// </summary>
    public class UniqueEGN : ValidationAttribute
    {
        /// <summary>
        /// Validates the uniqueness of the EGN.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A <see cref="ValidationResult"/> indicating whether the value is valid.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Retrieve the database context from the validation context
            RentACarDbContext dbContext = (RentACarDbContext)validationContext.GetService(typeof(RentACarDbContext));

            // Check if there is any user with the given EGN
            User user = dbContext.Users.SingleOrDefault(x => x.EGN == (string)value);

            // If a user with the EGN already exists, return an error message
            if (user != null)
            {
                return new ValidationResult("There is already a user with this EGN.");
            }

            // If the EGN is unique, return success
            return ValidationResult.Success;
        }
    }
}
