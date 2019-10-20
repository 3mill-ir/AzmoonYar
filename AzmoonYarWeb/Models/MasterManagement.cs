using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShahrdari.Areas.Admin3mill.Models;
using System.Data.Entity;

namespace AzmoonYarWeb.Models
{
    public class MasterManagement
    {
        public List<MasterModel> ListMasters()
        {
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                var ListObject = db.AspNetUsers.Where(q => q.Id != "71ffec0a-1d92-4bc3-8553-ffbff55b885f" && q.Status == true);
                List<MasterModel> list = new List<MasterModel>();
                foreach (var item in ListObject)
                {
                    MasterModel ListItem = new MasterModel();
                    ListItem.ID = item.Id;
                    ListItem.FirstName = item.FirstName;
                    ListItem.LastName = item.LastName;
                    ListItem.Email = item.Email;
                    ListItem.PhoneNumber = item.PhoneNumber;
                    list.Add(ListItem);
                }
                return list;
            }
        }


        public string EditMaster(MasterModel model)
        {
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                var EditObject = db.AspNetUsers.FirstOrDefault(u => u.Id == model.ID && u.Status == true);
                if (EditObject == null)
                    return "NotFound";
                EditObject.FirstName = model.FirstName;
                EditObject.LastName = model.LastName;
                EditObject.PhoneNumber = model.PhoneNumber;
                EditObject.Email = model.Email;
                db.SaveChanges();
                return "OK";
            }
        }

        public string ChangeStatusMaster(string MasterId)
        {
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                var EditObject = db.AspNetUsers.Include(q=>q.Azmoons).FirstOrDefault(u => u.Id == MasterId && u.Status == true);
                if (EditObject == null)
                    return "NotFound";
                EditObject.Status = !EditObject.Status;
                foreach (var item in EditObject.Azmoons)
                {
                    item.Status = !item.Status;
                    var Soalat = db.Soalats.Where(u => u.F_AzmoonId == item.Id);
                    foreach (var item2 in Soalat)
                    {
                        item2.Status = !item2.Status;
                    }
                }
                db.SaveChanges();
                return "OK";
            }
        }

    }
}