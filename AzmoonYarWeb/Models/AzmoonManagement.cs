using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShahrdari.Areas.Admin3mill.Models;
using System.Data.Entity;

namespace AzmoonYarWeb.Models
{
    public class AzmoonManagement
    {
        public List<AzmoonModel> ListAzmoon(string Type = "Master")
        {
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                string F_UserId = Tools.F_UserId();
                var ListObject = (Type == "Master") ? db.Azmoons.Include(u => u.AspNetUser).Where(q => q.Status == true && q.F_UserId == F_UserId) : db.Azmoons.Include(u => u.AspNetUser).Where(q => q.Status == true);
                List<AzmoonModel> list = new List<AzmoonModel>();
                foreach (var item in ListObject)
                {
                    AzmoonModel ListItem = new AzmoonModel();
                    ListItem.ID = item.Id;
                    ListItem.MasterName = item.AspNetUser.FirstName + " " + item.AspNetUser.LastName;
                    ListItem.Name = item.Name;
                    ListItem.StartDateOnUtcJalali = Tools.GetDateTimeReturnJalaliDate(item.StartDateOnUtc ?? default(DateTime));
                    ListItem.EndDateOnUtcJalali = Tools.GetDateTimeReturnJalaliDate(item.EndDateOnUtc ?? default(DateTime));
                    ListItem.IsActive = false;
                    if ((item.StartDateOnUtc < DateTime.Now || item.StartDateOnUtc == DateTime.Now) && (item.EndDateOnUtc > DateTime.Now || item.EndDateOnUtc == DateTime.Now))
                        ListItem.IsActive = true;
                    list.Add(ListItem);
                }
                return list;
            }
        }

        public void AddAzmoon(AzmoonModel model)
        {
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                Azmoon InsertObject = new Azmoon();
                InsertObject.StartDateOnUtc = model.StartDateOnUtc;
                InsertObject.EndDateOnUtc = model.EndDateOnUtc;
                InsertObject.Name = model.Name;
                InsertObject.CreateDateOnUtc = DateTime.Now;
                InsertObject.Status = true;
                InsertObject.F_UserId = Tools.F_UserId();
                db.Azmoons.Add(InsertObject);
                db.SaveChanges();
            }
        }


        public string EditAzmoon(AzmoonModel model)
        {
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                var EditObject = db.Azmoons.FirstOrDefault(u => u.Id == model.ID && u.Status == true);
                if (EditObject != null)
                {
                    EditObject.StartDateOnUtc = model.StartDateOnUtc;
                    EditObject.EndDateOnUtc = model.EndDateOnUtc;
                    EditObject.Name = model.Name;
                    db.SaveChanges();
                    return "OK";
                }
                else
                    return "NOK";
            }
        }
    }
}