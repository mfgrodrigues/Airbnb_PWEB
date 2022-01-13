using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Airbnb_PWEB.Data;
using Airbnb_PWEB.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Airbnb_PWEB.Controllers
{
    //[Authorize(Roles = "Owner_Employeer")]
    public class PropertiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public PropertiesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager; 

        }

        // GET: Properties
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (User.IsInRole("Owner_Manager"))
            {
                var company= _context.Companies.Where(c => c.Owner == currentUser).FirstOrDefault();
                var applicationDbContext = _context.Properties.Include(p => p.Images).Include(c => c.Category).Where(p => p.Company == company).ToListAsync();
                return View(await applicationDbContext);
            }
            else if (User.IsInRole("Owner_Employeer"))
            {
                var myCompany = _context.Companies.Where(c => c.Employeers.Contains(currentUser)).FirstOrDefault();
                var applicationDbContext = _context.Properties.Include(p => p.Images).Include(c => c.Category).Where(p => p.Company == myCompany).ToListAsync();
                return View(await applicationDbContext);

            }
            else {
                var applicationDbContext = _context.Properties.Include(p => p.Images).Include(c => c.Category);
                return View(await applicationDbContext.ToListAsync());
            }
        }
        // GET: Properties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @property = await _context.Properties
                .Include(p => p.Images)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        // GET: Properties/Create
        [Authorize(Roles = "Owner_Manager")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Properties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner_Manager")]
        public async Task<IActionResult> Create([Bind("CategoryId,Id,Tittle,Description,pricePerNigth,Address,City,Amenities,CompanyId")] Property @property, List<IFormFile> files)
        {
            var user = await _userManager.GetUserAsync(User);
            property.Company = _context.Companies.Where(c => c.Owner == user).FirstOrDefault(); // companhia do owner conectado no momento
            property.Images = new List<PropertyImage>();
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var image = new PropertyImage
                {
                    CreatedOn = DateTime.UtcNow,
                    FileType = file.ContentType,
                    Extension = extension,
                    Name = fileName
                };
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    image.Data = dataStream.ToArray();
                }

                property.Images.Add(image);
            }
            // TODO: Check this
            ModelState.Remove("Images");
            if (ModelState.IsValid)
            {
                _context.Add(@property);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", property.CategoryId);
            return View(@property);
        }

        // GET: Properties/Edit/5
        [Authorize(Roles = "Owner_Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @property = await _context.Properties.FindAsync(id);
            if (@property == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", property.CategoryId);
            return View(@property);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner_Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Id,Tittle,Description,pricePerNigth,Address,City,Amenities")] Property @property)
        {
            if (id != @property.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Images");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@property);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(@property.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", property.CategoryId);
            return View(@property);
        }

        // GET: Properties/Delete/5
        [Authorize(Roles = "Owner_Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @property = await _context.Properties
                .Include(p => p.Images)
                 .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner_Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @property = await _context.Properties.FindAsync(id);
            _context.Properties.Remove(@property);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyExists(int id)
        {
            return _context.Properties.Any(e => e.Id == id);
        }


        public async Task<IActionResult> Search(string text2search)
        {
            var applicationDbContext = _context.Properties.Include(p => p.Images);

            return View("Index", await applicationDbContext.Where(p => p.Tittle.ToLower().Contains(text2search.ToLower()) || 
                                                                  p.Description.ToLower().Contains(text2search.ToLower())|| 
                                                                  p.City.ToLower().Contains(text2search.ToLower())).ToListAsync());

        }
    }
}
