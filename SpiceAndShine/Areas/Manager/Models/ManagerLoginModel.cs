using System.ComponentModel.DataAnnotations;

namespace SpiceAndShine.Areas.Manager.Models
{
    public class ManagerLoginModel
    {
        [Required(ErrorMessage = "Please enter email or mobile number")]
        public string EmailOrMobileNo { get; set; }

        [Required(ErrorMessage = "Please enter password.")]
        [StringLength(16, ErrorMessage = "Please enter value between 8 to 16 character.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.{8,32})(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).*$", ErrorMessage = "Minimum 8 characters at least 1 Uppercase Alphabet,1 Lowercase Alphabet,1 Number and 1 Special character.")]
        public string Password { get; set; }
    }
    public class ManagerLoginResult
    {
        public ManagerSession ManagerData { get; set; }
        public int Flag { get; set; }
    }
    public class ManagerSession
    {
        public int ManagerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; } 
    }
    public class LoginStatus
    {
        public int Result { get; set; }
    }
    public enum ManagerLoginEnum
    {
        UserNotAvailable = 1,
        ContactAdmin = 2,
        PasswordMissmatch = 3,
        Active = 4
    }
}
