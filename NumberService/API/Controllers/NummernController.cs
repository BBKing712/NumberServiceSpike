namespace API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
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

            return await this._context.NummerDefinition.Include("NummerDefinitionQuellen").ToListAsync();
        }
        // GET: api/HoleNummerDefinition/5
        [HttpGet("HoleNummerDefinition/{id}")]
        public async Task<ActionResult<NummerDefinition>> HoleNummerDefinition(long id)
        {
            var nummerDefinition = await _context.NummerDefinition.Include("NummerDefinitionQuellen").Where(e => e.NummerDefinitionId == id).FirstOrDefaultAsync();

            if (nummerDefinition == null)
            {
                return NotFound();
            }

            return nummerDefinition;
        }
        // GET: api/HoleNummerDefinitionÜberBezeichnung/xyz
        [HttpGet("HoleNummerDefinitionÜberBezeichnung/{bezeichnung}")]
        public async Task<ActionResult<NummerDefinition>> HoleNummerDefinitionÜberBezeichnung(string bezeichnung)
        {
            NummerDefinition nummerDefinition = await this._context.NummerDefinition.Include("NummerDefinitionQuellen").Where(e => (e.NummerDefinitionBezeichnung == bezeichnung)).FirstOrDefaultAsync();


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

            NummerDefinition foundNummerDefinition = this._context.NummerDefinition.Include("NummerDefinitionQuellen").Where(e => (e.NummerDefinitionId == erstelleNummerInformation.Nummer_definition_id)).FirstOrDefault();
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
            //else if (erstelleNummerInformation.Ziel == null)
            //{
            //    throw new Exception("Das Ziel ist null.");
            //}
            else
            {
                NummerInformation nummerInformation = new NummerInformation();
                nummerInformation.NummerDefinitionId = erstelleNummerInformation.Nummer_definition_id;
                string jsonQuellen = NumberInformationJSONGenerator.GenerateJSON(foundNummerDefinition.NummerDefinitionQuellen, erstelleNummerInformation.Quellen);
                nummerInformation.NnmmerInformationQuelle = jsonQuellen;
                nummerInformation.NummerInformationZiel = erstelleNummerInformation.Ziel != null ? erstelleNummerInformation.Ziel.ToString() : null;

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

            NummerDefinition foundNummerDefinition = this._context.NummerDefinition.Include("NummerDefinitionQuellen").Where(e => (e.NummerDefinitionId == holeNummerInformation.Nummer_definition_id)).FirstOrDefault();
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
        // Put: api/Nummern/SetzeZielFürNummerInformation
        [HttpPut("SetzeZielFürNummerInformation")]
        public async Task<ActionResult<NummerInformation>> SetzeZielFürNummerInformation(SetzeZielFürNummerInformation setzeZielFürNummerInformation)
        {
            NummerInformation nummerInformation = null;

            if (setzeZielFürNummerInformation == null)
            {
                throw new ArgumentNullException(nameof(setzeZielFürNummerInformation));
            }

            if (setzeZielFürNummerInformation.NummerInformationGuid == null)
            {
                throw new ArgumentNullException(nameof(setzeZielFürNummerInformation.NummerInformationGuid));
            }

            if (setzeZielFürNummerInformation.Ziel == null)
            {
                throw new ArgumentNullException(nameof(setzeZielFürNummerInformation.Ziel));
            }
            else
            {
                Guid guid = (Guid)setzeZielFürNummerInformation.NummerInformationGuid;
                nummerInformation = await this._context.NummerInformation.Where(e => (e.NummerInformationGuid != null && e.NummerInformationGuid.ToString() == guid.ToString())).FirstOrDefaultAsync();

                if (nummerInformation == null)
                {
                    return NotFound();

                }
                else
                {
                    string ziel = setzeZielFürNummerInformation.Ziel.ToString();
                        nummerInformation.NummerInformationZiel = ziel;
                        _context.Entry(nummerInformation).State = EntityState.Modified;
                        try
                        {
                            await this._context.SaveChangesAsync();
                            return Ok(nummerInformation);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            return StatusCode((int)HttpStatusCode.InternalServerError);
                        }





                }




            }

        }
    }
}
