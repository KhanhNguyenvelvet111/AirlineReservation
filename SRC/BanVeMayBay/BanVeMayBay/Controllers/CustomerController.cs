using BanVeMayBay.Common;
using BanVeMayBay.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace BanVeMayBay.Controllers
{
    public class CustomerController : Controller
    {
        private BANVEMAYBAYEntities db = new BANVEMAYBAYEntities();
        // GET: Customer
        public ActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        public ActionResult login(FormCollection fc)
        {
            String Username = fc["username"];
            string Pass = Mystring.ToMD5(fc["password"]);
            var user_account = db.users.Where(m => m.access == 1 && m.status == 1 && (m.username == Username));
            var pass = user_account.FirstOrDefault()?.password;
                if (user_account.Count() == 0)
                {
                    ViewBag.error = "Username Incorrect";
                }
                else
                {
                    var pass_account = user_account.Where(m => m.access == 1 && m.status == 1 && m.password == Pass).FirstOrDefault();
                    if (pass_account == null)
                    {
                        ViewBag.error = "Incorrect password";
                    }
                    else
                    {
                    var user = user_account.First();
                    Session.Add(CommonConstants.CUSTOMER_SESSION, user);
                    Session["userName11"] = user.fullname;
                    Session["id"] = user.ID;
                    if (!Response.IsRequestBeingRedirected)
                        Message.set_flash("Logged in successfully", "success");
                    return Redirect("~/tai-khoan");
                }
                }
            
            ViewBag.sess = Session["Admin_id"];
            return View("Login");

        }
        public void logout()
        {
            Session["userName11"] = "";
            Session[Common.CommonConstants.CUSTOMER_SESSION]="";
            Response.Redirect("~/dang-nhap");
            Message.set_flash("Sign out successful", "success");
        }
        public ActionResult register()
        {
            return View("register");
        }
        [HttpPost]
        public ActionResult register(user muser, FormCollection fc)
        {
            string uname = fc["uname"];
            string fname = fc["fname"];
            string Pass = Mystring.ToMD5(fc["psw"]);
            string Pass2 = Mystring.ToMD5(fc["repsw"]);
            if (Pass2 != Pass)
            {
                ViewBag.error = "password incorrect";
                return View("loginEndRegister");
            }
            string email = fc["email"];
            string address = fc["address"];
            string phone = fc["phone"];
            if (ModelState.IsValid)
            {
                var Luser = db.users.Where(m => m.status == 1 && m.username == uname && m.access == 1);
                if (Luser.Count() > 0)
                {
                    ViewBag.error = "Username available";
                    return View("loginEndRegister");
                }
                else
                {
                    muser.img = "defalt.png";
                    muser.password = Pass;
                    muser.username = uname;
                    muser.fullname = fname;
                    muser.email = email;
                    muser.address = address;
                    muser.phone = phone;
                    muser.gender = "nam";
                    muser.created_at = DateTime.Now;
                    muser.updated_at = DateTime.Now;
                    muser.created_by = 1;
                    muser.updated_by = 1;
                    muser.access = 1;
                    muser.status = 1;
                    db.users.Add(muser);
                    db.SaveChanges();
                    Message.set_flash("Successful account registration, Login here ", "success");
                    return Redirect("~/dang-nhap");
                }

            }
            Message.set_flash("Account registration failed", "danger");
            return View("register");
        }

        public  ActionResult Myaccount()
        {
            user sessionUser = (user)Session[Common.CommonConstants.CUSTOMER_SESSION];
            return View("Myaccount", sessionUser);
        }
        [HttpPost]
        public ActionResult Myaccount(user user,FormCollection fc)
        {
            var pswO = fc["pswO"];
            var pswN = fc["pswN"];
            var pswR = fc["pswR"];
            if(pswO != null)
            {
                if (pswO.ToMD5() != user.password)
                {
                    ViewBag.success = "Old password is incorrect.";
                    return View("Myaccount", user);
                }
                if (pswN == null || pswR == null || pswN.Length < 6 || pswR.Length < 6)
                {
                    ViewBag.success = "The new password is not valid.";
                    return View("Myaccount", user);
                }    
                if (pswN.ToMD5() != pswR.ToMD5())
                {
                    ViewBag.success = "Password incorrect.";
                    return View("Myaccount", user);
                }
                else
                {
                    user.password = pswN.ToMD5();
                }
            }    

            Session[Common.CommonConstants.CUSTOMER_SESSION] = "";
            Session.Add(CommonConstants.CUSTOMER_SESSION, user);
            user.created_at = DateTime.Now;
            user.updated_at = DateTime.Now;
            user.created_by = 1;
            user.updated_by = 1;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            ViewBag.success = "Update successful.";
            return View("Myaccount", user);
        }
        public ActionResult ListOderCus()
        {
         
            user sessionUser = (user)Session[Common.CommonConstants.CUSTOMER_SESSION];
            var listOrder = db.orders.Where(m=>m.CusId == sessionUser.ID).OrderByDescending(m=>m.ID).ToList();
            return View("ListOderCus", listOrder);
        }
        public ActionResult orderDetailCus(int id)
        {
            var sigleOrder = db.orders.Find(id);
            return View("orderDetailCus", sigleOrder);
        }
        public ActionResult canelOrder(int OrderId)
        {
           

            order morder = db.orders.Find(OrderId);
            var orderDetail = db.ordersdetails.Where(m => m.orderid == morder.ID).ToList();
            foreach (var item in orderDetail)
            {
                var id = int.Parse(item.ticketId.ToString());
                ticket ticket = db.tickets.Find(id);
                DateTime ngaymuon = Convert.ToDateTime(
                    morder.created_ate);
                DateTime ngaytra = Convert.ToDateTime(ticket.departure_date);
                TimeSpan Time = ngaytra - ngaymuon;
                int TongSoNgay = Time.Days;
               if(TongSoNgay >= 14)
                {
                    ticket.Sold = ticket.Sold - item.quantity;
                    db.Entry(ticket).State = EntityState.Modified;
                    db.SaveChanges();
                    if (item == null)
                    {
                        Message.set_flash("Error Cancel Order", "danger");
                        return Redirect("~/tai-khoan");
                    }
                    db.ordersdetails.Remove(item);
                    db.SaveChanges();
                }
                else
                {
                    Message.set_flash("Tickets cannot be canceled 14 days before flight date", "dangger");
                    return Redirect("~/tai-khoan");
                }
                
               
            }
                  
            db.orders.Remove(morder);
            db.SaveChanges();
            Message.set_flash("Canceled 1 order", "success");
            return Redirect("~/tai-khoan");
        }
    }
}