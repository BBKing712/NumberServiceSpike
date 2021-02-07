using API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class NummernController
    {
        private readonly NumberserviceContext _context;

        public NummernController(NumberserviceContext context)
        {
            _context = context;
        }

        // GET: api/Nummern/HoleDatentypen
        [HttpGet("HoleDatentypen")]
        public async Task<ActionResult<IEnumerable<Datentyp>>> HoleDatentypen()
        {
            return await _context.Datentyps.ToListAsync();
        }
        // GET: api/Nummern/HoleNummerDefinitionen
        [HttpGet("HoleNummerDefinitionen")]
        public async Task<ActionResult<IEnumerable<NummerDefinition>>> HoleNummerDefinitionen()
        {
            var tmp = _context.NummerDefinitions.Include(e => e.NummerDefinitionQuelles);

            return await _context.NummerDefinitions.ToListAsync();
        }
        // POst: api/Nummern/ErstelleNummerDefinition
        [HttpPost("ErstelleNummerDefinition")]
        public async Task<ActionResult<long>> ErstelleNummerDefinition(NummerDefinition nummerDefinition)
        {
            //NummerDefinition nummerDefinition = new NummerDefinition();
            long id = nummerDefinition.NummerDefinitionId;
            return id;
        }



    }
}
