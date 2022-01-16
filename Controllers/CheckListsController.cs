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
    public class CheckListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CheckLists
        public async Task<IActionResult> Index()
        {
            return View(await _context.CheckList.Include(c => c.Category).ToListAsync());
        }

        // GET: CheckLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkList = await _context.CheckList
                .FirstOrDefaultAsync(m => m.CheckListId == id);
            if (checkList == null)
            {
                return NotFound();
            }

            return View(checkList);
        }


        public async Task<IActionResult> Create([Bind("CheckListId, Category")] CheckList checkList, int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var checkListAux = await _context.CheckList.Include(c=>c.Category).Where(c => c.Category.CategoryId == id).FirstOrDefaultAsync();
            if (checkListAux != null)// ja existe uma criada 
            {
                TempData["AlertMessage"] = "CheckList already exists for Category "+ checkListAux.Category.Name;
                return RedirectToAction(nameof(Index),"Categories");
            }
            var category = await _context.Categories.Where(c => c.CategoryId == id).FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                checkList.Category = category;
                _context.Add(checkList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(checkList);
        }

        // GET: CheckLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkList = await _context.CheckList.FindAsync(id);
            if (checkList == null)
            {
                return NotFound();
            }
            return View(checkList);
        }

        // POST: CheckLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CheckListId")] CheckList checkList)
        {
            if (id != checkList.CheckListId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(checkList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckListExists(checkList.CheckListId))
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
            return View(checkList);
        }

        // GET: CheckLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkList = await _context.CheckList.FindAsync(id);
            if (checkList == null)
            {
                return NotFound();
            }
            _context.CheckList.Remove(checkList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheckListExists(int id)
        {
            return _context.CheckList.Any(e => e.CheckListId == id);
        }
    }
}
