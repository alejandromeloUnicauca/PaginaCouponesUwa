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
    [Authorize]
    public class CouponController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        // GET: Coupon
        public async Task<ActionResult> Index()
        {
            return View(await db.Coupons.ToListAsync());
        }

        [AllowAnonymous]
        // GET: Coupon/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CouponModel couponModel = await db.Coupons.FindAsync(id);
            if (couponModel == null)
            {
                return HttpNotFound();
            }
            return View(couponModel);
        }

        // GET: Coupon/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Coupon/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "idCoupon,name,baseCost,saleCost,originalCost,description,stock,enable,expirationDate")] CouponModel couponModel)
        {
            if (ModelState.IsValid)
            {

                db.Coupons.Add(couponModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Coupon/Index");
            }

            return View(couponModel);
        }

        // GET: Coupon/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CouponModel couponModel = await db.Coupons.FindAsync(id);
            if (couponModel == null)
            {
                return HttpNotFound();
            }
            return View(couponModel);
        }

        // POST: Coupon/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "idCoupon,name,baseCost,saleCost,originalCost,description,stock,enable,expirationDate")] CouponModel couponModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(couponModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(couponModel);
        }

        // GET: Coupon/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CouponModel couponModel = await db.Coupons.FindAsync(id);
            if (couponModel == null)
            {
                return HttpNotFound();
            }
            return View(couponModel);
        }

        // POST: Coupon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CouponModel couponModel = await db.Coupons.FindAsync(id);
            db.Coupons.Remove(couponModel);
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
