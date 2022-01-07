using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Airbnb_PWEB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Airbnb_PWEB.Controllers
{
    public class RolesController : Controller
    {

        RoleManager<IdentityRole> roleManager;
        UserManager<ApplicationUser> userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;

        }

        public IActionResult Index()
        {
            return View(roleManager.Roles.ToList());
        }

        public async Task<IActionResult> Details(string id)
        {

            var role = roleManager.FindByIdAsync(id);

            if (role != null) 
            {
                var listaDeUtilizadores = await userManager.GetUsersInRoleAsync(role.Result.Name);
                if (listaDeUtilizadores != null) { 
                    RoleViewModel roleViewModel = new RoleViewModel
                    {
                        RoleId = role.Result.Id,
                        Nome = role.Result.Name,
                        Utilizadores = listaDeUtilizadores
                    };
                    return View(roleViewModel);
                }
                return NotFound();

            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit()
        {
            return View("Edit");
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await userManager.DeleteAsync(user);
            return View("Details");
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(ApplicationUser user)
        {

            await userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }



    }
}
