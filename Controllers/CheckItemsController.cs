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
        public async Task<IActionResult> Edit(int id) // recebemos o id da reserva
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
            var checkItemsList = _context.CheckItems.Where(c => c.CheckList.Category.CategoryId == category.CategoryId).ToList();

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
        public async Task<IActionResult> Edit(List<int> checkValues)
        {
            if (checkValues.Count() == 0)
            {
                return NotFound();
            }
            var checkItemsList = new List<CheckItem>();
            foreach (var item in checkValues)
            {
                checkItemsList.Add( _context.CheckItems.Where(c => c.CheckItemId == item).FirstOrDefault());
            }
            // procurar na base de dados o primeiro item selecionado
            //var checkItem = _context.CheckItems.Include(c => c.CheckList).Where(c => c.CheckItemId == checkValues[0]).FirstOrDefault();

            // verificar a que checklist pertence
            //var checkList = _context.CheckList.Where(c => c.CheckListId == checkItem.CheckListId).FirstOrDefault();

            //if (checkList == null)
            //    return NotFound();

            // lista do items da checklist encontrada
            //var checkItemsList = _context.CheckItems.Where(c => c.CheckListId == checkList.CheckListId).ToList(); 

            // colocar a true os items selecionados
            //foreach(var item in checkItemsList){
            //    foreach(var itemChecked in checkValues)
            //    {
            //        if(item.CheckItemId == itemChecked)
            //            item.IsCheck = true;
            //    }
            //}

            Result results = new Result()
            {
                Itens = new List<CheckItem>()
            };

            foreach (var item in checkItemsList)
            {
                results.Itens.Add(item);
            }
            _context.Update(results);

            var reservation = _context.Reservations.Include(r => r.ResultEntry).Where(r => r.ReservationId == saveReservationId).FirstOrDefault();
            reservation.ResultEntry = results;

           
            _context.Update(reservation);

            _context.SaveChanges();

            return RedirectToAction("List", "Results", new { id = reservation.ResultEntry.ResultId });
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
