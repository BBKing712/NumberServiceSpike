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
    public class NummerDefinitionsController : Controller
    {
        private readonly NumberserviceContext _context;

        public NummerDefinitionsController(NumberserviceContext context)
        {
            _context = context;
        }
        // GET: api/NummerDefinitions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NummerDefinition>>> GetNummerDefinitions()
                    {
            var xxx = _context.NummerDefinitions.Include(e => e.NummerDefinitionQuelles);

            return await _context.NummerDefinitions.ToListAsync();
        }

        // GET: NummerDefinitions/Details/5
        //public async Task<IActionResult> Details(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var nummerDefinition = await _context.NummerDefinitions
        //        .Include(n => n.NummerDefinitionZielDatentyp)
        //        .FirstOrDefaultAsync(m => m.NummerDefinitionId == id);
        //    if (nummerDefinition == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(nummerDefinition);
        //}

        // GET: NummerDefinitions/Create
        //public IActionResult Create()
        //{
        //    ViewData["NummerDefinitionZielDatentypId"] = new SelectList(_context.Datentyps, "DatentypId", "DatentypBezeichnung");
        //    return View();
        //}

        // POST: NummerDefinitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("NummerDefinitionId,NummerDefinitionBezeichnung,NummerDefinitionQuelleBezeichnung,NummerDefinitionZielDatentypId,NummerDefinitionZielBezeichnung")] NummerDefinition nummerDefinition)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(nummerDefinition);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["NummerDefinitionZielDatentypId"] = new SelectList(_context.Datentyps, "DatentypId", "DatentypBezeichnung", nummerDefinition.NummerDefinitionZielDatentypId);
        //    return View(nummerDefinition);
        //}

        // GET: NummerDefinitions/Edit/5
        //public async Task<IActionResult> Edit(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var nummerDefinition = await _context.NummerDefinitions.FindAsync(id);
        //    if (nummerDefinition == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["NummerDefinitionZielDatentypId"] = new SelectList(_context.Datentyps, "DatentypId", "DatentypBezeichnung", nummerDefinition.NummerDefinitionZielDatentypId);
        //    return View(nummerDefinition);
        //}

        // POST: NummerDefinitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(long id, [Bind("NummerDefinitionId,NummerDefinitionBezeichnung,NummerDefinitionQuelleBezeichnung,NummerDefinitionZielDatentypId,NummerDefinitionZielBezeichnung")] NummerDefinition nummerDefinition)
        //{
        //    if (id != nummerDefinition.NummerDefinitionId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(nummerDefinition);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!NummerDefinitionExists(nummerDefinition.NummerDefinitionId))
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
        //    ViewData["NummerDefinitionZielDatentypId"] = new SelectList(_context.Datentyps, "DatentypId", "DatentypBezeichnung", nummerDefinition.NummerDefinitionZielDatentypId);
        //    return View(nummerDefinition);
        //}

        // GET: NummerDefinitions/Delete/5
        //public async Task<IActionResult> Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var nummerDefinition = await _context.NummerDefinitions
        //        .Include(n => n.NummerDefinitionZielDatentyp)
        //        .FirstOrDefaultAsync(m => m.NummerDefinitionId == id);
        //    if (nummerDefinition == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(nummerDefinition);
        //}

        // POST: NummerDefinitions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(long id)
        //{
        //    var nummerDefinition = await _context.NummerDefinitions.FindAsync(id);
        //    _context.NummerDefinitions.Remove(nummerDefinition);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool NummerDefinitionExists(long id)
        //{
        //    return _context.NummerDefinitions.Any(e => e.NummerDefinitionId == id);
        //}
    }
}
