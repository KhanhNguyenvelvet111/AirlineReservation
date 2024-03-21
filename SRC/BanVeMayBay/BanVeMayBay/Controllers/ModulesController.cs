using BanVeMayBay.DesignPattern.TemplateMethod;
using BanVeMayBay.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanVeMayBay.Controllers
{
    public class ModulesController : TemplateMethodController
    {
        // GET: Modules
        BANVEMAYBAYEntities db = new BANVEMAYBAYEntities();

        public ModulesController()
        {
            var result = PrintInfo();
            Debugger.Log(1, "Logger: ", $"{result}");
        }


        public ActionResult _Header()
        {
            if ((string)Session["userName11"] != "")
            {
                ViewBag.sessionFullname = Session["userName11"];
            }
            else
            {

            }
            return View("_Header");
        }
        public ActionResult _Mainmenu()
        {

            var list = db.menus.Where(m => m.status == 1 && m.parentid == 0).ToList();
            return View("_Mainmenu", list);
        }
        public ActionResult _Footer()
        {
            return View("_Footer");
        }

        public ActionResult Slider()
        {
            return View("Slider");
        }
        public ActionResult LogoSlide()
        {
            return View("LogoSlide");
        }

        public override string PrintRoutes()
        {
            return "========================" +
                "Module Controller is running!" +
                "======================";
        }

        public override string PrintDIs()
        {
            return "=================No Dependence Injection================\n";
        }
    }
}