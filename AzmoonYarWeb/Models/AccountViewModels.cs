using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AzmoonYarWeb.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور فعلی")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور جدید")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز عبور جدید")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "نام کاربری را وارد کنید")]
        public string UserName { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "نام را وارد کنید")]
        public string Name { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "نام خانوادگی را وارد کنید")]
        public string LastName { get; set; }

        [Display(Name = "شماره تماس")]
        [Required(ErrorMessage = "شماره تماس را وارد کنید")]
        public string Phone { get; set; }

        [EmailAddress]
        [Display(Name = "پست الکترونیکی")]
        [Required(ErrorMessage = "پست الکترونیکی را وارد کنید")]
        public string Email { get; set; }

        [Required(ErrorMessage = "رمز عبور را وارد کنید")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Required(ErrorMessage="تکرار رمز عبور را وارد کنید")]
        [DataType(DataType.Password)]
        [Display(Name = "تکرار رمز عبور")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }



    public class GoogleCaptcha
    {

        public bool isValid(string Payload)
        {
            var response = Payload;// 
            //secret that was generated in key value pair
            const string secret = "6LdY8AcUAAAAABJ0lfjKHh_a4b7M-zouo2w4qSOl";

            var client = new System.Net.WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            //when response is false check for the error message
            if (captchaResponse.Success)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        class CaptchaResponse
        {
            [Newtonsoft.Json.JsonProperty("success")]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; }
        }
    }
}
