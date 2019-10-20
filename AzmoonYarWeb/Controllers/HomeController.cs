using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShahrdari.CustomFilters;

namespace AzmoonYarWeb.Controllers
{
    public class HomeController : Controller
    {
        [AuthLog(Roles = "Admin, Expert")]
        public ActionResult Index()
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Dashboard;
            //************ End Page Tittle *********************************
            return View();
        }
    }
}