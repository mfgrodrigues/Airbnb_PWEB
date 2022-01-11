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

namespace Airbnb_PWEB.Controllers
{
    public class EvaluationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EvaluationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Evaluations
        public async Task<IActionResult> Index()
        {
            List<MyEvaluationsViewModel> myEvaluations = new List<MyEvaluationsViewModel>();

            var evaluationList = await _context.Evaluation.Where(r => r.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)).Include(r => r.Reservation).ToListAsync();
            if (evaluationList != null)
            {
                foreach (var evaluation in evaluationList)
                {

                    MyEvaluationsViewModel evm = new MyEvaluationsViewModel
                    {
                        Property = await _context.Properties.Where(p => p.Id == evaluation.Reservation.PropertyId).FirstAsync(),
                        Reserve = evaluation.Reservation,
                        EvaluationComment = evaluation
                    };

                    myEvaluations.Add(evm); 

                }
                return View(myEvaluations);
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
        public IActionResult Create()
        {
            //ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId");
            return View();
        }

        // POST: Evaluations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EvaluationId,Comment,Classification,UserId,ReservationId")] Evaluation evaluation,int id)
        {
            evaluation.ReservationId = id;
            evaluation.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
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
        public async Task<IActionResult> Edit(int id, [Bind("EvaluationId,Comment,Classification,UserId, ReservationId")] Evaluation evaluation)
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
                    _context.Entry(evaluation).Property(e => e.UserId).IsModified = false;
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

        public async Task<IActionResult> ListComments(int id) // id da propriedade , id da reserva para saber o id da avaliacao
        {
            var lista = _context.Reservations.Where(r => r.PropertyId == id).ToList(); // ir à lista de reservas buscar todas as reservas que tem o id da propriedade 

            List<PropertyEvaluationViewModel> propertyEvaluations = new List<PropertyEvaluationViewModel>();
            
            if(lista != null)
            {
                foreach (var item in lista)
                {
                    var user = await _context.Users.Where(i => i.Id == item.ApplicationUser.Id).FirstOrDefaultAsync();
                    var comment = await _context.Evaluation.Where(i => i.ReservationId == item.ReservationId).FirstOrDefaultAsync();
                    if(comment != null && user != null)
                    {
                        PropertyEvaluationViewModel pevm = new PropertyEvaluationViewModel
                        {
                            EvaluationId = user.FirstName + " " + user.LastName,
                            Comment = comment
                        };
                        propertyEvaluations.Add(pevm);
                    }
                }
                    return View(propertyEvaluations);
            }
            else
            {
                return NotFound();  
            }
        }
    }
}
