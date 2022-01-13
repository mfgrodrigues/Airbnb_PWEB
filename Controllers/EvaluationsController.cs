using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Airbnb_PWEB.Data;
using Airbnb_PWEB.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Airbnb_PWEB.Controllers
{
    public class EvaluationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public EvaluationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Evaluations
        public async Task<IActionResult> Index()    // cliente
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var reserve = await _context.Reservations.Include(r => r.Property).Where(r => r.ApplicationUser == currentUser).ToListAsync();
 
            var evaluationList = new List<Evaluation>();

            foreach (var item in reserve)
            {
                var evaluation = await _context.Evaluation.Include(r => r.Reservation).Where(r => r.Reservation == item).FirstOrDefaultAsync();
                if(evaluation != null)
                    evaluationList.Add(evaluation);

            }
            if (evaluationList != null)
            {
                return View(evaluationList);
            }

            return NotFound();
            
        }

        // GET: Evaluations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluation = await _context.Evaluation
                .Include(e => e.Reservation)
                .FirstOrDefaultAsync(m => m.EvaluationId == id);
            if (evaluation == null)
            {
                return NotFound();
            }

            return View(evaluation);
        }

        // GET: Evaluations/Create
        public IActionResult Create(int id)
        {
            var evaluationAvailable = _context.Evaluation.Where(e => e.ReservationId == id).Count();
            if (evaluationAvailable > 0) {
                return RedirectToAction(nameof(Index),"Reservations");
            }
            //ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId");

            return View();
        }

        // POST: Evaluations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EvaluationId,Comment,Classification,ReservationId")] Evaluation evaluation,int id)
        {
            evaluation.ReservationId = id;
            //evaluation.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (ModelState.IsValid)
            {
                _context.Add(evaluation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId", evaluation.ReservationId);
            return View(evaluation);
        }

        // GET: Evaluations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluation = await _context.Evaluation
                .Include(e => e.Reservation)
                .FirstOrDefaultAsync(m => m.EvaluationId == id);
            if (evaluation == null)
            {
                return NotFound();
            }
            //ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId", evaluation.ReservationId);
            return View(evaluation);
        }

        // POST: Evaluations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EvaluationId,Comment,Classification, ReservationId")] Evaluation evaluation)
        {
            if (id != evaluation.EvaluationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Entry(evaluation).State = EntityState.Modified;
                    //_context.Entry(evaluation).Property(e => e.UserId).IsModified = false;
                    _context.Entry(evaluation).Property(e => e.ReservationId).IsModified = false;  
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EvaluationExists(evaluation.EvaluationId))
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
            //ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId", evaluation.ReservationId);
            return View(evaluation);
        }

        // GET: Evaluations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evaluation = await _context.Evaluation
                .Include(e => e.Reservation)
                .FirstOrDefaultAsync(m => m.EvaluationId == id);
            if (evaluation == null)
            {
                return NotFound();
            }

            return View(evaluation);
        }

        // POST: Evaluations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evaluation = await _context.Evaluation.FindAsync(id);
            _context.Evaluation.Remove(evaluation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EvaluationExists(int id)
        {
            return _context.Evaluation.Any(e => e.EvaluationId == id);
        }

        public async Task<IActionResult> ListComments(int? id) // id da propriedade , id da reserva para saber o id da avaliacao
        {
            if (id == null)
                return NotFound();

            var reserve = await _context.Reservations.Include(r => r.Property).Include(r=> r.ApplicationUser).Where(r => r.PropertyId == id).ToListAsync();
            if (reserve == null)
                return NotFound();

            var evaluationList = new List<Evaluation>();
            foreach (var item in reserve)
            {
                var evaluation = await _context.Evaluation.Include(r => r.Reservation).Where(r => r.Reservation == item).FirstOrDefaultAsync();
                if(evaluation != null)
                    evaluationList.Add(evaluation);

            }
            if (evaluationList != null)
            {
                return View(evaluationList);
            }

            return NotFound();
        }
    }
}
