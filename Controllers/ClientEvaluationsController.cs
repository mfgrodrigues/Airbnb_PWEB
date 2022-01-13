using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Airbnb_PWEB.Data;
using Airbnb_PWEB.Models;

namespace Airbnb_PWEB.Controllers
{
    public class ClientEvaluationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientEvaluationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ClientEvaluations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ClientEvaluations.Include(c => c.Reservation);
            return View(await applicationDbContext.ToListAsync());
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
        public IActionResult Create()
        {
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId");
            return View();
        }

        // POST: ClientEvaluations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientEvaluationId,Comment,ReservationId")] ClientEvaluation clientEvaluation,int reservationId)
        {
            clientEvaluation.ReservationId = reservationId;

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
        public async Task<IActionResult> Edit(int id, [Bind("ClientEvaluationId,Comment,ReservationId")] ClientEvaluation clientEvaluation)
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
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId", clientEvaluation.ReservationId);
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
