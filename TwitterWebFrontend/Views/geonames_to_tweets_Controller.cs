using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TwitterWebFrontend.Models;

namespace TwitterWebFrontend.Views
{
    public class geonames_to_tweets_Controller : Controller
    {
        private GeonamesDataEntities1 db = new GeonamesDataEntities1();

        // GET: /geonames_to_tweets_/
        public ActionResult Index()
        {
            var manualassign_tweetrandomsample2_to_geonamesrestricted = db.manualAssign_tweetRandomSample2_to_geonamesRestricted.Include(m => m.GeoNamesRestricted).Include(m => m.tweetRandomSample2);
            return View(manualassign_tweetrandomsample2_to_geonamesrestricted.ToList());
        }

        // GET: /geonames_to_tweets_/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            manualAssign_tweetRandomSample2_to_geonamesRestricted manualassign_tweetrandomsample2_to_geonamesrestricted = db.manualAssign_tweetRandomSample2_to_geonamesRestricted.Find(id);
            if (manualassign_tweetrandomsample2_to_geonamesrestricted == null)
            {
                return HttpNotFound();
            }
            return View(manualassign_tweetrandomsample2_to_geonamesrestricted);
        }

        // GET: /geonames_to_tweets_/Create
        public ActionResult Create()
        {
            ViewBag.geoNamesRestricted_geonameid = new SelectList(db.GeoNamesRestricted, "geonameid", "name");
            ViewBag.tweetRandomSample2_id = new SelectList(db.tweetRandomSample2, "id", "username");
            return View();
        }

        // POST: /geonames_to_tweets_/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,tweetRandomSample2_id,tweetId,is_unambiguously,geoNamesRestricted_geonameid,comment,misc")] manualAssign_tweetRandomSample2_to_geonamesRestricted manualassign_tweetrandomsample2_to_geonamesrestricted)
        {
            if (ModelState.IsValid)
            {
                db.manualAssign_tweetRandomSample2_to_geonamesRestricted.Add(manualassign_tweetrandomsample2_to_geonamesrestricted);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.geoNamesRestricted_geonameid = new SelectList(db.GeoNamesRestricted, "geonameid", "name", manualassign_tweetrandomsample2_to_geonamesrestricted.geoNamesRestricted_geonameid);
            ViewBag.tweetRandomSample2_id = new SelectList(db.tweetRandomSample2, "id", "username", manualassign_tweetrandomsample2_to_geonamesrestricted.tweetRandomSample2_id);
            return View(manualassign_tweetrandomsample2_to_geonamesrestricted);
        }

        // GET: /geonames_to_tweets_/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            manualAssign_tweetRandomSample2_to_geonamesRestricted manualassign_tweetrandomsample2_to_geonamesrestricted = db.manualAssign_tweetRandomSample2_to_geonamesRestricted.Find(id);
            if (manualassign_tweetrandomsample2_to_geonamesrestricted == null)
            {
                return HttpNotFound();
            }
            ViewBag.geoNamesRestricted_geonameid = new SelectList(db.GeoNamesRestricted, "geonameid", "name", manualassign_tweetrandomsample2_to_geonamesrestricted.geoNamesRestricted_geonameid);
            ViewBag.tweetRandomSample2_id = new SelectList(db.tweetRandomSample2, "id", "username", manualassign_tweetrandomsample2_to_geonamesrestricted.tweetRandomSample2_id);
            return View(manualassign_tweetrandomsample2_to_geonamesrestricted);
        }

        // POST: /geonames_to_tweets_/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,tweetRandomSample2_id,tweetId,is_unambiguously,geoNamesRestricted_geonameid,comment,misc")] manualAssign_tweetRandomSample2_to_geonamesRestricted manualassign_tweetrandomsample2_to_geonamesrestricted)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manualassign_tweetrandomsample2_to_geonamesrestricted).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.geoNamesRestricted_geonameid = new SelectList(db.GeoNamesRestricted, "geonameid", "name", manualassign_tweetrandomsample2_to_geonamesrestricted.geoNamesRestricted_geonameid);
            ViewBag.tweetRandomSample2_id = new SelectList(db.tweetRandomSample2, "id", "username", manualassign_tweetrandomsample2_to_geonamesrestricted.tweetRandomSample2_id);
            return View(manualassign_tweetrandomsample2_to_geonamesrestricted);
        }

        // GET: /geonames_to_tweets_/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            manualAssign_tweetRandomSample2_to_geonamesRestricted manualassign_tweetrandomsample2_to_geonamesrestricted = db.manualAssign_tweetRandomSample2_to_geonamesRestricted.Find(id);
            if (manualassign_tweetrandomsample2_to_geonamesrestricted == null)
            {
                return HttpNotFound();
            }
            return View(manualassign_tweetrandomsample2_to_geonamesrestricted);
        }

        // POST: /geonames_to_tweets_/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            manualAssign_tweetRandomSample2_to_geonamesRestricted manualassign_tweetrandomsample2_to_geonamesrestricted = db.manualAssign_tweetRandomSample2_to_geonamesRestricted.Find(id);
            db.manualAssign_tweetRandomSample2_to_geonamesRestricted.Remove(manualassign_tweetrandomsample2_to_geonamesrestricted);
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
