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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Airbnb_PWEB.Controllers
{

    [Authorize(Roles = "Owner_Manager,Client,Owner_Employeer")]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

       
        public ReservationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reservations
        public async Task<IActionResult> Index(int? id) 
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (id == null)     // index para clientes
            {
                if (User.IsInRole("Client")) { 
                    if (currentUser == null)
                        return NotFound();

                    var reservationList2 = await _context.Reservations.Include(r => r.Property).Where(r => r.ApplicationUser == currentUser).ToListAsync();

                    if (reservationList2 == null)
                        return NotFound();

                    return View(reservationList2);
                }
                else if(User.IsInRole("Owner_Employeer")){
                    var mycompany = _context.Companies.Where(c => c.Employeers.Contains(currentUser)).FirstOrDefault();
                    if (mycompany == null)
                        return NotFound();
                    var resultList = _context.Reservations.Include(r => r.ResultExit.Itens)
                                                           .Include(r => r.ResultEntry.Itens).
                                                           Include(r=> r.ApplicationUser).
                                                           Include(r => r.Property).
                                                           Where(r => r.Property.Company == mycompany).ToList();
                    if (resultList == null)
                        return NotFound();

                    return View(resultList);
                }
            }
            // index para funcionarios e gestor quando tem uma propriedade associada

            var reservationList = await _context.Reservations.Include(r => r.ResultExit).Include(r => r.ResultEntry).Include(r => r.ApplicationUser).Include(r => r.Property).Where(r => r.PropertyId == id).ToListAsync();

            if (reservationList == null)
                return NotFound();

            return View(reservationList);
        }

        public async Task<IActionResult> Confirm(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var reservation = await _context.Reservations.FindAsync(id);
            reservation.Status = StatusReservation.Approved;
            if (ModelState.IsValid)
            {
                _context.Update(reservation);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index), new { id = reservation.PropertyId });
        }

        public async Task<IActionResult> Cancel(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var reservation = await _context.Reservations.FindAsync(id);
            reservation.Status = StatusReservation.Canceled;
            if (ModelState.IsValid)
            {
                _context.Update(reservation);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index), new { id = reservation.PropertyId });
        }

        public async Task<IActionResult> CheckedIn(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var reservation = await _context.Reservations.FindAsync(id);
            reservation.Status = StatusReservation.Checkin;
            if (ModelState.IsValid)
            {
                _context.Update(reservation);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index), new { id = reservation.PropertyId });
        }


        public async Task<IActionResult> CheckedOut(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var reservation = await _context.Reservations.FindAsync(id);
            reservation.Status = StatusReservation.Checkout;
            if (ModelState.IsValid)
            {
                _context.Update(reservation);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index), new { id = reservation.PropertyId });
        }

        public async Task<IActionResult> AddPhotos(int id, List<IFormFile> files)
        {
            var reservation = _context.Reservations.Include(r=> r.ApplicationUser)
                                                    .Include(r=> r.Property)
                                                    .Where(r => r.ReservationId == id).FirstOrDefault();
            
            reservation.ImagesReservation = new List<ReservationImage>();
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var image = new ReservationImage
                {
                    CreatedOn = DateTime.UtcNow,
                    FileType = file.ContentType,
                    Extension = extension,
                    Name = fileName,
                    ReservationId = reservation.ReservationId
                };
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    image.Data = dataStream.ToArray();
                }

                reservation.ImagesReservation.Add(image);

            }

            reservation.Status = StatusReservation.Checkout;
            _context.Update(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { id = reservation.PropertyId });
        }



        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.Include(p=>p.Property.Images)
                                                          .Include(p=> p.ImagesReservation)
                                                         .Include(p => p.ResultExit.Itens)
                                                         .Include(p => p.ResultEntry.Itens)
                                                         .Include(p => p.Property)
                                                         .FirstOrDefaultAsync(m => m.ReservationId == id);

            //var property = await _context.Properties.FirstOrDefaultAsync(p => p.Id == reservation.PropertyId);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        [Authorize(Roles = "Client")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Create([Bind("ReservationId,CheckIn,CheckOut,PropertyId,ApplicationUser,Status")] Reservation reservation, int id)
        {
            reservation.Status = StatusReservation.Pending;
            reservation.PropertyId = id;

            reservation.ApplicationUser = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                if (reservation.CheckIn < DateTime.Now)
                    TempData["AlertMessage"] = "Please enter a valid date";
                else if (reservation.CheckIn >= reservation.CheckOut)
                    ModelState.AddModelError("CheckIn", "Enter a check-in prior to check-out");
                else if (_context.Reservations.Where(r => r.PropertyId == reservation.PropertyId
                         && !(reservation.CheckIn.Date  < r.CheckIn.Date && reservation.CheckOut.Date < r.CheckIn.Date)
                         && !(reservation.CheckIn.Date  > r.CheckOut.Date && reservation.CheckOut.Date > r.CheckOut.Date)).Count() > 0)
                         TempData["AlertMessage"] = "The property is not available for reserve on the dates indicated";

                else {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                }
            }
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        [Authorize(Roles = "Owner_Employeer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
           

            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner_Employeer")]
        public async Task<IActionResult> Edit(int id, Reservation reservation)
        {
            var updateReservation = _context.Reservations.Find(id);
            if (updateReservation == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    updateReservation.ApprovalComment = reservation.ApprovalComment;
                    _context.Update(updateReservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(updateReservation.ReservationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = updateReservation.PropertyId });
            }
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.ReservationId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.ReservationId == id);
        }
    }
}
