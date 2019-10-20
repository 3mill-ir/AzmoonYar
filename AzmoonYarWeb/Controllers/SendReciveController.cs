using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using AzmoonYarWeb.Models;
using System.Data.Entity;

namespace AzmoonYarWeb.Controllers
{
    public class SendReciveController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public void RecieveSMS(string To,string Message, string From)
        {
            if (!string.IsNullOrEmpty(From))
            {
                if (!From.StartsWith("0"))
                {
                    From = "0"+From;
                }
            }
            if (!string.IsNullOrEmpty(Message))
            {
                using (Entities db = new Entities())
                {
              
                    List<string> Information = new List<string>();
                    Information.AddRange(Message.Split('_'));
                    Regex Registering = new Regex(@"[A-Za-z\u0600-\u06FF]+_[A-Za-z\u0600-\u06FF]+_[0-9]+");
                    Regex Azmoon = new Regex(@"[0-9]+");
                    Regex Pasokh = new Regex(@"[0-9]+_[0-9]+");
                    var User = db.Profiles.FirstOrDefault(r => r.Tell == From);
                    int F_ProfileId;
                    if (User == null)
                    {
                        Profile P = new Profile();
                        P.Tell = From;
                        P.Status = true;
                        P.CreateDateOnUtc = DateTime.Now;
                        db.Profiles.Add(P);
                        db.SaveChanges();
                        F_ProfileId = P.Id;
                    }
                    else
                        F_ProfileId = User.Id;
                    if (Registering.Match(Message).Success)
                    {
                        var FoundedObject = db.Profiles.FirstOrDefault(e => e.Tell == From);
                        FoundedObject.FirstName = Information[0];
                        FoundedObject.LastName = Information[1];
                        FoundedObject.CodeMelli = Information[2];
                        FoundedObject.EditDateOnUtc = DateTime.Now;
                        db.SaveChanges();
                    }
                    else if (Pasokh.Match(Message).Success)
                    {
                        var Temp = db.Mapping_Profile_Azmoon.Include(e => e.Azmoon).OrderByDescending(q => q.CreateDateOnUtc).FirstOrDefault(t => t.F_ProfileId == F_ProfileId);
                        if (Temp != null)
                        {
                            if (Temp.CreateDateOnUtc > Temp.Azmoon.CreateDateOnUtc && Temp.CreateDateOnUtc < Temp.Azmoon.EndDateOnUtc)
                            {
                                int SoalId = int.Parse(Information[0]);
                                int Key = int.Parse(Information[1]);
                                var Answer = db.Pasokhs.FirstOrDefault(u => u.F_SoalatId == SoalId && u.AnswerKey == Key);
                                //var UsedBefore = db.Mapping_Profile_Pasokh.FirstOrDefault(u => u.F_PasokhId == Answer.Id && u.F_ProfileId == F_ProfileId);
                                ////var mypasokh = db.Soalats.FirstOrDefault(u => u.Id == SoalId).Pasokhs.FirstOrDefault(u=>u.AnswerKey==Key).Mapping_Profile_Pasokh.FirstOrDefault(u=>u.F_ProfileId==F_ProfileId);
                                var myPasokh = db.Mapping_Profile_Pasokh.FirstOrDefault(u => u.F_ProfileId == F_ProfileId && u.SoalId == SoalId);
                                if (myPasokh == null)
                                {
                                    Mapping_Profile_Pasokh MPP = new Mapping_Profile_Pasokh();
                                    MPP.F_ProfileId = F_ProfileId;
                                    MPP.F_PasokhId = Answer.Id;
                                    MPP.CreateDateOnUtc = DateTime.Now;
                                    db.Mapping_Profile_Pasokh.Add(MPP);
                                    Answer.Score++;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                    else if (Azmoon.Match(Message).Success)
                    {
                        int AzmoonId = int.Parse(Information[0]);
                        var Temp = db.Azmoons.FirstOrDefault(u => u.Id == AzmoonId);
                        if (Temp != null)
                        {
                            Mapping_Profile_Azmoon MFP = new Mapping_Profile_Azmoon();
                            MFP.F_AzmoonId = Temp.Id;
                            MFP.F_ProfileId = F_ProfileId;
                            MFP.CreateDateOnUtc = DateTime.Now;
                            db.Mapping_Profile_Azmoon.Add(MFP);
                            db.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}