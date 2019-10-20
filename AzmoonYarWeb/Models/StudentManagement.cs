using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzmoonYarWeb.Models
{
    public class StudentManagement
    {
        public List<StudentModel> ListStudents()
        {
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                var ListObject = db.Profiles.Where(q => q.Status == true);
                List<StudentModel> list = new List<StudentModel>();
                foreach (var item in ListObject)
                {
                    StudentModel ListItem = new StudentModel();
                    ListItem.ID = item.Id;
                    ListItem.FirstName = item.FirstName;
                    ListItem.LastName = item.LastName;
                    ListItem.SSN = item.CodeMelli;
                    ListItem.Tell = item.Tell;
                    ListItem.CreatedDateOnUtc = item.CreateDateOnUtc ?? default(DateTime);
                    ListItem.EditDateOnUtc = item.EditDateOnUtc ?? default(DateTime);
                    list.Add(ListItem);
                }
                return list;
            }
        }


        public string AddStudent(StudentModel model)
        {
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                var Temp = db.Profiles.FirstOrDefault(u => u.Tell == model.Tell && u.Status == true);
                if (Temp == null)
                {
                    Profile InsertObject = new Profile();
                    InsertObject.CodeMelli = model.SSN;
                    InsertObject.FirstName = model.FirstName;
                    InsertObject.LastName = model.LastName;
                    InsertObject.Tell = model.Tell;
                    InsertObject.Status = true;
                    InsertObject.CreateDateOnUtc = DateTime.Now;
                    db.Profiles.Add(InsertObject);
                    db.SaveChanges();
                    return "OK";
                }
                else
                    return "Iteration";
            }
        }

        public string EditStudent(StudentModel model)
        {
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                var EditObject = db.Profiles.FirstOrDefault(u => u.Id == model.ID);
                if (EditObject == null)
                    return "NotFound";
                EditObject.CodeMelli = model.SSN;
                EditObject.FirstName = model.FirstName;
                EditObject.LastName = model.LastName;
                EditObject.EditDateOnUtc = DateTime.Now;
                db.SaveChanges();
                return "OK";
            }
        }
    }

}