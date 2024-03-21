using BanVeMayBay.Common;
using BanVeMayBay.DesignPattern.TemplateMethod;
using BanVeMayBay.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanVeMayBay.Controllers
{
    public class SiteController : TemplateMethodController
    {
        BANVEMAYBAYEntities db = new BANVEMAYBAYEntities();

        public SiteController()
        {
            var result = PrintInfo();
            Debugger.Log(1, "Logger: ", $"{result}");
        }

        public ActionResult Index()
        {
            return View();
        }
         
        [HttpPost]        
        public ActionResult flightSearch(FormCollection fc, int? page)
        {
            
            string typeTicket = fc["typeticket"];
            if (page == null) 
            { 
                page = 1; 
            }
            int pageSize = 4;
          
            int songuoi1 = int.Parse(fc["songuoi1"]);
            int songuoi2 = int.Parse(fc["songuoi2"]);
            int songuoi3 = int.Parse(fc["songuoi3"]);
            int tong = songuoi1 + songuoi2 + songuoi3;
            int songuoi = tong;
            ViewBag.songuoi = songuoi;
            string noiBay = fc["departure_address"];
            string noiVe = fc["arrival_address"];
            string ngaybay = fc["departure_date"];
           
            ViewBag.url = "chuyen-bay";

            //convert sang mm/dd/yy cho may hieu 
            DateTime ngaybay1 = DateTime.ParseExact(ngaybay, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            //sang mm/dd/yy
            string ngaybay2 = ngaybay1.ToString("MM-dd-yyyy");
            DateTime ngaybay3 = DateTime.Parse(ngaybay2);
            ViewBag.noiBay = noiBay;
            ViewBag.noiVe = noiVe;
            ViewBag.ngaybay = ngaybay;
           
            // neu la ve 2 chieu
            if (typeTicket.Equals("enable"))
            {
                string ngayve = fc["arrival_date"];
                DateTime ngayden1 = DateTime.ParseExact(ngayve, "d/M/yyyy", CultureInfo.InvariantCulture);
                string ngayden2 = ngayden1.ToString("MM-dd-yyyy");
                DateTime ngayden3 = DateTime.Parse(ngayden2);
                ViewBag.ngayden = ngayve;
                ViewBag.date = ngayden3;

                if (ngaybay1 > ngayden1)
                {
                    Message.set_flash("Return date must be greater than or equal to departure date!", "danger");
                    return Redirect("~/Home/Index");
                }
                var list = db.tickets.Where(m => m.city.cityName.Contains(noiBay) && m.city1.cityName.Contains(noiVe)).
             Where(m => m.departure_date == ngaybay3).Where(m => m.status == 1).ToList();
                int pageNumber = (page ?? 1);
                return View("flightSearchReturn", list.ToPagedList(pageNumber, pageSize));
                    
                
            }
            else
            {

                //ve 1 chieu
                var list = db.tickets.Where(m => m.city.cityName.Contains(noiBay) && m.city1.cityName.Contains(noiVe)).
             Where(m => m.departure_date == ngaybay3).Where(m=>m.status==1).ToList();
                int pageNumber = (page ?? 1);
                return View("flightSearchOnway", list.ToPagedList(pageNumber, pageSize));
            
            }

        }
        
        public ActionResult return_ticket(DateTime date,string noibay, string noiden)
        {
           
            var list = db.tickets.Where(m => m.city.cityName.Contains(noiden) && m.city1.cityName.Contains(noibay)).
               Where(m => m.departure_date == date).Where(m => m.status == 1).ToList();
            return View("_returnTicket", list);
        }
        public ActionResult AllChuyenBay(int? page)
        {
            if (page == null) page = 1;
            int pageSize = 10;
            var singleC = db.topics.Where(m => m.status == 1).Where(m => m.status == 1).First();
            ViewBag.url = "all-chuyen-bay";
            int pageNumber = (page ?? 1);
            //không biết sử dụng
            //ViewBag.breadcrumb = "Tất cả chuyến bay";//không biết sử dụng
            var list_flight = db.tickets.Where(m => m.status == 1).ToList();
            return View("allflight", list_flight.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult postOftoPic(int? page, string slug)
        {
            if (page == null) page = 1;
            int pageSize = 4;
            var singleC = db.topics.Where(m => m.status == 1 && m.slug == slug).Where(m => m.status == 1).First();
            ViewBag.nameTopic = slug;
            ViewBag.url = "tin-tuc/" + slug + "";
            int pageNumber = (page ?? 1);
            var listPost = db.posts.Where(m => m.status == 1 && m.topid == singleC.ID).OrderByDescending(m => m.ID).ToList();
            return View("postOftoPic", listPost.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult topic()
        {

            var list = db.topics.Where(m => m.status == 1).Where(m => m.status == 1).ToList();
            return View("_topic", list);
        }

        public ActionResult postSearch(string keyw, int? page)
        {
            if (page == null) page = 1;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            ViewBag.url = "tim-kiem-bai-viet?keyw=" + keyw + "";
            @ViewBag.nameTopic = "Tim kiếm từ khóa: " + keyw;
            var list = db.posts.Where(m => m.title.Contains(keyw) || m.detail.Contains(keyw)).Where(m => m.status == 1).OrderBy(m => m.ID);
            return View("postOftoPic", list.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult PostDetal(string slug)
        {

            var single = db.posts.Where(m => m.status == 1 && m.slug == slug).First();
            ViewBag.Bra = single.title;
            return View("PostDetal", single);
        }
        
            public ActionResult flightDetail(int id)
        {
            var single = db.tickets.Where(m => m.status == 1 && m.id == id).First();
            return View("flightDetail", single);
        }
        public ActionResult lienHe()
        {
            var single = db.posts.Where(m => m.status == 1 && m.slug == "contact-us").First();
            return View("PostDetal", single);
        }



        public override string PrintRoutes()
        {
            return "========================" +
                "Site Controller is running!" +
                "======================";
        }

        public override string PrintDIs()
        {
            return "=================No Dependence Injection================\n";
        }
    }
}