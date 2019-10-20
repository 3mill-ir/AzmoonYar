using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AzmoonYarWeb.Models
{
    public class StudentModel
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "نام را وارد کنید")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "نام خانوادگی را وارد کنید")]
        public string LastName { get; set; }

        [Display(Name = "شماره تماس")]
        [Required(ErrorMessage = "شماره تماس را وارد کنید")]
        public string Tell { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "کد ملی را وارد کنید")]
        public string SSN { get; set; }

        [Display(Name = "وضعیت")]
        public bool Status { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreatedDateOnUtc { get; set; }

        [Display(Name = "آخرین بروز رسانی")]
        public DateTime EditDateOnUtc { get; set; }

        //[Required(ErrorMessage = "فایل اکسل مورد نظر را وارد کنید")]
        public HttpPostedFileBase ExcelFile { get; set; }
    }
}