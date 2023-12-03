using Identity.Constants;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() { }
        public ApplicationUser(string firstName, string lastName, string email)
        {
            SetFirstName(firstName);
            SetLastName(lastName);
            Email = email;
        }

        public string FirstName { get; private set; }
        public void SetFirstName(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException("First Name cann't be empty");
            }
            var regex = new Regex(RegularExpressions.ALPHABETS_AND_SPACE_ONLY);
            var match = regex.Match(firstName);
            if (!match.Success)
            {
                throw new ArgumentNullException("First Name must be alphabets and space only");
            }

            FirstName = firstName;
        }

        public string LastName { get; private set; }

        public void SetLastName(string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException("Last Name cann't be empty");
            }
            var regex = new Regex(RegularExpressions.ALPHABETS_AND_SPACE_ONLY);
            var match = regex.Match(lastName);
            if (!match.Success)
            {
                throw new ArgumentNullException("Last Name must be alphabets and space only");
            }

            LastName = lastName;
        }

        public string? RefreshToken { get; set; }
    }
}
