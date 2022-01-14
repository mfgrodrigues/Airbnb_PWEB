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
    public class ResultsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResultsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Results
        public async Task<IActionResult> List(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            var result = await _context.Results.Include(i => i.Itens).Where(i => i.ResultId == id).FirstOrDefaultAsync();
            if(result == null)
                return NotFound();

            return View(result);
        }

        
    }
}
