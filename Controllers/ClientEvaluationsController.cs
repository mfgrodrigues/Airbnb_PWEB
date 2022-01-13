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
        public async Task<IActionResult> Index(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            // vista do Manager (ve todas as avaliações realizadas)
            if (id == null)
            {
                
                var applicationDbContext = _context.ClientEvaluations.Include(c => c.Company).Include(c => c.Reservation).Where(c => c.Company.Owner == user);
                return View(await applicationDbContext.ToListAsync());
            }
            // vista do Funcionário da Emrpresa
            var myCompany = _context.Companies.Where(c => c.Employeers.Contains(user)).FirstOrDefault();
            var evaluationlist = _context.ClientEvaluations.Include(c => c.Company).Where(e => e.Reservation.ApplicationUser.Id == id && e.Company == myCompany);
            return View(await evaluationlist.ToListAsync());
        }

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
        public IActionResult Create(int id)
        {
            var clientEvaluation = _context.ClientEvaluations.Where(e => e.ReservationId == id).Count();
            if(clientEvaluation == 0)
            {
                return View();
            }
            var reservation = _context.Reservations.Include(p => p.Property).Where(p => p.ReservationId == id).FirstOrDefault();
            //ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId");
            return RedirectToAction("Index");

        }

        // POST: ClientEvaluations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // POST: ClientEvaluations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientEvaluationId,Company, Comment,ReservationId")] ClientEvaluation clientEvaluation)
        {
            if (id != clientEvaluation.ClientEvaluationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientEvaluation);
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
