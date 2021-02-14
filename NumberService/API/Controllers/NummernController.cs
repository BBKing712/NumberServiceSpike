namespace API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using API.Helpers;
    using Common.Models;
    using Common.Requests;
    using Common.Responses;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class NummernController : ControllerBase
    {
        private readonly NumberserviceContext _context;

        public NummernController(NumberserviceContext context)
        {
            this._context = context;
        }

        // GET: api/Nummern/HoleDatentypen
        [HttpGet("HoleDatentypen")]
        public async Task<ActionResult<IEnumerable<Datentyp>>> HoleDatentypen()
        {
            return await this._context.Datentyp.ToListAsync();
        }

        // GET: api/Nummern/HoleNummerDefinitionen
        [HttpGet("HoleNummerDefinitionen")]
        public async Task<ActionResult<IEnumerable<NummerDefinition>>> HoleNummerDefinitionen()
        {
            var tmp = this._context.NummerDefinition.Include(e => e.NummerDefinitionQuellen.ToList());

            return await this._context.NummerDefinition.ToListAsync();
        }
        // GET: api/HoleNummerDefinition/5
        [HttpGet("HoleNummerDefinition/{id}")]
        public async Task<ActionResult<NummerDefinition>> HoleNummerDefinition(long id)
        {
            var tmp = this._context.NummerDefinition.Include(e => e.NummerDefinitionQuellen.ToList());
            var nummerDefinition = await _context.NummerDefinition.FindAsync(id);

            if (nummerDefinition == null)
            {
                return NotFound();
            }

            return nummerDefinition;
        }
        // GET: api/HoleNummerDefinitionÜberBezeichnung/"xyz"
        [HttpGet("HoleNummerDefinitionÜberBezeichnung/{bezeichnung}")]
        public async Task<ActionResult<NummerDefinition>> HoleNummerDefinitionÜberBezeichnung(string  bezeichnung)
        {
            var tmp = this._context.NummerDefinition.Include(e => e.NummerDefinitionQuellen.ToList());
            var nummerDefinition = await _context.NummerDefinition.FindAsync(bezeichnung);

            if (nummerDefinition == null)
            {
                return NotFound();
            }

            return nummerDefinition;
        }



        // POst: api/Nummern/ErstelleNummerDefinition
        [HttpPost("ErstelleNummerDefinition")]
        public async Task<ActionResult<ErstellteNummerDefinition>> ErstelleNummerDefinition(NummerDefinition nummerDefinition)
        {
            NummerDefinition foundNummerDefinition = this._context.NummerDefinition.Where(e => (e.NummerDefinitionBezeichnung == nummerDefinition.NummerDefinitionBezeichnung)).FirstOrDefault();
            if (foundNummerDefinition != null)
            {
                throw new Exception("der Wert für NummerDefinitionBezeichnung muss eindeutig sein.");
            }

            var invalidgroups = nummerDefinition.NummerDefinitionQuellen.GroupBy(e => e.NummerDefinitionQuelleBezeichnung).Select(group => new
            {
                Key = group.Key,
                Count = group.Count(),
            }).Where(e => e.Count > 1);
            if (invalidgroups.Count() > 0)
            {
                throw new Exception("der Werte für NummerDefinitionQuelleBezeichnung müssen eindeutig sein.");
            }

            List<NummerDefinitionQuelle> nummerDefinitionQuelles = nummerDefinition.NummerDefinitionQuellen.ToList();
            nummerDefinition.NummerDefinitionQuellen.Clear();
            nummerDefinition.NummerDefinitionQuellen = null;
            nummerDefinition.NummerInformationen.Clear();
            nummerDefinition.NummerInformationen = null;

            this._context.NummerDefinition.Add(nummerDefinition);
            await this._context.SaveChangesAsync();

            int pos = 1;
            foreach (var nummerDefinitionQuelle in nummerDefinitionQuelles)
            {
                nummerDefinitionQuelle.NummerDefinitionQuellePos = pos;
                pos++;
                this._context.NummerDefinitionQuelle.Add(nummerDefinitionQuelle);
                nummerDefinitionQuelle.NummerDefinitionId = nummerDefinition.NummerDefinitionId;
            }

            await this._context.SaveChangesAsync();
            ErstellteNummerDefinition erstellteNummerDefinition = new ErstellteNummerDefinition();
            erstellteNummerDefinition.Id = nummerDefinition.NummerDefinitionId;
            erstellteNummerDefinition.Guid = nummerDefinition.NummerDefinitionGuid;
            erstellteNummerDefinition.Bezeichnung = nummerDefinition.NummerDefinitionBezeichnung;
            erstellteNummerDefinition.NummerDefinitionQuellen = nummerDefinition.NummerDefinitionQuellen.ToList();
            return erstellteNummerDefinition;
        }

        // POst: api/Nummern/ErstelleNummerInformation
        [HttpPost("ErstelleNummerInformation")]
        public async Task<ActionResult<Guid?>> ErstelleNummerInformation(ErstelleNummerInformation erstelleNummerInformation)
        {
            Guid? guid = null;
            var tmp = await this._context.NummerDefinition.Include(e => e.NummerDefinitionQuellen).ToListAsync();

            NummerDefinition foundNummerDefinition = this._context.NummerDefinition.Where(e => (e.NummerDefinitionId == erstelleNummerInformation.Nummer_definition_id)).FirstOrDefault();
            if (foundNummerDefinition == null)
            {
                throw new Exception(string.Format("für die nummer_definition_id = '{0}' existiert keine gültig Nummerdefinition.", erstelleNummerInformation.Nummer_definition_id));
            }
            else if (foundNummerDefinition.NummerDefinitionQuellen == null || foundNummerDefinition.NummerDefinitionQuellen.Count == 0)
            {
                throw new Exception("Für die Nummerdefinition sind keine Quellen definiert.");
            }
            else if (foundNummerDefinition.NummerDefinitionQuellen.Count != erstelleNummerInformation.Quellen.Count())
            {
                throw new Exception("Die Anzahl der definierten Quellen stimmt nicht mit der Anzahl der übergebenen Quellen überein.");
            }
            else if (erstelleNummerInformation.Ziel == null)
            {
                throw new Exception("Das Ziel ist null.");
            }
            else
            {
                NummerInformation nummerInformation = new NummerInformation();
                nummerInformation.NummerDefinitionId = erstelleNummerInformation.Nummer_definition_id;
                string jsonQuellen = NumberInformationJSONGenerator.GenerateJSON(foundNummerDefinition.NummerDefinitionQuellen, erstelleNummerInformation.Quellen);
                nummerInformation.NnmmerInformationQuelle = jsonQuellen;
                nummerInformation.NummerInformationZiel = erstelleNummerInformation.Ziel.ToString();

                this._context.NummerInformation.Add(nummerInformation);
                await this._context.SaveChangesAsync();

                guid = nummerInformation.NummerInformationGuid;
            }

            return guid;
        }

        // GET: api/Nummern//HoleNummerInformation/5
        [HttpGet("HoleNummerInformation/{id}")]
        public async Task<ActionResult<NummerInformation>> HoleNummerInformation(long id)
        {
            var nummerInformation = await this._context.NummerInformation.FindAsync(id);

            if (nummerInformation == null)
            {
                return this.NotFound();
            }

            return nummerInformation;
        }

        // POst: api/Nummern/HoleNummerInformation
        [HttpPost("HoleNummerInformation")]
        public async Task<ActionResult<NummerInformation>> HoleNummerInformation(HoleNummerInformation holeNummerInformation)
        {
            NummerInformation nummerInformation = null;
            var tmp = await this._context.NummerDefinition.Include(e => e.NummerDefinitionQuellen).ToListAsync();

            NummerDefinition foundNummerDefinition = this._context.NummerDefinition.Where(e => (e.NummerDefinitionId == holeNummerInformation.Nummer_definition_id)).FirstOrDefault();
            if (foundNummerDefinition == null)
            {
                throw new Exception(string.Format("für die nummer_definition_id = '{0}' existiert keine gültig Nummerdefinition.", holeNummerInformation.Nummer_definition_id));
            }
            else if (foundNummerDefinition.NummerDefinitionQuellen == null || foundNummerDefinition.NummerDefinitionQuellen.Count == 0)
            {
                throw new Exception("Für die Nummerdefinition sind keine Quellen definiert.");
            }
            else if (foundNummerDefinition.NummerDefinitionQuellen.Count != holeNummerInformation.Quellen.Count())
            {
                throw new Exception("Die Anzahl der definierten Quellen stimmt nicht mit der Anzahl der übergebenen Quellen überein.");
            }
            else
            {
                // List<NummerInformation> NummerInformations = _context.NummerInformations.Where(e => e.NummerDefinitionId == holeNummerInformation.nummer_definition_id).ToList();
                string rawSQL = NummerInformationRawSQLGenerator.GenersateRawSQL(holeNummerInformation.Nummer_definition_id, foundNummerDefinition.NummerDefinitionQuellen, holeNummerInformation.Quellen);
                List<NummerInformation> nummerInformations = await this._context.NummerInformation.FromSqlRaw(rawSQL).ToListAsync();
                if (nummerInformations != null && nummerInformations.Count > 0)
                {
                    nummerInformation = nummerInformations.FirstOrDefault();
                    return nummerInformation;
                }
                else
                {
                    return this.NotFound();
                }
            }
        }
    }
}
