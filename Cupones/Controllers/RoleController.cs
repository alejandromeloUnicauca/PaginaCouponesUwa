using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Cupones.Models;
using System.Threading.Tasks;

namespace Cupones.Controllers
{
    [Authorize(Roles="Admin")]
    public class RoleController : Controller
    {

        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();
        public RoleController()
        {
        }

        public RoleController(ApplicationRoleManager rolManager, ApplicationUserManager userManager)
        {
            RoleManager = rolManager;
            UserManager = userManager;
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().Get<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Role
        public ActionResult Index()
        {
            List<RoleViewModel> list = new List<RoleViewModel>();
            foreach (var role in RoleManager.Roles)
            {
                list.Add(new RoleViewModel(role));
            }
            return View(list);
        }

        public ActionResult AssignRoleUser()
        {
            ViewBag.Name = new SelectList(db.Roles.ToList(), "Name", "Name");
            ViewBag.UserName = new SelectList(db.Users.ToList(), "UserName", "UserName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssignRoleUser(RegisterViewModel model, RoleViewModel model2)
        {
            var role = new ApplicationRole() { Name = model2.Name };
            var user = new ApplicationUser() { UserName = model.UserName };
            var id = await UserManager.FindByNameAsync(user.UserName);
            await UserManager.AddToRoleAsync(id.Id, role.Name);
            //await RoleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleViewModel model)
        {
            var role = new ApplicationRole() { Name = model.Name };
            await RoleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new RoleViewModel(role));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleViewModel model)
        {
            var role = new ApplicationRole() {Id = model.Id, Name = model.Name };
            await RoleManager.UpdateAsync(role);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Details(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new RoleViewModel(role));
        }

        public async Task<ActionResult> Delete(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new RoleViewModel(role));
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            await RoleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }
    }
}