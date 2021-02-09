using API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
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
            return await _context.Datentyp.ToListAsync();
        }
        // GET: api/Nummern/HoleNummerDefinitionen
        [HttpGet("HoleNummerDefinitionen")]
        public async Task<ActionResult<IEnumerable<NummerDefinition>>> HoleNummerDefinitionen()
        {
            var tmp = _context.NummerDefinition.Include(e => e.NummerDefinitionQuellen.ToList());
            
        

            return await _context.NummerDefinition.ToListAsync();
        }
        // POst: api/Nummern/ErstelleNummerDefinition
        [HttpPost("ErstelleNummerDefinition")]
        public async Task<ActionResult<long>> ErstelleNummerDefinition(NummerDefinition nummerDefinition)
        {
            NummerDefinition foundNummerDefinition = _context.NummerDefinition.Where(e => (e.NummerDefinitionBezeichnung == nummerDefinition.NummerDefinitionBezeichnung)).FirstOrDefault();
            if (foundNummerDefinition != null)
            {
                throw new Exception("der Wert für NummerDefinitionBezeichnung muss eindeutig sein.");
            }
            List<NummerDefinitionQuelle> nummerDefinitionQuelles = nummerDefinition.NummerDefinitionQuellen.ToList();
            nummerDefinition.NummerDefinitionQuellen.Clear();
            nummerDefinition.NummerDefinitionQuellen = null;
            nummerDefinition.NummerInformationen.Clear();
                nummerDefinition.NummerInformationen = null;

            _context.NummerDefinition.Add(nummerDefinition);
            await _context.SaveChangesAsync();


            int pos = 1;
            foreach (var nummerDefinitionQuelle in nummerDefinitionQuelles)
            {
                nummerDefinitionQuelle.NummerDefinitionQuellePos = pos;
                pos++;
                _context.NummerDefinitionQuelle.Add(nummerDefinitionQuelle);
                nummerDefinitionQuelle.NummerDefinitionId = nummerDefinition.NummerDefinitionId;

            }
            await _context.SaveChangesAsync();
            long id = nummerDefinition.NummerDefinitionId;
            return id;
        }

        //POst: api/Nummern/ErstelleNummerInformation
       [HttpPost("ErstelleNummerInformation")]
        public async Task<ActionResult<Guid?>> ErstelleNummerInformation(ErstelleNummerInformation erstelleNummerInformation)
        {
            Guid? guid = null;
            var tmp = await _context.NummerDefinition.Include(e => e.NummerDefinitionQuellen).ToListAsync();

            NummerDefinition foundNummerDefinition = _context.NummerDefinition.Where(e => (e.NummerDefinitionId == erstelleNummerInformation.nummer_definition_id)).FirstOrDefault();
            if(foundNummerDefinition == null)
            {
                throw new Exception(string.Format("für die nummer_definition_id = '{0}' existiert keine gültig Nummerdefinition.", erstelleNummerInformation.nummer_definition_id));
            }
            else if (foundNummerDefinition.NummerDefinitionQuellen == null || foundNummerDefinition.NummerDefinitionQuellen.Count == 0)
            {
                throw new Exception("Für die Nummerdefinition sind keine Quellen definiert.");
            }
            else if(foundNummerDefinition.NummerDefinitionQuellen.Count != erstelleNummerInformation.quellen.Count())
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
                string jsonQuellen = NumberInformationJSONGenerator.GenerateJSON(foundNummerDefinition.NummerDefinitionQuellen, erstelleNummerInformation.quellen);
                nummerInformation.NnmmerInformationQuelle = jsonQuellen ;
                nummerInformation.NummerInformationZiel = erstelleNummerInformation.ziel.ToString();

                _context.NummerInformation.Add(nummerInformation);
            await _context.SaveChangesAsync();

                guid = nummerInformation.NummerInformationGuid;

            }


            return guid;
        }
        // GET: api/Nummern//HoleNummerInformation/5
        [HttpGet("HoleNummerInformation/{id}")]
        public async Task<ActionResult<NummerInformation>> HoleNummerInformation(long id)
        {
            var nummerInformation = await _context.NummerInformation.FindAsync(id);

            if (nummerInformation == null)
            {
                return NotFound();
            }

            return nummerInformation;
        }
        //POst: api/Nummern/HoleNummerInformation
        [HttpPost("HoleNummerInformation")]
        public async Task<ActionResult<NummerInformation>> HoleNummerInformation(HoleNummerInformation holeNummerInformation)
        {
            NummerInformation nummerInformation = null;
            var tmp = await _context.NummerDefinition.Include(e => e.NummerDefinitionQuellen).ToListAsync();

            NummerDefinition foundNummerDefinition = _context.NummerDefinition.Where(e => (e.NummerDefinitionId == holeNummerInformation.nummer_definition_id)).FirstOrDefault();
            if (foundNummerDefinition == null)
            {
                throw new Exception(string.Format("für die nummer_definition_id = '{0}' existiert keine gültig Nummerdefinition.", holeNummerInformation.nummer_definition_id));
            }
            else if (foundNummerDefinition.NummerDefinitionQuellen == null || foundNummerDefinition.NummerDefinitionQuellen.Count == 0)
            {
                throw new Exception("Für die Nummerdefinition sind keine Quellen definiert.");
            }
            else if (foundNummerDefinition.NummerDefinitionQuellen.Count != holeNummerInformation.quellen.Count())
            {
                throw new Exception("Die Anzahl der definierten Quellen stimmt nicht mit der Anzahl der übergebenen Quellen überein.");
            }
            else
            {
                //List<NummerInformation> NummerInformations = _context.NummerInformations.Where(e => e.NummerDefinitionId == holeNummerInformation.nummer_definition_id).ToList();
                string rawSQL = NummerInformationRawSQLGenerator.GenersateRawSQL(holeNummerInformation.nummer_definition_id, foundNummerDefinition.NummerDefinitionQuellen, holeNummerInformation.quellen);
                List<NummerInformation> NummerInformations = await  _context.NummerInformation.FromSqlRaw(rawSQL).ToListAsync();
                if(NummerInformations !!= null && NummerInformations.Count > 0)
                {
                    nummerInformation = NummerInformations.FirstOrDefault();
                    return nummerInformation;
                }
                else
                {
                    return NotFound();
                }

            }


        }






    }
}
