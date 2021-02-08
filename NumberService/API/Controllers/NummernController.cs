using API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using API.Models;
using API.Requests;
using API.Helpers;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class NummernController : ControllerBase
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
            var tmp = _context.NummerDefinitions.Include(e => e.NummerDefinitionQuelles).ToList();
            
        

            return await _context.NummerDefinitions.ToListAsync();
        }
        // POst: api/Nummern/ErstelleNummerDefinition
        [HttpPost("ErstelleNummerDefinition")]
        public async Task<ActionResult<long>> ErstelleNummerDefinition(NummerDefinition nummerDefinition)
        {
            NummerDefinition foundNummerDefinition = _context.NummerDefinitions.Where(e => (e.NummerDefinitionBezeichnung == nummerDefinition.NummerDefinitionBezeichnung)).FirstOrDefault();
            if (foundNummerDefinition != null)
            {
                throw new Exception("der Wert für NummerDefinitionBezeichnung muss eindeutig sein.");
            }
            List<NummerDefinitionQuelle> nummerDefinitionQuelles = nummerDefinition.NummerDefinitionQuelles.ToList();
            nummerDefinition.NummerDefinitionQuelles.Clear();
            nummerDefinition.NummerDefinitionQuelles = null;
            nummerDefinition.NummerInformations.Clear();
                nummerDefinition.NummerInformations = null;

            _context.NummerDefinitions.Add(nummerDefinition);
            await _context.SaveChangesAsync();


            int pos = 1;
            foreach (var nummerDefinitionQuelle in nummerDefinitionQuelles)
            {
                nummerDefinitionQuelle.NummerDefinitionPos = pos;
                pos++;
                _context.NummerDefinitionQuelles.Add(nummerDefinitionQuelle);
                nummerDefinitionQuelle.NummerDefinitionId = nummerDefinition.NummerDefinitionId;

            }
            await _context.SaveChangesAsync();
            long id = nummerDefinition.NummerDefinitionId;
            return id;
        }

        //POst: api/Nummern/ErstelleNummerInformation
       [HttpPost("ErstelleNummerInformation")]
        public async Task<ActionResult<long>> ErstelleNummerInformation(ErstelleNummerInformation erstelleNummerInformation)
        {
            long id = 0;
            var tmp = await _context.NummerDefinitions.Include(e => e.NummerDefinitionQuelles).ToListAsync();

            NummerDefinition foundNummerDefinition = _context.NummerDefinitions.Where(e => (e.NummerDefinitionId == erstelleNummerInformation.nummer_definition_id)).FirstOrDefault();
            if(foundNummerDefinition == null)
            {
                throw new Exception(string.Format("für die nummer_definition_id = '{0}' existiert keine gültig Nummerdefinition.", erstelleNummerInformation.nummer_definition_id));
            }
            else if (foundNummerDefinition.NummerDefinitionQuelles == null || foundNummerDefinition.NummerDefinitionQuelles.Count == 0)
            {
                throw new Exception("Für die Nummerdefinition sind keine Quellen definiert.");
            }
            else if(foundNummerDefinition.NummerDefinitionQuelles.Count != erstelleNummerInformation.quellen.Count())
            {
                throw new Exception("Die Anzahl der definierten Quellen stimmt nicht mit der Anzahl der übergebenen Quellen überein.");
            }
            else if(erstelleNummerInformation.ziel == null)
            {
                throw new Exception("Das Ziel ist null.");
            }
            else
            {
                NummerInformation nummerInformation = new NummerInformation();
                nummerInformation.NummerDefinitionId = erstelleNummerInformation.nummer_definition_id;
                string jsonQuellen = NumberInformationJSONGenerator.GenerateJSON(foundNummerDefinition.NummerDefinitionQuelles, erstelleNummerInformation.quellen);
                nummerInformation.NnmmerInformationQuelle = jsonQuellen ;
                nummerInformation.NummerInformationZiel = erstelleNummerInformation.ziel.ToString();

                _context.NummerInformations.Add(nummerInformation);
            await _context.SaveChangesAsync();

                id = nummerInformation.NummerDefinitionId;

            }


            return id;
        }
        // GET: api/Nummern//HoleNummerInformation/5
        [HttpGet("HoleNummerInformation/{id}")]
        public async Task<ActionResult<NummerInformation>> HoleNummerInformation(long id)
        {
            var nummerInformation = await _context.NummerInformations.FindAsync(id);

            if (nummerInformation == null)
            {
                return NotFound();
            }

            return nummerInformation;
        }





    }
}
