using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Airbnb_PWEB.Data;
using Airbnb_PWEB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Airbnb_PWEB.Controllers
{
    public class ClientEvaluationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public ClientEvaluationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ClientEvaluations
        [Authorize(Roles = "Owner_Manager,Owner_Employeer")]
        public async Task<IActionResult> Index(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            // vista do Manager (ve todas as avaliações realizadas)
            if (id == null)
            {
                
                var applicationDbContext = _context.ClientEvaluations.Include(c => c.Reservation.ApplicationUser).Include(c => c.Company).Include(c => c.Reservation).Where(c => c.Company.Owner == user);
                return View(await applicationDbContext.ToListAsync());
            }
            // vista do Funcionário da Empresa
            var myCompany = _context.Companies.Where(c => c.Employeers.Contains(user)).FirstOrDefault();
            var evaluationlist = _context.ClientEvaluations.Include(c=>c.Reservation.ApplicationUser).Include(c => c.Company).Where(e => e.Reservation.ApplicationUser.Id == id && e.Company == myCompany);
            return View(await evaluationlist.ToListAsync());
        }

        [Authorize(Roles = "Owner_Manager,Owner_Employeer")]
        // GET: ClientEvaluations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientEvaluation = await _context.ClientEvaluations
                .Include(c => c.Reservation)
                .FirstOrDefaultAsync(m => m.ClientEvaluationId == id);
            if (clientEvaluation == null)
            {
                return NotFound();
            }

            return View(clientEvaluation);
        }

        // GET: ClientEvaluations/Create
        [Authorize(Roles = "Owner_Manager")]
        public IActionResult Create(int id)
        {
            var clientEvaluation = _context.ClientEvaluations.Where(e => e.ReservationId == id).Count();
            if(clientEvaluation == 0)
            {
                return View();
            }
            var reservation = _context.Reservations.Include(p => p.Property).Where(p => p.ReservationId == id).FirstOrDefault();
            TempData["AlertMessage"] = "A comment has already been made for this reservation";
            return RedirectToAction(nameof(Index),"Reservations", new { id = reservation.PropertyId });

        }

        // POST: ClientEvaluations/Create
        [Authorize(Roles = "Owner_Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientEvaluationId, Company, Comment,ReservationId")] ClientEvaluation clientEvaluation,int id)
        {
            clientEvaluation.ReservationId =id;

            var user = await _userManager.GetUserAsync(User);
            var company = _context.Companies.Where(c => c.Owner == user).FirstOrDefault(); // companhia do owner conectado no momento
            clientEvaluation.Company = company;

            if (ModelState.IsValid)
            {
                _context.Add(clientEvaluation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId", clientEvaluation.ReservationId);
            return View(clientEvaluation);
        }

        [Authorize(Roles = "Owner_Manager")]
        // GET: ClientEvaluations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientEvaluation = await _context.ClientEvaluations.FindAsync(id);
            if (clientEvaluation == null)
            {
                return NotFound();
            }
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId", clientEvaluation.ReservationId);
            return View(clientEvaluation);
        }

        [Authorize(Roles = "Owner_Manager")]
        // POST: ClientEvaluations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,ClientEvaluation clientEvaluation)
        {
            var clientEvaluation2 = _context.ClientEvaluations.Where(c=> c.ClientEvaluationId == id).FirstOrDefault();
            if (id != clientEvaluation.ClientEvaluationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    clientEvaluation2.Comment = clientEvaluation.Comment;
                    _context.Update(clientEvaluation2);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientEvaluationExists(clientEvaluation.ClientEvaluationId))
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
            //ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId", clientEvaluation.ReservationId);
            return View(clientEvaluation);
        }

        [Authorize(Roles = "Owner_Manager")]
        // GET: ClientEvaluations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientEvaluation = await _context.ClientEvaluations
                .Include(c => c.Reservation)
                .FirstOrDefaultAsync(m => m.ClientEvaluationId == id);
            if (clientEvaluation == null)
            {
                return NotFound();
            }

            return View(clientEvaluation);
        }

        // POST: ClientEvaluations/Delete/5
        [Authorize(Roles = "Owner_Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientEvaluation = await _context.ClientEvaluations.FindAsync(id);
            _context.ClientEvaluations.Remove(clientEvaluation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientEvaluationExists(int id)
        {
            return _context.ClientEvaluations.Any(e => e.ClientEvaluationId == id);
        }
    }
}
