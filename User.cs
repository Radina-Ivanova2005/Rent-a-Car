using Rent_A_Car.Models.Attributes;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Rent_A_Car.Models
{
    public class User : IdentityUser
    {
        
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [MinLength(10, ErrorMessage = "EGN must be 10 symbols long.")]
        [MaxLength(10, ErrorMessage = "EGN must be 10 symbols long.")]
        [Display(Name = "EGN")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "EGN must contain only numbers.")]
        [UniqueEGN]
        public string EGN { get; set; }
        [UniquePhoneNumber]
        public string PhoneNumber { get; set; }
        [UniqueEmail]
        public string Email { get; set; }
    }
}
