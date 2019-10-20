using AzmoonYarWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShahrdari.Areas.Admin3mill.Models;
using PagedList;
using WebShahrdari.CustomFilters;

namespace AzmoonYarWeb.Controllers
{
    [AuthLog(Roles = "Admin, Expert")]
    public class MastersController : Controller
    {
        public ActionResult ListMasters(int? page)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_ListMasters;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_ListMasters;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_AddMaster, "Register", "Account", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *********************************
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.PaginationCount = pageNumber;
            MasterManagement post = new MasterManagement();
            return View(post.ListMasters().ToPagedList(pageNumber, pageSize));
        }


        public ActionResult EditMaster(string MasterId, int Page)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_EditMaster;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_EditMaster;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_Tittle_ListMasters, "ListMasters", "Masters", "btn-green ti-list", null));
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_AddMaster, "Register", "Account", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                var temp = db.AspNetUsers.FirstOrDefault(u => u.Id == MasterId);
                if (temp == null) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                MasterModel model = new MasterModel();
                model.ID = temp.Id;
                model.FirstName = temp.FirstName;
                model.LastName = temp.LastName;
                model.Email = temp.Email;
                model.PhoneNumber = temp.PhoneNumber.Substring(1);
                TempData["Page"] = Page;
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMaster(MasterModel model)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_EditMaster;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_EditMaster;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_Tittle_ListMasters, "ListMasters", "Masters", "btn-green ti-list", null));
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_AddMaster, "Register", "Account", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            if (ModelState.IsValid)
            {
                MasterManagement post = new MasterManagement();
                model.PhoneNumber = '0' + model.PhoneNumber.Replace(")", "").Replace("(", "").Replace("-", "").Replace(" ", "");
                if (post.EditMaster(model) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                return RedirectToAction("ListMasters", "Masters", new { page = TempData["Page"] });
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMaster(string MasterId, int Page)
        {
            MasterManagement MM = new MasterManagement();
            string Scale = MM.ChangeStatusMaster(MasterId);
            if (Scale == "OK")
                return RedirectToAction("ListMasters", "Masters", new { page = Page });
            else
                return View("~/Views/Shared/NotFoundFailed.cshtml");
        }
    }
}