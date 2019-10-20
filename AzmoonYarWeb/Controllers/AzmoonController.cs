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
    public class AzmoonController : Controller
    {
        [AuthLog(Roles = "Expert")]
        public ActionResult ListMasterAzmoon(int? page)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_ListAzmoon;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_ListAzmoon;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_AddAzmoon, "AddAzmoon", "Azmoon", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *********************************
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.PaginationCount = pageNumber;
            AzmoonManagement AM = new AzmoonManagement();
            return View(AM.ListAzmoon().ToPagedList(pageNumber, pageSize));
        }
        [AuthLog(Roles = "Admin")]
        public ActionResult ListAllAzmoon(int? page)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_ListAzmoon;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_ListAzmoon;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_Dashboard, "Index", "Home", "btn-purple ti-view-list-alt", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *********************************
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.PaginationCount = pageNumber;
            AzmoonManagement AM = new AzmoonManagement();
            return View(AM.ListAzmoon("Admin").ToPagedList(pageNumber, pageSize));
        }
        [AuthLog(Roles = "Expert")]
        public ActionResult AddAzmoon()
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_AddAzmoon;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_AddAzmoon;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_Tittle_ListAzmoon, "ListMasterAzmoon", "Azmoon", "btn-green ti-list", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            return View();
        }
        [HttpPost]
        [AuthLog(Roles = "Expert")]
        [ValidateAntiForgeryToken]
        public ActionResult AddAzmoon(AzmoonModel model)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_AddAzmoon;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_AddAzmoon;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_Tittle_ListAzmoon, "ListMasterAzmoon", "Azmoon", "btn-green ti-list", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            if (!Tools.GetJalaliDateReturnDateTime(model.StartDateOnUtcJalali, out start))
            {
                ModelState.AddModelError("StartDateOnUTC", Resource.Resource.View_DateFormatError);
            }
            if (!Tools.GetJalaliDateReturnDateTime(model.EndDateOnUtcJalali, out end))
            {
                ModelState.AddModelError("EndDateOnUTC", Resource.Resource.View_DateFormatError);
            }
            if (ModelState.IsValid)
            {
                AzmoonManagement AM = new AzmoonManagement();
                model.StartDateOnUtc = start;
                model.EndDateOnUtc = end;
                AM.AddAzmoon(model);
                return RedirectToAction("ListMasterAzmoon", "Azmoon");
            }
            else
            {
                return View(model);
            }
        }

        [AuthLog(Roles = "Expert")]
        public ActionResult EditAzmoon(int AzmoonId, int Page)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_EditAzmoon;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_EditAzmoon;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_Tittle_ListAzmoon, "ListMasterAzmoon", "Azmoon", "btn-green ti-list", null));
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_AddAzmoon, "AddAzmoon", "Azmoon", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                var temp = db.Azmoons.FirstOrDefault(u => u.Id == AzmoonId);
                if (temp == null) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                AzmoonModel model = new AzmoonModel();
                model.ID = temp.Id;
                model.Name = temp.Name;
                model.StartDateOnUtcJalali = Tools.GetDateTimeReturnJalaliDate(temp.StartDateOnUtc ?? default(DateTime));
                model.EndDateOnUtcJalali = Tools.GetDateTimeReturnJalaliDate(temp.EndDateOnUtc ?? default(DateTime));
                TempData["Page"] = Page;
                return View(model);
            }
        }
        [HttpPost]
        [AuthLog(Roles = "Expert")]
        [ValidateAntiForgeryToken]
        public ActionResult EditAzmoon(AzmoonModel model)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_EditAzmoon;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_EditAzmoon;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_Tittle_ListAzmoon, "ListMasterAzmoon", "Azmoon", "btn-green ti-list", null));
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_AddAzmoon, "AddAzmoon", "Azmoon", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            if (!Tools.GetJalaliDateReturnDateTime(model.StartDateOnUtcJalali, out start))
            {
                ModelState.AddModelError("StartDateOnUTC", Resource.Resource.View_DateFormatError);
            }
            if (!Tools.GetJalaliDateReturnDateTime(model.EndDateOnUtcJalali, out end))
            {
                ModelState.AddModelError("EndDateOnUTC", Resource.Resource.View_DateFormatError);
            }
            model.StartDateOnUtc = start;
            model.EndDateOnUtc = end;
            if (ModelState.IsValid)
            {
                AzmoonManagement SM = new AzmoonManagement();
                if (SM.EditAzmoon(model) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                return RedirectToAction("ListMasterAzmoon", "Azmoon", new { page = TempData["Page"] });
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [AuthLog(Roles = "Expert")]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeStatusAzmoon(int AzmoonId, int Page)
        {
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                var temp = db.Azmoons.FirstOrDefault(u => u.Id == AzmoonId);
                if (temp == null) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                temp.Status = !temp.Status;
                db.SaveChanges();
                return RedirectToAction("ListMasterAzmoon", "Azmoon", new { page = Page });
            }
        }
    }
}