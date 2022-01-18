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
            if(ViewBag.Message != null)
                ViewBag.Message = TempData["shortMessage"].ToString();
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
            var userOld = await context.Users.FindAsync(user.Id);
            
            if (user.Id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    userOld.FirstName = user.FirstName;
                    userOld.LastName = user.LastName;
                    userOld.PhoneNumber = user.PhoneNumber;

                    if(user.FunctionRole == null)   // gestor a editar dados do funcionario
                    {
                        context.Update(userOld);
                        await context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index),"Companies");
                    }

                    if (user.FunctionRole.Equals("Client") && userOld.FunctionRole.Equals("Owner_Manager")){// ver se nao tem funcionarios nem imoveis dele
                        var companyOwner = context.Companies.Include(p=> p.Employeers).Where(p => p.Owner == userOld).FirstOrDefault();
                        var property = context.Properties.Include(p=>p.Company).Where(p => p.Company == companyOwner).FirstOrDefault();

                        if(companyOwner.Employeers.Count==0 && property == null)
                        {
                            TempData["SuccessMessage"] = "Success changing Role from Manager to Client";
                            userOld.FunctionRole = user.FunctionRole;
                            await userManager.RemoveFromRoleAsync(userOld, "Owner_Manager");
                            await userManager.AddToRoleAsync(userOld, "Client");
                        }
                        else
                        {
                            TempData["AlertMessage"] = "Error! The Manager displays properties or employees associated with your company!";  
                            return RedirectToAction(nameof(Index));
                            //ModelState.AddModelError("Function Role", "O Gestor apresenta propriedades ou funcionarios associados à sua empresa");
                            //return View();
                        }

                    }
                    else if(user.FunctionRole.Equals("Owner_Manager") && userOld.FunctionRole.Equals("Client"))
                    {
                        userOld.FunctionRole = user.FunctionRole;
                        await userManager.RemoveFromRoleAsync(userOld, "Client");
                        await userManager.AddToRoleAsync(userOld, "Owner_Manager");
                        Company company = new Company()
                        {
                            Owner = userOld,
                            Employeers = new List<ApplicationUser>()
                        };
                        context.Add(company);
                        await context.SaveChangesAsync();
                    }

                    context.Update(userOld);
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
