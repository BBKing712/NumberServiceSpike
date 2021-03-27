namespace API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using API.Helpers;
    using Common.Helpers;
    using Common.Requests;
    using Common.Responses;
    using Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class NummernController : ControllerBase
    {
        private readonly NummernserviceContext _context;

        public NummernController(NummernserviceContext context)
        {
            this._context = context;
        }

        // GET: api/Nummern/HoleDatentypen
        [HttpGet("HoleDatentypen")]
        public async Task<ActionResult<IEnumerable<Data.Models.Datentyp>>> HoleDatentypen()
        {
            return await this._context.Datentypen.ToListAsync();
        }

        // GET: api/Nummern/HoleNummerDefinitionen
        [HttpGet("HoleNummerDefinitionen")]
        public async Task<ActionResult<IEnumerable<Data.Models.NummerDefinition>>> HoleNummerDefinitionen()
        {

            return await this._context.Nummerdefinitionen.Include("NummerDefinitionQuellen").ToListAsync();
        }
        // GET: api/HoleNummerDefinition/5
        [HttpGet("HoleNummerDefinition/{id}")]
        public async Task<ActionResult<Data.Models.NummerDefinition>> HoleNummerDefinition(long id)
        {
            var nummerDefinition = await _context.Nummerdefinitionen.Include("NummerDefinitionQuellen").Where(e => e.Id == id).FirstOrDefaultAsync();

            if (nummerDefinition == null)
            {
                return NotFound();
            }

            return nummerDefinition;
        }
        // GET: api/HoleNummerDefinitionUeberBezeichnung/xyz
        [HttpGet("HoleNummerDefinitionUeberBezeichnung/{bezeichnung}")]
        public async Task<ActionResult<Data.Models.NummerDefinition>> HoleNummerDefinitionUeberBezeichnung(string bezeichnung)
        {
            Data.Models.NummerDefinition nummerDefinition = await this._context.Nummerdefinitionen.Include("NummerDefinitionQuellen").Where(e => (e.Bezeichnung == bezeichnung)).FirstOrDefaultAsync();


            if (nummerDefinition == null)
            {
                return NotFound();
            }

            return nummerDefinition;
        }



        // POst: api/Nummern/ErstelleNummerDefinition
        [HttpPost("ErstelleNummerDefinition")]
        public async Task<ActionResult<ErstellteNummerDefinitionResponse>> ErstelleNummerDefinition(Data.Models.NummerDefinition nummerDefinition)
        {
            Data.Models.NummerDefinition foundNummerDefinition = this._context.Nummerdefinitionen.Where(e => (e.Bezeichnung == nummerDefinition.Bezeichnung)).FirstOrDefault();
            if (foundNummerDefinition != null)
            {
                throw new Exception("der Wert für NummerDefinitionBezeichnung muss eindeutig sein.");
            }

            var invalidgroups = nummerDefinition.NummerDefinitionQuellen.GroupBy(e => e.Bezeichnung).Select(group => new
            {
                Key = group.Key,
                Count = group.Count(),
            }).Where(e => e.Count > 1);
            if (invalidgroups.Count() > 0)
            {
                throw new Exception("der Werte für NummerDefinitionQuelleBezeichnung müssen eindeutig sein.");
            }
            // TODO BK Bezeichnungen der Quellen müssen gültige JSON-Bezeichnungen sein(nicht mit Zahlen beginnen.)

            List<Data.Models.NummerDefinitionQuelle> nummerDefinitionQuelles = nummerDefinition.NummerDefinitionQuellen.ToList();
            nummerDefinition.NummerDefinitionQuellen.Clear();
            nummerDefinition.NummerDefinitionQuellen = null;
            nummerDefinition.NummerInformationen.Clear();
            nummerDefinition.NummerInformationen = null;

            this._context.Nummerdefinitionen.Add(nummerDefinition);
            await this._context.SaveChangesAsync();

            int pos = 1;
            foreach (var nummerDefinitionQuelle in nummerDefinitionQuelles)
            {
                nummerDefinitionQuelle.Position = pos;
                pos++;
                this._context.Nummerdefinitionquellen.Add(nummerDefinitionQuelle);
                nummerDefinitionQuelle.NummerdefinitionenId = nummerDefinition.Id;
            }

            await this._context.SaveChangesAsync();
            ErstellteNummerDefinitionResponse ErstellteNummerDefinitionResponse = new ErstellteNummerDefinitionResponse();
            ErstellteNummerDefinitionResponse.Id = nummerDefinition.Id;
            ErstellteNummerDefinitionResponse.Guid = nummerDefinition.Guid;
            ErstellteNummerDefinitionResponse.Bezeichnung = nummerDefinition.Bezeichnung;
            ErstellteNummerDefinitionResponse.NummerDefinitionQuellen = nummerDefinition.NummerDefinitionQuellen.ToList();
            return ErstellteNummerDefinitionResponse;
        }

        // POst: api/Nummern/ErstelleNummerInformation
        [HttpPost("ErstelleNummerInformation")]
        public async Task<ActionResult<Guid?>> ErstelleNummerInformation(ErstelleNummerInformationRequest erstelleNummerInformationRequest)
        {
            Guid? guid = null;

            NummerDefinition foundNummerDefinition = this._context.Nummerdefinitionen.Include("NummerDefinitionQuellen").Where(e => (e.Id == erstelleNummerInformationRequest.Nummer_definition_id)).FirstOrDefault();
            if (foundNummerDefinition == null)
            {
                throw new Exception(string.Format("für die nummer_definition_id = '{0}' existiert keine gültig Nummerdefinition.", erstelleNummerInformationRequest.Nummer_definition_id));
            }
            else if (foundNummerDefinition.NummerDefinitionQuellen == null || foundNummerDefinition.NummerDefinitionQuellen.Count == 0)
            {
                throw new Exception("Für die Nummerdefinition sind keine Quellen definiert.");
            }
            else if (foundNummerDefinition.NummerDefinitionQuellen.Count != erstelleNummerInformationRequest.Quellen.Count())
            {
                throw new Exception("Die Anzahl der definierten Quellen stimmt nicht mit der Anzahl der übergebenen Quellen überein.");
            }
            //else if (erstelleNummerInformation.Ziel == null)
            //{
            //    throw new Exception("Das Ziel ist null.");
            //}
            else
            {
                Data.Models.NummerInformation nummerInformation = new Data.Models.NummerInformation();
                nummerInformation.NummerdefinitionenId = erstelleNummerInformationRequest.Nummer_definition_id;
                string jsonQuellen = NumberInformationJSONGenerator.GenerateJSON(foundNummerDefinition.NummerDefinitionQuellen, erstelleNummerInformationRequest.Quellen);
                nummerInformation.Quelle = jsonQuellen;
                nummerInformation.Ziel = erstelleNummerInformationRequest.Ziel != null ? erstelleNummerInformationRequest.Ziel.ToString() : null;

                this._context.Nummerinformationen.Add(nummerInformation);
                await this._context.SaveChangesAsync();

                guid = nummerInformation.Guid;
            }

            return guid;
        }

        // GET: api/Nummern//HoleNummerInformation/5
        [HttpGet("HoleNummerInformation/{id}")]
        public async Task<ActionResult<Data.Models.NummerInformation>> HoleNummerInformation(long id)
        {
            var nummerInformation = await this._context.Nummerinformationen.FindAsync(id);

            if (nummerInformation == null)
            {
                return this.NotFound();
            }

            return nummerInformation;
        }

        // POst: api/Nummern/HoleNummerInformation
        [HttpPost("HoleNummerInformation")]
        public async Task<ActionResult<Data.Models.NummerInformation>> HoleNummerInformation(HoleNummerInformationRequest holeNummerInformationRequest)
        {
            Data.Models.NummerInformation nummerInformation = null;

            NummerDefinition foundNummerDefinition = this._context.Nummerdefinitionen.Include("NummerDefinitionQuellen").Where(e => (e.Id == holeNummerInformationRequest.Nummer_definition_id)).FirstOrDefault();
            if (foundNummerDefinition == null)
            {
                throw new Exception(string.Format("für die nummer_definition_id = '{0}' existiert keine gültig Nummerdefinition.", holeNummerInformationRequest.Nummer_definition_id));
            }
            else if (holeNummerInformationRequest.DurchQuellen && (foundNummerDefinition.NummerDefinitionQuellen == null || foundNummerDefinition.NummerDefinitionQuellen.Count == 0))
            {
                throw new Exception("Für die Nummerdefinition sind keine Quellen definiert.");
            }
            else if (holeNummerInformationRequest.DurchQuellen && (foundNummerDefinition.NummerDefinitionQuellen.Count != holeNummerInformationRequest.Quellen.Count()))
            {
                throw new Exception("Die Anzahl der definierten Quellen stimmt nicht mit der Anzahl der übergebenen Quellen überein.");
            }
            else if(!holeNummerInformationRequest.DurchQuellen && holeNummerInformationRequest.Ziel == null)
            {
                throw new ArgumentNullException(nameof(holeNummerInformationRequest.Ziel));
            }
            else
            {
                if(holeNummerInformationRequest.DurchQuellen)
                {
                    string rawSQL = NummerInformationRawSQLGenerator.GenersateRawSQL(holeNummerInformationRequest.Nummer_definition_id, foundNummerDefinition.NummerDefinitionQuellen, holeNummerInformationRequest.Quellen);
                    List<NummerInformation> nummerInformations = await this._context.Nummerinformationen.FromSqlRaw(rawSQL).ToListAsync();
                    if (nummerInformations != null && nummerInformations.Count > 0)
                    {
                        nummerInformation = nummerInformations.FirstOrDefault();
                    }
                    else
                    {
                        return this.NotFound();
                    }
                }
                else
                {
                    string ziel = holeNummerInformationRequest.Ziel.ToString();
                     nummerInformation = await this._context.Nummerinformationen.Where(e => e.NummerdefinitionenId == foundNummerDefinition.Id && e.Ziel == ziel).FirstOrDefaultAsync();

                }
                if (nummerInformation != null)
                {
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
        public async Task<ActionResult<Data.Models.NummerInformation>> SetzeZielFürNummerInformation(SetzeZielFürNummerInformationRequest setzeZielFürNummerInformationRequest)
        {
            Data.Models.NummerInformation nummerInformation = null;

            if (setzeZielFürNummerInformationRequest == null)
            {
                throw new ArgumentNullException(nameof(setzeZielFürNummerInformationRequest));
            }

            if (setzeZielFürNummerInformationRequest.NummerInformationGuid == null)
            {
                throw new ArgumentNullException(nameof(setzeZielFürNummerInformationRequest.NummerInformationGuid));
            }

            if (setzeZielFürNummerInformationRequest.Ziel == null)
            {
                throw new ArgumentNullException(nameof(setzeZielFürNummerInformationRequest.Ziel));
            }
            //TODO BK Ziel muss für jede Definition eindeutig sein.
            else
            {
                Guid guid = (Guid)setzeZielFürNummerInformationRequest.NummerInformationGuid;
                nummerInformation = await this._context.Nummerinformationen.Where(e => (e.Guid != null && e.Guid.ToString() == guid.ToString())).FirstOrDefaultAsync();

                if (nummerInformation == null)
                {
                    return NotFound();

                }
                else
                {
                    string ziel = setzeZielFürNummerInformationRequest.Ziel.ToString();
                        nummerInformation.Ziel = ziel;
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
        // GET: api/HoleNummerInformationZielUeberGuid/424B434E-0BB8-4839-B8C3-15F00A31F66E
        [HttpGet("HoleNummerInformationZielUeberGuid/{guid}")]
        public async Task<ActionResult<object>> HoleNummerInformationZielUeberGuid(Guid guid)
        {
            object ziel = null;

            NummerInformation nummerInformation = await this._context.Nummerinformationen.Where(e => (e.Guid == guid)).FirstOrDefaultAsync();


            if (nummerInformation != null && nummerInformation.Ziel != null)
            {
                NummerDefinition nummerDefinition = await this._context.Nummerdefinitionen.Where(e => e.Id == nummerInformation.NummerdefinitionenId).FirstOrDefaultAsync();
                if(nummerDefinition != null)
                {
                    switch (nummerDefinition.ZielDatentypenId)
                    {
                        case 1:

                            ziel = nummerInformation.Ziel;
                            break;
                        case 2:
                            Int32 output2 = 0;
                            if (Int32.TryParse(nummerInformation.Ziel, out output2))
                            {
                                ziel = output2;
                            }

                            break;
                        case 3:
                            Guid output3 = Guid.NewGuid();
                            if (Guid.TryParse(nummerInformation.Ziel, out output3))
                            {
                                ziel = output3;
                            }
                            break;
                        default:
                            break;
                    }

                }

            }
            if(ziel == null)
            {
                return NotFound();
            }
            else
            {
                return ziel;
            }
        }


    }
}
