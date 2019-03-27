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
using Microsoft.AspNet.Identity.Owin;

namespace Cupones.Controllers
{
    public class CouponCodeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
         private ApplicationUserManager _userManager;

        public CouponCodeController()
        {

        }

        public CouponCodeController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [Authorize(Roles="Empresa")]
        // GET: CouponCode
        public async Task<ActionResult> Index()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            List<EmpresaModel> empresas = await db.EmpresaModel.Where(e => e.UserId == user.Id).ToListAsync();
            EmpresaModel empresa = empresas.ElementAt(0);
            List<CouponModel> cupones = await db.Coupons.Where(x => x.idEmpresa == empresa.idEmpresa).ToListAsync();
            return View(cupones);
        }

        // GET: CouponCode/Details/5
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
            var msg = TempData["msg"] as string;
            ViewBag.msg = msg;
            CouponCodeModel couponCode = new CouponCodeModel();
            CouponCodeViewModel couponViewModel = new CouponCodeViewModel() { couponModel = couponModel, couponCode = couponCode };
            return View(couponViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Redimir(CouponCodeViewModel cuponcodemodel)
        {
            if (cuponcodemodel != null)
            {
                List<CouponCodeModel> cupones = await db.CuoponCodeModels.Where(x => x.code == cuponcodemodel.couponCode.code).ToListAsync();
                if (cupones.Count > 0)
                {
                    CouponCodeModel cuponCode = cupones.ElementAt(0);
                    if (cuponCode.status)
                    {
                        TempData["msg"] = "El cupon ya esta redimido";
                        return RedirectToAction("Details", new { id = cuponcodemodel.couponModel.idCoupon });
                    }
                    else
                    {
                        cuponCode.status = true;
                        db.Entry(cuponCode).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        TempData["msg"] = "Cupon redimido con exito";
                        return RedirectToAction("Details", new { id = cuponcodemodel.couponModel.idCoupon });
                    }
                }
                else
                {
                    TempData["msg"] = "No se encontro el codigo";
                    return RedirectToAction("Details", new { id = cuponcodemodel.couponModel.idCoupon});
                }
            }
            return View();
        }




        // GET: CouponCode/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            ViewBag.idCoupon = new SelectList(db.Coupons, "idCoupon", "name");
            return View();
        }


        // POST: CouponCode/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "idCouponCode,code,status,idCoupon,UserId")] CouponCodeModel couponCodeModel)
        {
            if (ModelState.IsValid)
            {
                db.CuoponCodeModels.Add(couponCodeModel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", couponCodeModel.UserId);
            ViewBag.idCoupon = new SelectList(db.Coupons, "idCoupon", "name", couponCodeModel.idCoupon);
            return View(couponCodeModel);
        }

        // GET: CouponCode/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CouponCodeModel couponCodeModel = await db.CuoponCodeModels.FindAsync(id);
            if (couponCodeModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", couponCodeModel.UserId);
            ViewBag.idCoupon = new SelectList(db.Coupons, "idCoupon", "name", couponCodeModel.idCoupon);
            return View(couponCodeModel);
        }

        // POST: CouponCode/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "idCouponCode,code,status,idCoupon,UserId")] CouponCodeModel couponCodeModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(couponCodeModel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", couponCodeModel.UserId);
            ViewBag.idCoupon = new SelectList(db.Coupons, "idCoupon", "name", couponCodeModel.idCoupon);
            return View(couponCodeModel);
        }

        // GET: CouponCode/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CouponCodeModel couponCodeModel = await db.CuoponCodeModels.FindAsync(id);
            if (couponCodeModel == null)
            {
                return HttpNotFound();
            }
            return View(couponCodeModel);
        }

        // POST: CouponCode/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CouponCodeModel couponCodeModel = await db.CuoponCodeModels.FindAsync(id);
            db.CuoponCodeModels.Remove(couponCodeModel);
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
