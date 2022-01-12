using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Airbnb_PWEB.Data;
using Airbnb_PWEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Airbnb_PWEB.Controllers
{
    [Authorize(Roles ="Admin,Owner_Manager")]
    public class RolesController : Controller
    {

        RoleManager<IdentityRole> roleManager;
        UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.context = context;

        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var selectRoles = roleManager.Roles.Where(r => !r.Name.Equals("Owner_Employeer"));
            
            return View(selectRoles.ToList());
        }

        public async Task<IActionResult> Details(string id)
        {
            if(id == null)
                return NotFound();

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


        public async Task<ActionResult> ManageAccounts(string id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var countUser = await userManager.FindByIdAsync(id);

            if (countUser == null)
            {
                return NotFound();
            }

            return View(countUser);
        }

        public async Task<IActionResult> Ativate(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var countUser = await userManager.FindByIdAsync(id);

            countUser.EmailConfirmed = true;
            context.SaveChanges();
            context.Update(countUser);
            if (User.IsInRole("Owner_Manager"))
            {
                return RedirectToAction(nameof(Index),"Companies");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Desactivate(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var countUser = await userManager.FindByIdAsync(id);
            countUser.EmailConfirmed = false;
            context.SaveChanges();
            context.Update(countUser);
            if (User.IsInRole("Owner_Manager"))
            {
                return RedirectToAction(nameof(Index), "Companies");
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Back()
        {
            if (User.IsInRole("Owner_Manager"))
                return RedirectToAction(nameof(Index), "Companies");

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var countUser = await userManager.FindByIdAsync(id);
            if (countUser == null)
            {
                return NotFound();
            }
            return View(countUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            var user2 = await context.Users.FindAsync(user.Id);
            
            if (user.Id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user2.FirstName = user.FirstName;
                    user2.LastName = user.LastName;
                    user2.PhoneNumber = user.PhoneNumber;
                   
                    context.Update(user2);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));  
            }
            return View(user);
        }

        private bool UserExists(string id)
        {
            return context.Users.Any(e => e.Id == id);
        }


    }
}
