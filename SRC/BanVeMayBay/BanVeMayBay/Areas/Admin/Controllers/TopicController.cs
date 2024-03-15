using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanVeMayBay.Common;
using BanVeMayBay.Models;

namespace BanVeMayBay.Areas.Admin.Controllers
{
    public class TopicController : BaseController
    {
        private BANVEMAYBAYEntities db = new BANVEMAYBAYEntities();

        // GET: Admin/Topic
        public ActionResult Index()
        {
            
            var list = db.topics.Where(m => m.status !=0).OrderByDescending(m => m.ID).ToList();
            return View(list);
        }

        // GET: Admin/Topic/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            topic mtopic = db.topics.Find(id);
            if (mtopic == null)
            {
                return HttpNotFound();
            }
            return View(mtopic);
        }

        // GET: Admin/Topic/Create
        public ActionResult Create()
        {
            ViewBag.listtopic = db.topics.Where(m => m.status != 0).ToList();
            return View();
        }

        // POST: Admin/Topic/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(topic mtopic)
        {
            if (ModelState.IsValid)
            {
                //category
                string slug = Mystring.ToSlug(mtopic.name.ToString());
                mtopic.slug = slug;
                mtopic.created_at = DateTime.Now;
                mtopic.updated_at = DateTime.Now;
                mtopic.created_by = int.Parse(Session["Admin_id"].ToString());
                mtopic.updated_by = int.Parse(Session["Admin_id"].ToString());
                db.topics.Add(mtopic);
                db.SaveChanges();
                Message.set_flash("More success", "success");
                return RedirectToAction("Index");
            }
            Message.set_flash("More failure", "danger");
            ViewBag.listtopic = db.topics.Where(m => m.status != 0).ToList();
            return View(mtopic);
        }

        // GET: Admin/Topic/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            topic mtopic = db.topics.Find(id);
            if (mtopic == null)
            {
                return HttpNotFound();
            }
            ViewBag.listtopic = db.topics.Where(m => m.status != 0).ToList();
            return View(mtopic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( topic mtopic)
        {
            if (ModelState.IsValid)
            {
                string slug = Mystring.ToSlug(mtopic.name.ToString());


                mtopic.updated_at = DateTime.Now;
                mtopic.updated_by = int.Parse(Session["Admin_id"].ToString());
                db.Entry(mtopic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.listtopic = db.topics.Where(m => m.status != 0).ToList();
            return View(mtopic);
        }

        public ActionResult Status(int id)
        {
            topic mtopic = db.topics.Find(id);
            mtopic.status = (mtopic.status == 1) ? 2 : 1;
            mtopic.updated_at = DateTime.Now;
            mtopic.updated_by = int.Parse(Session["Admin_id"].ToString());
            db.Entry(mtopic).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Status change successful", "success");
            return RedirectToAction("Index");
        }
        //trash
        public ActionResult trash()
        {
            var list = db.topics.Where(m => m.status == 0).ToList();
            return View("Trash", list);
        }
        public ActionResult Deltrash(int id)
        {
            topic mtopic = db.topics.Find(id);
            mtopic.status = 0;
            mtopic.updated_at = DateTime.Now;
            mtopic.updated_by = int.Parse(Session["Admin_id"].ToString());
            db.Entry(mtopic).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Delete successfully", "success");
            return RedirectToAction("Index");
        }

        public ActionResult Retrash(int id)
        {
            topic mtopic = db.topics.Find(id);
            mtopic.status = 2;
            mtopic.updated_at = DateTime.Now;
            mtopic.updated_by = int.Parse(Session["Admin_id"].ToString());
            db.Entry(mtopic).State = EntityState.Modified;
            db.SaveChanges();
            Message.set_flash("Restore Success", "success");
            return RedirectToAction("trash");
        }
        public ActionResult deleteTrash(int id)
        {
            topic mtopic = db.topics.Find(id);
            db.topics.Remove(mtopic);
            db.SaveChanges();
            Message.set_flash("1 Thread has been permanently deleted", "success");
            return RedirectToAction("trash");
        }
    }
}
