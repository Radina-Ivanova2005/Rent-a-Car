using System;
using System.ComponentModel.DataAnnotations;
using Rent_A_Car.Models.Attributes;

namespace Rent_A_Car.Models
{
    /// <summary>
    /// Represents a car entity.
    /// </summary>
    public class Car
    {
        /// <summary>
        /// Gets or sets the ID of the car.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the brand of the car.
        /// </summary>
        [Required(ErrorMessage = "This field is required")]
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets the model of the car.
        /// </summary>
        [Required(ErrorMessage = "This field is required")]
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the year of the car.
        /// </summary>
        [Display(Name = "Year:")]
        [YearAttribute(ErrorMessage = "Time cannot be set past the present.")]
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the number of seats in the car.
        /// </summary>
        [Required]
        [Display(Name = "Number of seats:")]
        [Range(0, int.MaxValue, ErrorMessage = "Seats cannot be a negative number.")]
        public int Seats { get; set; }

        /// <summary>
        /// Gets or sets additional information about the car.
        /// </summary>
        [Display(Name = "Info:")]
        public string Info { get; set; }

        /// <summary>
        /// Gets or sets the price per day of renting the car.
        /// </summary>
        [Required]
        [Display(Name = "Price per day:")]
        [Range(0, double.MaxValue, ErrorMessage = "Price per day cannot be negative.")]
        public double PricePerDay { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Car"/> class.
        /// </summary>
        public Car()
        {
        }
    }
}
