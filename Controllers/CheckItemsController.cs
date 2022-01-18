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
    public class CheckItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        private static int saveReservationId; 

        public CheckItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CheckItems
        public async Task<IActionResult> Index(int id)
        {
            var applicationDbContext = _context.CheckItems.Include(c => c.CheckList).Where(c => c.CheckList.CheckListId == id);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CheckItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkItem = await _context.CheckItems
                .Include(c => c.CheckList)
                .FirstOrDefaultAsync(m => m.CheckItemId == id);
            if (checkItem == null)
            {
                return NotFound();
            }

            return View(checkItem);
        }

        // GET: CheckItems/Create
        public IActionResult Create()
        {
            ViewData["CheckListId"] = new SelectList(_context.CheckList, "CheckListId", "CheckListId");
            return View();
        }

        // POST: CheckItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CheckItemId,Name,IsCheck,CheckListId")] CheckItem checkItem, int id)
        {
            if(id == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                checkItem.CheckListId = id;
                //checkItem.IsCheck = false;
                _context.Add(checkItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "CheckLists");
            }
            ViewData["CheckListId"] = new SelectList(_context.CheckList, "CheckListId", "CheckListId", checkItem.CheckListId);
            return View(checkItem);
        }

        // GET: CheckItems/Edit/5
        public async Task<IActionResult> CheckEdit(int id) // recebemos o id da reserva
        {
            if (id <= 0)
            {
                return NotFound();
            }

            saveReservationId = id;

            // reserva feita
            var reservation = _context.Reservations.Include(p => p.Property).Where(r => r.ReservationId == id).FirstOrDefault();

            if(reservation == null)
                return NotFound();  

            // categoria da propriedade reservada
            var category = _context.Categories.Where(c => c.CategoryId == reservation.Property.CategoryId).FirstOrDefault();

            if (category == null)
                return NotFound();

            // itens dachecklist da categoria a que pertence a propriedade
            var checkItemsList = _context.CheckItems.Where(c => c.CheckList.Category.CategoryId == category.CategoryId ).ToList();

            if (checkItemsList == null)
            {
                return NotFound();
            }

            //ViewData["CheckListId"] = new SelectList(_context.CheckList, "CheckListId", "CheckListId", checkItem.CheckListId);
            return View(checkItemsList);
        }

        // POST: CheckItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckEdit(List<int> checkValues)
        {
            if (checkValues.Count() == 0)
            {
                return NotFound();
            }
            var checkItemsList = new List<CheckItem>();
            foreach (var item in checkValues)
            {
                var copia = _context.CheckItems.Where(c => c.CheckItemId == item).FirstOrDefault();
                var newitem = new CheckItem() {
                    Name = copia.Name,
                    CheckListId = copia.CheckListId
                    
                };
                checkItemsList.Add(newitem);
            }

            Result results = new Result()
            {
                Itens = new List<CheckItem>()
            };

            foreach (var item in checkItemsList)
            {
                results.Itens.Add(item);
            }
            _context.Update(results);

            var reservation = _context.Reservations.Include(r => r.ResultExit).Include(r => r.ResultEntry).Where(r => r.ReservationId == saveReservationId).FirstOrDefault();
            if(reservation.ResultEntry != null)
                reservation.ResultExit = results;
            else
                reservation.ResultEntry = results;

           
            _context.Update(reservation);
            _context.SaveChanges();

            return RedirectToAction("Index", "Reservations", new { id = reservation.PropertyId});
        }

        public async Task<IActionResult> Edit(int ? id)
        {
                if (id == null)
                {
                    return NotFound();
                }

                var item = await _context.CheckItems.FindAsync(id);
                if (item == null)
                {
                    return NotFound();
                }
                return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,CheckItem item)
        {
            var item2 = _context.CheckItems.Where(c => c.CheckItemId == id).FirstOrDefault();
            if (id != item.CheckItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    item2.Name = item.Name;
                    _context.Update(item2);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckItemExists(item.CheckItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index),"CheckLists");
            }
            return View(item);
        }


        // GET: CheckItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkItem = await _context.CheckItems
                .Include(c => c.CheckList)
                .FirstOrDefaultAsync(m => m.CheckItemId == id);
            if (checkItem == null)
            {
                return NotFound();
            }

            return View(checkItem);
        }

        // POST: CheckItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var checkItem = await _context.CheckItems.FindAsync(id);
            _context.CheckItems.Remove(checkItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "CheckLists");
        }

        private bool CheckItemExists(int id)
        {
            return _context.CheckItems.Any(e => e.CheckItemId == id);
        }
    }
}
