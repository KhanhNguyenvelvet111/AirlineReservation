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
    public class HomeController : TemplateMethodController
    {
        BANVEMAYBAYEntities db = new BANVEMAYBAYEntities();

        public HomeController()
        {
            var result = PrintInfo();
            Debugger.Log(1, "Logger: ", $"{result}");
        }


        // GET: Home
        public ActionResult Index()
        {
            DateTime date_now = DateTime.Now;
            string date_now1 = date_now.ToString("MM-dd-yyyy");
            DateTime date_now2 = DateTime.Parse(date_now1);
            ViewBag.dateNow = date_now;
            // lay cac chuyen bay trong ngay
            //var list = db.Take(20).ToList();
            var cities = db.cities.ToList();
            ViewBag.cities = cities;
            var tickets = db.tickets.Where(m => m.status == 1 && m.departure_date == date_now2).ToList();
            ViewBag.tickets = tickets;
            return View();



        }
    
        public ActionResult flightOfday()
        {
            DateTime date_now = DateTime.Now;
            string date_now1 = date_now.ToString("MM-dd-yyyy");
            DateTime date_now2 = DateTime.Parse(date_now1);
            ViewBag.dateNow = date_now;

            // lay cac chuyen bay trong ngay
            var list = db.tickets.Where(m => m.status == 1 && m.departure_date == date_now2).Take(20).ToList();
            return View("flightOfDay", list);
        }

        public override string PrintRoutes()
        {
            return "========================" +
                "Home Controller is running!" +
                "======================";
        }

        public override string PrintDIs()
        {
            return "=================No dependence Injection================\n";
        }

    }
}