using GroupProject.Data;
using GroupProject.Models;
using GroupProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GroupProject.API
{
    public class ProductsApiController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<ApiProduct> GetProducts()
        {
            var products = db.Products.Include(p => p.Manufacturer).Include(p => p.Category);
            List<ApiProduct> apiProducts = new List<ApiProduct>();
            int i = 0;
            foreach (var product in products)
            {
                apiProducts.Add(new ApiProduct());
                apiProducts[i].Manufacturer = product.Manufacturer.Name;
                apiProducts[i].Name = product.Name;
                apiProducts[i].Price = product.Price;
                apiProducts[i].Category = product.Category.Name;
                i++;
            }

            return apiProducts;
        }
        // GET: api/Trainers/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Trainers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ID)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Trainers
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostTrainer(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.ID }, product);
        }

        // DELETE: api/Trainers/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteTrainer(int id)
        {
            Product trainer = db.Products.Find(id);
            if (trainer == null)
            {
                return NotFound();
            }

            db.Products.Remove(trainer);
            db.SaveChanges();

            return Ok(trainer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ID == id) > 0;
        }
    }
}
