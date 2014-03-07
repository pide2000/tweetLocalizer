using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TwitterWebFrontend.Models;

namespace TwitterWebFrontend.Controllers
{
    public class tweetRandomSample2014Controller : Controller
    {
        private GeonamesDataEntities1 db = new GeonamesDataEntities1();

        // GET: /tweetRandomSample2014/
        public ActionResult Index(Int64 searchId)
        {
            var data = from t in db.tweetRandomSample2 select t;

            data = data.Where(s => s.tweetid.Equals(searchId));
            
            return View(data);
        }

       

        // GET: /tweetRandomSample2014/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tweetRandomSample2 tweetrandomsample2 = db.tweetRandomSample2.Find(id);
            if (tweetrandomsample2 == null)
            {
                return HttpNotFound();
            }
            return View(tweetrandomsample2);
        }

        // GET: /tweetRandomSample2014/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /tweetRandomSample2014/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,tweetid,username,long,lat,hashtag,time,status,tweetTime,skipped,place,geotagged,userlocation,lang,utc_offset,timezone")] tweetRandomSample2 tweetrandomsample2)
        {
            if (ModelState.IsValid)
            {
                db.tweetRandomSample2.Add(tweetrandomsample2);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tweetrandomsample2);
        }

        // GET: /tweetRandomSample2014/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tweetRandomSample2 tweetrandomsample2 = db.tweetRandomSample2.Find(id);
            if (tweetrandomsample2 == null)
            {
                return HttpNotFound();
            }
            return View(tweetrandomsample2);
        }

        // POST: /tweetRandomSample2014/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,tweetid,username,long,lat,hashtag,time,status,tweetTime,skipped,place,geotagged,userlocation,lang,utc_offset,timezone")] tweetRandomSample2 tweetrandomsample2)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tweetrandomsample2).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tweetrandomsample2);
        }

        // GET: /tweetRandomSample2014/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tweetRandomSample2 tweetrandomsample2 = db.tweetRandomSample2.Find(id);
            if (tweetrandomsample2 == null)
            {
                return HttpNotFound();
            }
            return View(tweetrandomsample2);
        }

        // POST: /tweetRandomSample2014/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tweetRandomSample2 tweetrandomsample2 = db.tweetRandomSample2.Find(id);
            db.tweetRandomSample2.Remove(tweetrandomsample2);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
