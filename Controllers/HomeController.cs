﻿using ReactMvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ReactMvc.Controllers
{
    public class HomeController : Controller
    {
        private DictionaryEntities db = new DictionaryEntities();
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public async Task<ActionResult> Words()
        {
            var words = await db.Words.ToListAsync();
            //return Json(words.OrderByDescending(w=>w.Id).Take(20), JsonRequestBehavior.AllowGet);
            return Json(words.OrderByDescending(w=>w.Id), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "French,Vietnam,Type")] Word word)
        {
            word.CreatedOn = DateTime.Now;
            //var user = User.Identity.Name;
            //word.CreatedBy = user.Substring(Math.Max(0, user.Length - 10), Math.Min(10, user.Length - 1));
            db.Words.Add(word);
            await db.SaveChangesAsync();
            return Content("Success :)");
        }
    }
}