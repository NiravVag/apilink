using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace DTO.Helper
{
    public class ValidationHelper
    {
        public static ValidationResult ValidateEmail(string Email)
        {
            return !IsValidEmail(Email)
                ? new ValidationResult("Invalid Email")
                : ValidationResult.Success;
        }
        private static bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
