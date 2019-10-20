using AzmoonYarWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShahrdari.Areas.Admin3mill.Models;
using PagedList;
using System.Data.Entity;
using WebShahrdari.CustomFilters;

namespace AzmoonYarWeb.Controllers
{
    public class SoalatController : Controller
    {
        [AuthLog(Roles = "Admin, Expert")]
        public ActionResult ListSoalat(int? page, int AzmoonId)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_ListSoalat;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_ListSoalat;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            if (User.IsInRole("Expert"))
            {
                PathLog.Add(new PageTitle(Resource.Resource.PageTittle_AddSoalat, "AddSoalat", "Soalat", "btn-green ti-plus", "?AzmoonId=" + AzmoonId));
            }
            else
                PathLog.Add(new PageTitle(Resource.Resource.PageTittle_Dashboard, "Index", "Home", "btn-purple ti-view-list-alt", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *********************************
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.PaginationCount = pageNumber;
            SoalatManagement SM = new SoalatManagement();
            return View(SM.ListSoalat(AzmoonId).ToPagedList(pageNumber, pageSize));
        }
        [AuthLog(Roles = "Expert")]
        public ActionResult AddSoalat(int AzmoonId)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_AddSoalat;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_Tittle_ListSoalat, "ListSoalat", "Soalat", "btn-purple ti-view-list-alt", "?AzmoonId=" + AzmoonId));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            SoalatModel model = new SoalatModel();
            model.PasokhHa.Add(new PasokhModel() { AnswerKey = 1 });
            model.PasokhHa.Add(new PasokhModel() { AnswerKey = 2 });
            model.PasokhHa.Add(new PasokhModel() { AnswerKey = 3 });
            model.PasokhHa.Add(new PasokhModel() { AnswerKey = 4 });
            model.F_AzmoonId = AzmoonId;
            return View(model);
        }
        [HttpPost]
        [AuthLog(Roles = "Expert")]
        [ValidateAntiForgeryToken]
        public ActionResult AddSoalat(SoalatModel model)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_AddSoalat;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_Tittle_ListSoalat, "ListSoalat", "Soalat", "btn-purple ti-view-list-alt", "?AzmoonId=" + model.F_AzmoonId));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            if (ModelState.IsValid)
            {
                if (model.Pasokh == 0)
                {
                    ViewBag.Error = "گزینه صحیح مورد نظر را انتخاب کنید";
                    return View(model);
                }
                SoalatManagement SM = new SoalatManagement();
                int scale1 = SM.AddSoalat(model);
                if (scale1 == 1)
                    return RedirectToAction("ListSoalat", "Soalat", new { AzmoonId = model.F_AzmoonId });
                else
                {
                    return View(model);
                }
            }
            else
            {
                if (model.Pasokh == 0)
                    ViewBag.Error = "گزینه صحیح مورد نظر را انتخاب کنید";
                return View(model);
            }
        }
        [AuthLog(Roles = "Expert")]
        public ActionResult EditSoalat(int SoalatId, int AzmoonId, int? Page)
        {

            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_EditSoalat;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_Tittle_ListSoalat, "ListSoalat", "Soalat", "btn-purple ti-view-list-alt", "?AzmoonId=" + AzmoonId));
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_AddSoalat, "AddSoalat", "Soalat", "btn-green ti-plus", "?AzmoonId=" + AzmoonId));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_DetailSoalat, "DetailSoalat", "Soalat", "btn-purple ti-bar-chart", "?SoalatId=" + SoalatId + "&&AzmoonId=" + AzmoonId));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *********************************
            TempData["Page"] = Page;
            using (AzmoonYarWeb.Models.Entities db = new Entities())
            {
                SoalatModel model = new SoalatModel();
                var temp = db.Soalats.Include(z => z.Pasokhs).FirstOrDefault(u => u.Id == SoalatId && u.Status == true);
                if (temp == null) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                model.ID = temp.Id;
                model.Pasokh = temp.Pasokh ?? default(int);
                model.Question = temp.Question;
                model.F_AzmoonId = temp.F_AzmoonId ?? default(int);
                foreach (var item in temp.Pasokhs)
                {
                    PasokhModel ans = new PasokhModel();
                    ans.AnswerText = item.AnswerText;
                    ans.ID = item.Id;
                    ans.AnswerKey = item.AnswerKey ?? default(int);
                    ans.F_SoalatId = item.F_SoalatId ?? default(int);
                    model.PasokhHa.Add(ans);
                }
                return View(model);
            }

        }
        [HttpPost]
        [AuthLog(Roles = "Expert")]
        [ValidateAntiForgeryToken]
        public ActionResult EditSoalat(SoalatModel model)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_EditSoalat;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_Tittle_ListSoalat, "ListSoalat", "Soalat", "btn-purple ti-view-list-alt", "?AzmoonId=" + model.F_AzmoonId));
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_AddSoalat, "AddSoalat", "Soalat", "btn-green ti-plus", "?AzmoonId=" + model.F_AzmoonId));
            PathLog.Add(new PageTitle(Resource.Resource.PageTittle_DetailSoalat, "DetailSoalat", "Soalat", "btn-purple ti-bar-chart", "?SoalatId=" + model.ID + "&&AzmoonId=" + model.F_AzmoonId));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *********************************
            if (model.Pasokh == 0)
            {
                ViewBag.Error = "گزینه صحیح مورد نظر را انتخاب کنید";
            }
            if (ModelState.IsValid)
            {
                SoalatManagement SM = new SoalatManagement();
                string result = SM.EditSoalat(model);
                if (result == "NotFound")
                {
                    return View("~/Views/Shared/NotFoundFailed.cshtml");
                }
                else
                {
                    return RedirectToAction("ListSoalat", "Soalat", new { page = TempData["Page"], AzmoonId = model.F_AzmoonId });
                }
            }
            return View(model);
        }


        [HttpPost]
        [AuthLog(Roles = "Expert")]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeStatusSoalat(int SoalatId, int Page)
        {
            SoalatManagement SM = new SoalatManagement();
            int Scale = SM.ChangeStatusSoalat(SoalatId);
            if (Scale == -1) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
            return RedirectToAction("ListSoalat", "Soalat", new { page = Page, AzmoonId = Scale });
        }

        [AuthLog(Roles = "Admin, Expert")]
        public ActionResult DetailSoalat(int SoalatId, int AzmoonId, int? Page)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_DetailSoalat;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_Tittle_ListSoalat, "ListSoalat", "Soalat", "btn-purple ti-view-list-alt", "?AzmoonId=" + AzmoonId + "&&Page=" + Page));
            if (User.IsInRole("Expert"))
            {
                PathLog.Add(new PageTitle(Resource.Resource.PageTittle_AddSoalat, "AddSoalat", "Soalat", "btn-green ti-plus", "?AzmoonId=" + AzmoonId));
                PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_EditSoalat, "EditSoalat", "Soalat", "btn-purple ti-pencil", "?SoalatId=" + SoalatId + "&&AzmoonId=" + AzmoonId));
            }
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *********************************
            SoalatManagement post = new SoalatManagement();
            SoalatModel model = new SoalatModel();
            model = post.SoalatDetail(SoalatId);
            if (model == null) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
            return View(model);
        }

    }
}