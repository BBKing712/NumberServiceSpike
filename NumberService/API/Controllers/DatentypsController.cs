using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatentypsController : Controller
    {
        private readonly NumberserviceContext _context;

        public DatentypsController(NumberserviceContext context)
        {
            _context = context;
        }
        // GET: api/Datentyps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Datentyp>>> GetDatentyps()
        {
            return await _context.Datentyps.ToListAsync();
        }


        // GET: Datentyps
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Datentyps.ToListAsync());
        //}

        // GET: Datentyps/Details/5
        //public async Task<IActionResult> Details(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var datentyp = await _context.Datentyps
        //        .FirstOrDefaultAsync(m => m.DatentypId == id);
        //    if (datentyp == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(datentyp);
        //}

        // GET: Datentyps/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: Datentyps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("DatentypId,DatentypBezeichnung")] Datentyp datentyp)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(datentyp);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(datentyp);
        //}

        // GET: Datentyps/Edit/5
        //public async Task<IActionResult> Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var datentyp = await _context.Datentyps.FindAsync(id);
        //    if (datentyp == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(datentyp);
        //}

        // POST: Datentyps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(long id, [Bind("DatentypId,DatentypBezeichnung")] Datentyp datentyp)
        //{
        //    if (id != datentyp.DatentypId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(datentyp);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!DatentypExists(datentyp.DatentypId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(datentyp);
        //}

        // GET: Datentyps/Delete/5
        //public async Task<IActionResult> Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var datentyp = await _context.Datentyps
        //        .FirstOrDefaultAsync(m => m.DatentypId == id);
        //    if (datentyp == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(datentyp);
        //}

        // POST: Datentyps/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(long id)
        //{
        //    var datentyp = await _context.Datentyps.FindAsync(id);
        //    _context.Datentyps.Remove(datentyp);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool DatentypExists(long id)
        //{
        //    return _context.Datentyps.Any(e => e.DatentypId == id);
        //}
    }
}
