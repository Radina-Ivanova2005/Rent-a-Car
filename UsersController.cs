using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent_A_Car.Data;
using Rent_A_Car.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rent_A_Car.Controllers
{
    /// <summary>
    /// Controller for managing user-related actions.
    /// </summary>
    public class UsersController : Controller
    {
        private readonly RentACarDbContext _context;
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="userManager">The user manager.</param>
        public UsersController(RentACarDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Displays the list of users.
        /// </summary>
        /// <returns>The index view.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        /// <summary>
        /// Displays details of a specific user.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The details view.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        /// <summary>
        /// Displays the form for editing a user.
        /// </summary>
        /// <param name="id">The ID of the user to edit.</param>
        /// <returns>The edit view.</returns>
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditViewModel model = new EditViewModel();
            model.User = user;
            return View(model);
        }

        /// <summary>
        /// Handles the post request for editing a user.
        /// </summary>
        /// <param name="id">The ID of the user to edit.</param>
        /// <param name="password">The new password.</param>
        /// <param name="user">The user object with updated properties.</param>
        /// <returns>The index view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string password, [Bind("Id,EGN,PasswordHash,Email,UserName")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    User user2 = await _context.Users.FindAsync(id);
                    user2.EGN = user.EGN;
                    user2.Email = user.Email;
                    user2.UserName = user.UserName;
                    if (password != null)
                    {
                        string newPassword = _userManager.PasswordHasher.HashPassword(user, password);
                        user2.PasswordHash = newPassword;
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        /// <summary>
        /// Displays the confirmation page for deleting a user.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>The delete view.</returns>
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        /// <summary>
        /// Deletes the user from the database.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>The index view.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        /// <summary>
        /// ViewModel for editing user information.
        /// </summary>
        public class EditViewModel
        {
            /// <summary>
            /// Gets or sets the user.
            /// </summary>
            public User User { get; set; }

            /// <summary>
            /// Gets or sets the password.
            /// </summary>
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
        }
    }
}
