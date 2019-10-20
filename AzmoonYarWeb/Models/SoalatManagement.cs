using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShahrdari.Areas.Admin3mill.Models;
using System.Data.Entity;


namespace AzmoonYarWeb.Models
{
    public class SoalatManagement
    {
        public List<SoalatModel> ListSoalat(int AzmoonId)
        {
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                string F_UserId = Tools.F_UserId();
                var ListObject = (F_UserId != "71ffec0a-1d92-4bc3-8553-ffbff55b885f") ? db.Soalats.Where(m => m.Status == true && m.Azmoon.F_UserId == F_UserId && m.F_AzmoonId == AzmoonId).OrderBy(m => m.CreateDateOnUtc) : db.Soalats.Where(m => m.Status == true && m.F_AzmoonId == AzmoonId).OrderBy(m => m.CreateDateOnUtc);
                List<SoalatModel> list = new List<SoalatModel>();
                foreach (var ListItem in ListObject)
                {
                    SoalatModel t = new SoalatModel();
                    DateTime creat = new DateTime();
                    creat = ListItem.CreateDateOnUtc ?? default(DateTime);
                    t.CreateDateOnUtcJalali = Tools.GetDateTimeReturnJalaliDate(creat);
                    t.ID = ListItem.Id;
                    t.F_AzmoonId = ListItem.F_AzmoonId ?? default(int);
                    t.Question = ListItem.Question;
                    t.Status = ListItem.Status ?? default(bool);
                    t.Pasokh = ListItem.Pasokh ?? default(int);
                    t.SoalNumber = ListItem.SoalNumber ?? default(int);
                    list.Add(t);
                }
                return list;
            }
        }

        public int AddSoalat(SoalatModel model)
        {
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                Soalat InsertObject = new Soalat();
                InsertObject.CreateDateOnUtc = DateTime.Now;
                InsertObject.Question = model.Question;
                InsertObject.F_AzmoonId = model.F_AzmoonId;
                InsertObject.Status = true;
                InsertObject.Pasokh = model.Pasokh;
                var soalat = ListSoalat(model.F_AzmoonId).LastOrDefault();
                if (soalat != null)
                {
                    InsertObject.SoalNumber = soalat.SoalNumber+1;
                }
                else
                {
                    InsertObject.SoalNumber = 1;
                }
                db.Soalats.Add(InsertObject);
                db.SaveChanges();
                int t = 1;
                foreach (var item in model.PasokhHa)
                {
                    Pasokh p = new Pasokh();
                    p.F_SoalatId = InsertObject.Id;
                    p.AnswerText = item.AnswerText;
                    p.AnswerKey = t++;
                    p.Score = 0;
                    db.Pasokhs.Add(p);
                }
                db.SaveChanges();
                return 1;
            }
        }

        public string EditSoalat(SoalatModel model)
        {
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                var EditObject = db.Soalats.Include(e => e.Pasokhs).FirstOrDefault(u => u.Id == model.ID && u.Status == true);
                if (EditObject == null) { return "NotFound"; }
                EditObject.Question = model.Question;
                EditObject.Pasokh = model.Pasokh;
                foreach (var item in model.PasokhHa)
                {
                    var found = EditObject.Pasokhs.FirstOrDefault(t => t.Id == item.ID);
                    if (found != null)
                    {
                        if (item.AnswerText != found.AnswerText)
                        {
                            found.AnswerText = item.AnswerText;
                            found.EditDateOnUtc = DateTime.Now;
                        }
                    }
                }
                db.SaveChanges();
                return "OK";
            }
        }


        public SoalatModel SoalatDetail(int SoalatId)
        {
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                var Founded = db.Soalats.FirstOrDefault(u => u.Id == SoalatId && u.Status == true);
                if (Founded == null || Founded.Status == false) { return null; }
                SoalatModel t = new SoalatModel();
                t.Pasokh = Founded.Pasokh ?? default(int);
                t.Question = Founded.Question;
                t.CreateDateOnUtcJalali = Tools.GetDateTimeReturnJalaliDate(Founded.CreateDateOnUtc ?? default(DateTime));
                t.ID = Founded.Id;
                t.AzmoonName = Founded.Azmoon.Name;
                var answerbox = db.Pasokhs.Where(u => u.F_SoalatId == SoalatId);
                List<int> PasokhIds = new List<int>();
                PasokhIds.AddRange(answerbox.Where(e => e.F_SoalatId == SoalatId).Select(o => o.Id));
                foreach (var item in answerbox)
                {
                    PasokhModel model = new PasokhModel();
                    model.AnswerText = item.AnswerText;
                    model.AnswerKey = item.AnswerKey ?? default(int);
                    model.ID = item.Id;
                    model.Score = item.Score ?? default(int);
                    t.PasokhHa.Add(model);
                }
                t.PasokhHa = t.PasokhHa.OrderBy(r => r.AnswerKey).ToList();
                int Total = db.Mapping_Profile_Pasokh.Where(u => PasokhIds.Contains(u.F_PasokhId ?? default(int))).Count();
                if (Total != 0)
                {
                    int CorrectAnswerScore = answerbox.FirstOrDefault(q => q.AnswerKey == (Founded.Pasokh)).Score ?? default(int);
                    double tempscore = (double)CorrectAnswerScore / Total;
                    t.CorrectPercentage = tempscore * 100 + " %";
                }
                else
                    t.CorrectPercentage = "این سوال توسط هیچ کس پاسخ داده نشده";
                return t;
            }
        }


        public int ChangeStatusSoalat(int SoalatId)
        {
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                var ChangeStatusObject = db.Soalats.FirstOrDefault(u => u.Id == SoalatId && u.Status == true);
                if (ChangeStatusObject == null) { return -1; }
                ChangeStatusObject.Status = false;
                //var soalat = ListSoalat(ChangeStatusObject.F_AzmoonId ?? default(int)).Where(u=>u.ID>SoalatId).ToList();
                string F_UserId = Tools.F_UserId();
                int azmoonID = ChangeStatusObject.F_AzmoonId ?? default(int);
                var ListObject = (F_UserId != "71ffec0a-1d92-4bc3-8553-ffbff55b885f") ? db.Soalats.Where(m => m.Status == true && m.Azmoon.F_UserId == F_UserId && m.F_AzmoonId == azmoonID && m.Id > SoalatId).OrderBy(m => m.CreateDateOnUtc).ToList() : db.Soalats.Where(m => m.Status == true && m.F_AzmoonId == azmoonID && m.Id > SoalatId).OrderBy(m => m.CreateDateOnUtc).ToList();
                if (ListObject != null)
                {
                    ListObject.ForEach(u => u.SoalNumber = u.SoalNumber-1);
                }
                db.SaveChanges();
                return ChangeStatusObject.F_AzmoonId ?? default(int);
            }
        }
    }
}