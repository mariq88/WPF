using System;
using System.Linq;
using System.Net.Mail;

namespace Spammer
{
    public class Validator
    {
        private const int MinUsernamePasswordLength = 6;
        private const int MaxUsernamePasswordLength = 30;
        private const int AuthenticationCodeLength = 40;
        private const string ValidUsernameChars = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890_.@";

        public static void ValidateUsername(string username)
        {
            if (username.Length < MinUsernamePasswordLength || MaxUsernamePasswordLength < username.Length)
            {
                throw new FormatException(
                    string.Format("Username must be between {0} and {1} characters",
                        MinUsernamePasswordLength,
                        MaxUsernamePasswordLength));
            }
            if (username.Any(ch => !ValidUsernameChars.Contains(ch)))
            {
                throw new FormatException("Username contains invalid characters");
            }
        }

        public static void ValidatePassword(string password)
        {
            if (password.Length < MinUsernamePasswordLength || MaxUsernamePasswordLength < password.Length)
            {
                throw new FormatException(
                    string.Format("Password must be between {0} and {1} characters",
                        MinUsernamePasswordLength,
                        MaxUsernamePasswordLength));
            }
        }

        public static void ValidateEmail(string email)
        {
            try
            {
                new MailAddress(email);
            }
            catch (FormatException ex)
            {
                throw new FormatException("Email is invalid", ex);
            }
        }

        public static void ValidateAuthCode(string authCode)
        {
            if (string.IsNullOrEmpty(authCode) || authCode.Length != AuthenticationCodeLength)
            {
                throw new FormatException("Password is invalid");
            }
        }
    }
}
