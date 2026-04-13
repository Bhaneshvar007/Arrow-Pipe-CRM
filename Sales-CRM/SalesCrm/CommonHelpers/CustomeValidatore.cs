namespace SalesCrm.CommonHelpers
{
    public class CustomeValidatore
    {
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            return System.Text.RegularExpressions.Regex.IsMatch(name, @"^[A-Za-z\s]+$");
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$");
        }

        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Digits + + - space ()
            return System.Text.RegularExpressions.Regex.IsMatch(phone, @"^[0-9+\-\s()]+$");
        }

        public static string ExtractDomain(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return string.Empty;

            email = email.Trim();

            int atIndex = email.IndexOf('@');

            if (atIndex == -1 || atIndex == email.Length - 1)
                return string.Empty;

            return email.Substring(atIndex + 1);
        }
    }
}
