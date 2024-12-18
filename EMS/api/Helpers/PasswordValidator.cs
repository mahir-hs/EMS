using System.Text.RegularExpressions;
using System.Text;

namespace api.Helpers
{
    public static class PasswordValidator
    {
        private static readonly Regex _hasUpperCase = new Regex("[A-Z]", RegexOptions.Compiled);
        private static readonly Regex _hasLowerCase = new Regex("[a-z]", RegexOptions.Compiled);
        private static readonly Regex _hasDigit = new Regex("[0-9]", RegexOptions.Compiled);
        private static readonly Regex _hasSpecialChar = new Regex("[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]", RegexOptions.Compiled);
        public static string CheckPassword(string pass)
        {
            StringBuilder sb = new();

            if (pass.Length < 9)
                sb.Append("Minimum password length should be 9" + Environment.NewLine);

            if (!(_hasLowerCase.IsMatch(pass) && _hasUpperCase.IsMatch(pass) && _hasDigit.IsMatch(pass)))
                sb.Append("Password should be alphanumeric" + Environment.NewLine);

            if (!_hasSpecialChar.IsMatch(pass))
                sb.Append("Password should contain at least one special character" + Environment.NewLine);

            return sb.ToString();
        }
    }
}
