using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReactMvc.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using System.Web.Http;

namespace ReactMvc.Controllers
{
    //[EnableCors(origins: "https://nha-en.web.app", headers: "*", methods: "*")]
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class WordsController : Controller
    {
        private Entities db = new Entities();

        // GET: Words
        public ActionResult Index()
        {

            return View();
        }

        public async Task<ActionResult> Words_Get()
        {
            var user = Session["User"] as string;
            if (user != null)
                user = user.Trim();
            var words = await db.Words.OrderByDescending(w => w.Id).ToListAsync();
            if (string.IsNullOrEmpty(user) || string.IsNullOrWhiteSpace(user))
            {
                words = words.Take(10).ToList();
                Session["Edit"] = string.Empty;
            }
            else if (user != "admin" && user != "nhaen")
            {
                words = words.Where(w => w.owner == user).OrderByDescending(w => w.Id).ToList();
            }

            return Json(words, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> Word_Get(int id)
        {
            var word = await db.Words.Where(w => w.Id == id).FirstOrDefaultAsync();
            return Json(word, JsonRequestBehavior.AllowGet);
        }
        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Put | HttpVerbs.Options)]

        public async Task<ActionResult> Word_Update( Word word)
        {
            Response.StatusCode = 200;
            //string jsonData = Request.Form[0];
            //var word = JsonConvert.DeserializeObject<Word>(jsonData);
            
            var wordExistant = await db.Words.Where(w => w.Id == word.Id).FirstOrDefaultAsync();
            if(wordExistant != null)
            {
                wordExistant.Prononciation = word.Prononciation;
                wordExistant.Exemple = word.Exemple;
                word.ModifiedOn = DateTime.Now;
                db.Entry(wordExistant).Property(w=>w.Prononciation).IsModified=true;
                db.Entry(wordExistant).Property(w=>w.Exemple).IsModified=true;
                await db.SaveChangesAsync();
            }
            return Json(wordExistant, JsonRequestBehavior.AllowGet);
        }
        //[HttpOptions]
        //public async Task<ActionResult> Word_Update()
        //{
        //    Response.StatusCode = 200;
        //    HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
        //    return ;
        //}

        public async Task<ActionResult> Words_Read([DataSourceRequest]DataSourceRequest request)
        {
            var user = Session["User"] as string;
            if (user != null)
                user = user.Trim();
            var words = await db.Words.OrderByDescending(w => w.Id).ToListAsync();
            if (string.IsNullOrEmpty(user) || string.IsNullOrWhiteSpace(user))
            {
                words = words.Take(10).ToList();
                Session["Edit"] = string.Empty;
            }
            else if (user != "admin" && user != "nhaen")
            {
                words = words.Where(w => w.owner == user).OrderByDescending(w => w.Id).ToList();
            }

            return Json(words.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> Words_Update([DataSourceRequest] DataSourceRequest request, Word word)
        {
            if (word != null && ModelState.IsValid)
            {
                word.ModifiedOn = DateTime.Now;
                db.Entry(word).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }

            return Json(new[] { word }.ToDataSourceResult(request, ModelState));
        }
        // GET: Words/Details/5
        //public async Task<ActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Word word = await db.Words.FindAsync(id);
        //    if (word == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(word);
        //}

        //// GET: Words/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Words/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "Id,French,Vietnam,Type,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] Word word)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Words.Add(word);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    return View(word);
        //}
        public ActionResult Edit()
        {
            return View();
        }
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
       

        public ActionResult Deconnecter(string user, string pass)
        {
            Session["Edit"] = string.Empty;
            Session["User"] = string.Empty;
            Session["NbInscrit"] = string.Empty;
            return View("Index");
        }

        // GET: Words/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Word word = await db.Words.FindAsync(id);
        //    if (word == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(word);
        //}

        // POST: Words/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Id,French,Vietnam,Type,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] Word word)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(word).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(word);
        //}

        // GET: Words/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Word word = await db.Words.FindAsync(id);
        //    if (word == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(word);
        //}

        //// POST: Words/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Word word = await db.Words.FindAsync(id);
        //    db.Words.Remove(word);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

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
