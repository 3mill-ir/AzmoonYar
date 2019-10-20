using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AzmoonYarWeb.Models
{
    public class AzmoonModel
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }
        [Display(Name = "نام آزمون")]
        [Required(ErrorMessage = "نام آزمون را وارد کنید")]
        public string Name { get; set; }
        public DateTime CreateDateOnUtc { get; set; }
        public DateTime StartDateOnUtc { get; set; }
        public DateTime EndDateOnUtc { get; set; }
        public string F_UserId { get; set; }
        public string MasterName { get; set; }
        public bool Status { get; set; }
        [Display(Name = "تاریخ شروع")]
        [Required(ErrorMessage = "تاریخ شروع آزمون را وارد کنید")]
        public string StartDateOnUtcJalali { get; set; }
        [Display(Name = "تاریخ پایان")]
        [Required(ErrorMessage = "تاریخ پایان آزمون را وارد کنید")]
        public string EndDateOnUtcJalali { get; set; }
        public bool IsActive { get; set; }
    }
}