using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AzmoonYarWeb.Models
{
    public class MasterModel
    {
        [Display(Name = "شناسه")]
        public string ID { get; set; }

        [Required(ErrorMessage = "نام را وارد کنید")]
        [Display(Name = "نام")]
        public string FirstName { get; set; }

        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "نام خانوادگی را وارد کنید")]
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "شماره تماس را وارد کنید")]
        [Display(Name = "شماره تماس")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "پست الکترونیکی را وارد کنید")]
        [EmailAddress]
        [Display(Name = "پست الکترونیکی")]
        public string Email { get; set; }
    }
}