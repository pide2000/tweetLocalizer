using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TwitterWebFrontend.Models;
using TwitterWebFrontend.ViewModels;

namespace TwitterWebFrontend.Controllers
{
    public class manualAssignmentController : Controller
    {
        private GeonamesDataEntities1 db = new GeonamesDataEntities1();

        // GET: /manualAssignment/
        public ActionResult Index()
        {
            List<get_tweetRandomSample2_notProcessed_Result> tweetRandomSampleList = db.get_tweetRandomSample2_notProcessed().ToList();
            var manualAssignementView = new manualAssignementViewModel();
            List<contains_search_geoNamesRestricted_Result> list1 = (db.contains_search_geoNamesRestricted("\"" + tweetRandomSampleList.First().userlocation + "\"")).ToList();
            List<contains_search_alternateNames3Join_Result> list2 = (db.contains_search_alternateNames3Join("\"" + tweetRandomSampleList.First().userlocation + "\"")).ToList();
            manualAssignementView.containsSearchGeoNamesRestricted = list1;
            manualAssignementView.containsSearchAlternateNames3 = list2;
            
            manualAssignementView.tweetRandomSample2 = tweetRandomSampleList;
            
            return View(manualAssignementView);
        }

        // GET: /_geoNamesRestricted/
        public PartialViewResult _geoNamesRestricted(string q)
        {
            var manualAssignementView = new manualAssignementViewModel();
            List<contains_search_geoNamesRestricted_Result> list1 = (db.contains_search_geoNamesRestricted("\""+ q +"\"")).ToList();
            List<contains_search_alternateNames3Join_Result> list2 = (db.contains_search_alternateNames3Join("\""+ q +"\"")).ToList();
            manualAssignementView.containsSearchGeoNamesRestricted = list1;
            manualAssignementView.containsSearchAlternateNames3 = list2;
            return this.PartialView(manualAssignementView);
        }

        // GET: /manualAssignment/Details/5
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

        // GET: /manualAssignment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /manualAssignment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( manualAssignementViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                db.manualAssign_tweetRandomSample2_to_geonamesRestricted.Add(viewmodel.manualAssign);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else {
                var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();
            }


            return View(viewmodel.manualAssign);
        }



        // GET: /manualAssignment/Edit/5
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

        // POST: /manualAssignment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,tweetid,username,lon,lat,hashtag,time,status,tweetTime,skipped,place,geotagged,userlocation,lang,utc_offset,timezone,coord,processed")] manualAssign_tweetRandomSample2_to_geonamesRestricted manualAssign_trs2_to_gr)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manualAssign_trs2_to_gr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manualAssign_trs2_to_gr);
        }

        // GET: /manualAssignment/Delete/5
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

        // POST: /manualAssignment/Delete/5
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
