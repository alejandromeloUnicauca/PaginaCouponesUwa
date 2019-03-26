using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cupones.Models;

namespace Cupones.Controllers
{
    public class CuoponCodeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CuoponCodeModels
        public async Task<ActionResult> Index()
        {
            return View(await db.CuoponCodeModels.ToListAsync());
        }

        // GET: CuoponCodeModels/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CouponCodeModel cuoponCodeModel = await db.CuoponCodeModels.FindAsync(id);
            if (cuoponCodeModel == null)
            {
                return HttpNotFound();
            }
            return View(cuoponCodeModel);
        }

        // GET: CuoponCodeModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CuoponCodeModels/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "idCouponCode,code")] CouponCodeModel cuoponCodeModel)
        {
            if (ModelState.IsValid)
            {
                db.CuoponCodeModels.Add(cuoponCodeModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(cuoponCodeModel);
        }

        // GET: CuoponCodeModels/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CouponCodeModel cuoponCodeModel = await db.CuoponCodeModels.FindAsync(id);
            if (cuoponCodeModel == null)
            {
                return HttpNotFound();
            }
            return View(cuoponCodeModel);
        }

        // POST: CuoponCodeModels/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "idCouponCode,code")] CouponCodeModel cuoponCodeModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cuoponCodeModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cuoponCodeModel);
        }

        // GET: CuoponCodeModels/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CouponCodeModel cuoponCodeModel = await db.CuoponCodeModels.FindAsync(id);
            if (cuoponCodeModel == null)
            {
                return HttpNotFound();
            }
            return View(cuoponCodeModel);
        }

        // POST: CuoponCodeModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CouponCodeModel cuoponCodeModel = await db.CuoponCodeModels.FindAsync(id);
            db.CuoponCodeModels.Remove(cuoponCodeModel);
            await db.SaveChangesAsync();
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
