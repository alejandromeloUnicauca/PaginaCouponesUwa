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
    
    public class CouponController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public CouponController()
        {

        }

        public CouponController(ApplicationUserManager userManager)
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


        
        // GET: Coupon
        [Authorize(Roles = "Publicista")]
        public async Task<ActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await UserManager.FindByNameAsync(User.Identity.Name);
                return View(await db.Coupons.Where(x => x.UserId == user.Id).ToListAsync());
            }
            else return View();
        }

        
        // GET: Coupon/Details/5
        [Authorize(Roles = "Publicista")]
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
        [Authorize(Roles = "Publicista")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Coupon/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Publicista")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( CouponModel couponModel, HttpPostedFileBase fileImg)
        {
            if (ModelState.IsValid)
            {
                var path = "../../fileUpload/" + fileImg.FileName;
                if (fileImg.ContentLength > 0)
                {
                    var user = await UserManager.FindByNameAsync(User.Identity.Name);   

                    couponModel.UserId = user.Id;
                    couponModel.pathImg = path;
                    fileImg.SaveAs(Server.MapPath("~/fileUpload/")+fileImg.FileName);
                    db.Coupons.Add(couponModel);
                    ViewBag.Msg = "Uploaded Successfully";
                    await db.SaveChangesAsync();
                    
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Msg = "Uploaded Failed";
                }
            }

            return View(couponModel);
        }

        // GET: Coupon/Edit/5
        [Authorize(Roles = "Publicista")]
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
        [Authorize(Roles = "Publicista")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CouponModel couponModel, HttpPostedFileBase fileImg)
        {
            if (ModelState.IsValid)
            {
                var path = "../../fileUpload/" + fileImg.FileName;
                if (fileImg.ContentLength > 0)
                {
                    var user = await UserManager.FindByNameAsync(User.Identity.Name);

                    couponModel.UserId = user.Id;
                    couponModel.pathImg = path;
                    fileImg.SaveAs(Server.MapPath("~/fileUpload/") + fileImg.FileName);
                    db.Entry(couponModel).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    ViewBag.Msg = "Uploaded Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Msg = "Uploaded Failed";
                }
            }
            return View(couponModel);
        }

        // GET: Coupon/Delete/5
        [Authorize(Roles = "Publicista")]
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
        [Authorize(Roles = "Publicista")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CouponModel couponModel = await db.Coupons.FindAsync(id);
            db.Coupons.Remove(couponModel);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<ActionResult> BuyCoupon(int? id)
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


        [Authorize]
        public async Task<ActionResult> ConfirmBuy(CouponModel couponModel)
        {
            if (couponModel!=null)
            {
                CouponCodeModel cuponCodeModel = new CouponCodeModel();

                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[8];
                var random = new Random();
                var code = "";
                
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                code = new String(stringChars);

                cuponCodeModel.code = code;
                cuponCodeModel.idCoupon = couponModel.idCoupon;

                var user = await UserManager.FindByNameAsync(User.Identity.Name);
                cuponCodeModel.UserId = user.Id;
                db.CuoponCodeModels.Add(cuponCodeModel);
                await db.SaveChangesAsync();

                return RedirectToAction("BuyDetail", cuponCodeModel);
            }
            return HttpNotFound();
        }

        [Authorize]
        public ActionResult BuyDetail(CouponCodeModel couponCode)
        {
            if (couponCode != null)
            {
                return View(couponCode);
            }
            return HttpNotFound();
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
