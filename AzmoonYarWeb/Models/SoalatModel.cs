using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AzmoonYarWeb.Models
{
    public class SoalatModel
    {
        public SoalatModel()
        {
            PasokhHa = new List<PasokhModel>();
        }
        [Display(Name = "شناسه")]
        public int ID { get; set; }
          [Required(ErrorMessage = "متن سوال را وارد کنید")]

        [Display(Name = "متن سوال")]
        public string Question { get; set; }

        [Display(Name = "وضعیت")]
        public bool Status { get; set; }
        public DateTime CreateDateOnUtc { get; set; }
         [Display(Name = "تاریخ ایجاد")]
        public string CreateDateOnUtcJalali { get; set; }
        public int F_AzmoonId { get; set; }
        [Display(Name = "پاسخ صحیح سوال")]
        [Required(ErrorMessage = "پاسخ صحیح این سوال را انتخاب کنید")]
        public int Pasokh { get; set; }

        [Display(Name = "نام آزمون")]
        public string AzmoonName { get; set; }
        public List<PasokhModel> PasokhHa { get; set; }
        [Display(Name = "درصد افرادی که به این سوال درست پاسخ داده اند")]
        public string CorrectPercentage { get; set; }


        public int SoalNumber { get; set; }
    }
}