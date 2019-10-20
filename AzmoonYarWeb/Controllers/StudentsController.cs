using AzmoonYarWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShahrdari.Areas.Admin3mill.Models;
using PagedList;
using LinqToExcel;
using System.IO;
using WebShahrdari.CustomFilters;
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace AzmoonYarWeb.Controllers
{
    [AuthLog(Roles = "Admin, Expert")]
    public class StudentsController : Controller
    {
        public ActionResult ListStudents(int? page)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_ListStudents;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_ListStudents;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_AddStudents, "AddStudent", "Students", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *********************************
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.PaginationCount = pageNumber;
            StudentManagement SM = new StudentManagement();
            return View(SM.ListStudents().ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddStudent()
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_AddStudents;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_AddStudent;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_Tittle_ListStudents, "ListStudents", "Students", "btn-green ti-list", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            if (TempData["Alert"] != null)
                ViewBag.Alert = TempData["Alert"].ToString();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStudent(StudentModel model)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_AddStudents;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_AddStudent;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_Tittle_ListStudents, "ListStudents", "Students", "btn-green ti-list", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            if (ModelState.IsValid)
            {
                model.Tell = '0' + model.Tell.Replace(")", "").Replace("(", "").Replace("-", "").Replace(" ", "");
                StudentManagement SM = new StudentManagement();
                if (SM.AddStudent(model) == "Iteration") { ViewBag.Error = "دانش آموز با شماره تماس مورد نظر در سیستم موجود است"; return View(model); }
                return RedirectToAction("ListStudents", "Students");
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStudentFromExcel(HttpPostedFileBase ExcelFile)
        {
            if (ExcelFile == null)
            {
                TempData["Alert"] = "هیچ فایلی انتخاب نشده است";
                return RedirectToAction("AddStudent", "Students");
            }
            var allowedExtensions = new[] { ".xlsx", ".xls" };
            var extension = Path.GetExtension(ExcelFile.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                TempData["Alert"] = "قالب فایل انتخابی صحیح نمی باشد";
                return RedirectToAction("AddStudent", "Students");
            }
            string fileName = ExcelFile.FileName;
            string fileContentType = ExcelFile.ContentType;
            byte[] fileBytes = new byte[ExcelFile.ContentLength];
            var data = ExcelFile.InputStream.Read(fileBytes, 0, Convert.ToInt32(ExcelFile.ContentLength));
            var usersList = new List<Profile>();
            using (var package = new ExcelPackage(ExcelFile.InputStream))
            {
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                using (AzmoonYarWeb.Models.Entities db = new Entities())
                {
                    var ListTels = db.Profiles.Where(u => u.Status == true).Select(q => q.Tell);
                    for (int rowIterator = 1; rowIterator <= noOfRow; rowIterator++)
                    {
                        var t = workSheet.Cells[rowIterator, 4];
                        string Temp = '0' + t.Value.ToString();
                        if (!ListTels.Contains(Temp))
                        {
                            if (t.Value != null)
                            {
                                var prof = new Profile();
                                prof.FirstName = workSheet.Cells[rowIterator, 1].Value.ToString();
                                prof.LastName = workSheet.Cells[rowIterator, 2].Value.ToString();
                                prof.CodeMelli = workSheet.Cells[rowIterator, 3].Value.ToString();
                                prof.Tell = t.Value.ToString();
                                if (prof.Tell[0] != '0')
                                    prof.Tell = '0' + prof.Tell;
                                prof.Status = true;
                                prof.CreateDateOnUtc = DateTime.Now;
                                db.Profiles.Add(prof);
                            }
                        }
                    }
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ListStudents", "Students");
        }



        public ActionResult EditStudent(int StudentId, int Page)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_EditStudent;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_EditStudent;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_Tittle_ListStudents, "ListStudents", "Students", "btn-green ti-list", null));
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_AddStudents, "AddStudent", "Students", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                var temp = db.Profiles.FirstOrDefault(u => u.Id == StudentId);
                if (temp == null) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                StudentModel model = new StudentModel();
                model.ID = temp.Id;
                model.FirstName = temp.FirstName;
                model.LastName = temp.LastName;
                model.SSN = temp.CodeMelli;
                model.Tell = temp.Tell;
                TempData["Page"] = Page;
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudent(StudentModel model)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_EditStudent;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_EditStudent;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_Tittle_ListStudents, "ListStudents", "Students", "btn-green ti-list", null));
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_AddStudents, "AddStudent", "Students", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            if (ModelState.IsValid)
            {
                StudentManagement SM = new StudentManagement();
                if (SM.EditStudent(model) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                return RedirectToAction("ListStudents", "Students", new { page = TempData["Page"] });
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeStatusStudent(int StudentId, int Page)
        {
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                var temp = db.Profiles.FirstOrDefault(u => u.Id == StudentId);
                if (temp == null) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                temp.Status = !temp.Status;
                db.SaveChanges();
                return RedirectToAction("ListStudents", "Students", new { page = Page });
            }

        }
    }
}