using API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatentypController : ControllerBase
    {
        private readonly NumberserviceContext _context;

        public DatentypController(NumberserviceContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            _context = context;
        }

        // GET: api/Datentyp
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Datentyp>>> GetDatentypen()
        //{
        //    return await _context.Datentyps.ToListAsync();

        //}
        public IEnumerable<Datentyp> Get()
        {

            List<Datentyp> resultList = _context.Datentyps.ToList();

            //return await _context.Datentyps.ToListAsync();
            return resultList;

        }

    }
}
