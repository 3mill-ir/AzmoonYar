using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AzmoonYarWeb.Models
{
    public class PasokhModel
    {
        [Display(Name = "شناسه")]
        public int ID { get; set; }

        [Display(Name = "کلید پاسخ")]
        public int AnswerKey { get; set; }

        [Display(Name = "متن گزینه پاسخ")]
        [Required(ErrorMessage = "متن گزینه پاسخ را وارد کنید")]
        public string AnswerText { get; set; }

        [Display(Name = "امتیاز")]
        public int Score { get; set; }

        public int F_SoalatId { get; set; }

        [Display(Name = "آخرین تاریخ به روز رسانی")]
        public DateTime EditDateOnUtc { get; set; }
    }
}