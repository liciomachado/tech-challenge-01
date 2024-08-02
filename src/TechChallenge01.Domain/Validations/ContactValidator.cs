using System.Text.RegularExpressions;

namespace TechChallenge01.Domain.Validations
{
    public class ContactValidator
    {
        public static bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        public static bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }
    }
}
