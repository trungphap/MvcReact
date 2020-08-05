using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ReactMvc.Models;

namespace ReactMvc.Controllers
{
    [RoutePrefix("api/WordsNg")]
    public class WordsNgController : ApiController
    {
        private Entities db = new Entities();
        [HttpGet]
        [Route("GetWords")]
        // GET: api/WordsNg
        public IQueryable<Word> GetWords()
        {
            return db.Words.OrderByDescending(x => x.Id);         
        }

        [HttpGet]
        [Route("GetTypes")]
        // GET: api/WordsNg
        public IQueryable<string> GetTypes()
        {
            return db.Words.Select(x => x.Type).Distinct();
        }
        // GET: api/WordsNg/5
        [HttpGet]
        [Route("GetWord/{id}")]
        [ResponseType(typeof(Word))]
        public async Task<IHttpActionResult> GetWord(int id)
        {
            Word word = await db.Words.FindAsync(id);
            if (word == null)
            {
                return NotFound();
            }

            return Ok(word);
        }

        [HttpPut]
        [Route("PutWord")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutWord(Word word)
        {            
            var id = word.Id;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(word).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WordExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
       

        // POST: api/WordsNg
        [ResponseType(typeof(Word))]
        public async Task<IHttpActionResult> PostWord(Word word)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Words.Add(word);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = word.Id }, word);
        }

        // DELETE: api/WordsNg/5
        //[ResponseType(typeof(Word))]
        //public async Task<IHttpActionResult> DeleteWord(int id)
        //{
        //    Word word = await db.Words.FindAsync(id);
        //    if (word == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Words.Remove(word);
        //    await db.SaveChangesAsync();

        //    return Ok(word);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WordExists(int id)
        {
            return db.Words.Count(e => e.Id == id) > 0;
        }
    }
}