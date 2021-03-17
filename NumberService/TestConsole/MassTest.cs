using Common.Helpers;
using Common.Models;
using Common.Requests;
using Common.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TestConsole
{
    public class MassTest
    {
        private const string BaseAPIURL = "http://localhost:51868/api/Nummern/";
        private readonly NumberserviceContext _context;

        public MassTest()
        {
            this._context = new NumberserviceContext();
        }

        public  async Task<MassTestResult> RunAsync()
        {
            MassTestResult massTestResult = new MassTestResult();
            List<NummerDefinition> nummerDefinitionen = new List<NummerDefinition>();
            List<ErstelleNummerInformation> ErstelleNummerInformationen = new List<ErstelleNummerInformation>();
            long countOfDefinitions = Random_Helper.GetLong(1L, 100L);
            massTestResult.CountOfDefinitions = countOfDefinitions;
            for (long i = 0; i < countOfDefinitions; i++)
            {
                NummerDefinition nummerDefinition = CreateRandomNummerDefinition();
                ErstellteNummerDefinition erstellteNummerDefinition = await ErstelleNummerDefinitionAsync(nummerDefinition);
                if (erstellteNummerDefinition != null)
                {
                    nummerDefinition.NummerDefinitionGuid = erstellteNummerDefinition.Guid;
                    nummerDefinition.NummerDefinitionId = erstellteNummerDefinition.Id;
                    nummerDefinitionen.Add(nummerDefinition);
                }
            }
            long countOfInformations = Random_Helper.GetLong(1L, 100L);
            massTestResult.CountOfInformations = countOfInformations;
            foreach (var nummerDefinition in nummerDefinitionen)
            {
                for (int i = 0; i < countOfInformations; i++)
                {
                    ErstelleNummerInformation erstelleNummerInformation = CreateRandomErstelleNummerInformation(nummerDefinition);
                    Guid? guid = await ErstelleNummerInformationAsync(erstelleNummerInformation);
                    if(guid.HasValue)
                    {
                        ErstelleNummerInformationen.Add(erstelleNummerInformation);
                        SetzeZielFürNummerInformation setzeZielFürNummerInformation = ErstelleSetzeZielFürNummerInformation(guid, nummerDefinition);
                        bool success = await SetzeZielFürNummerInformationAsync(setzeZielFürNummerInformation);
                        if(success)
                        {
                            MassTestMeasure massTestMeasure = new MassTestMeasure();
                            massTestMeasure.CountOfInformations = await this._context.NummerInformation.CountAsync();
                            massTestMeasure.Start = DateTime.Now;
                            NummerInformation nummerInformation = await HoleNummerInformationAsync(nummerDefinition, erstelleNummerInformation);
                            if(nummerInformation != null && nummerInformation.NummerInformationZiel.ToString() == setzeZielFürNummerInformation.Ziel.ToString())
                            {
                                massTestMeasure.End = DateTime.Now;
                                massTestResult.MassTestMeasures.Add(massTestMeasure);
                            }
                        }
                    }
                }
            }
            massTestResult.CountOfErstelleNummerInformationen = ErstelleNummerInformationen.Count;


            return massTestResult;

        }
        private static NummerDefinition CreateRandomNummerDefinition()
        {
            NummerDefinition nummerDefinition = new NummerDefinition();
            nummerDefinition.NummerDefinitionBezeichnung = Guid.NewGuid().ToString();
            nummerDefinition.NummerDefinitionQuelleBezeichnung = Guid.NewGuid().ToString();
            nummerDefinition.NummerDefinitionZielBezeichnung = Guid.NewGuid().ToString();
            nummerDefinition.NummerDefinitionZielDatentypId = (long)Random_Helper.GetLong((long)Datentyp.String, (long)Datentyp.Guid);

            ICollection<NummerDefinitionQuelle> nummerDefinitionQuellen = new List<NummerDefinitionQuelle>();
            long length = Random_Helper.GetLong(2L, 100L);
            for (long i = 1L; i < length; i++)
            {
                NummerDefinitionQuelle nummerDefinitionQuelle = new NummerDefinitionQuelle();
                nummerDefinitionQuelle.NummerDefinitionQuelleBezeichnung = Random_Helper.GetString(1,20,false);
                nummerDefinitionQuelle.NummerDefinitionQuelleDatentypId = (long)Random_Helper.GetLong((long)Datentyp.String, (long)Datentyp.Guid);
                nummerDefinition.NummerDefinitionQuellen.Add(nummerDefinitionQuelle);
            }

            return nummerDefinition;

        }
        private static async Task<ErstellteNummerDefinition> ErstelleNummerDefinitionAsync(NummerDefinition nummerDefinition)
        {
            ErstellteNummerDefinition erstellteNummerDefinition = null;

            if (erstellteNummerDefinition == null)
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(nummerDefinition), Encoding.UTF8, "application/json");
                    using (HttpResponseMessage response = await httpClient.PostAsync(BaseAPIURL + "ErstelleNummerDefinition/", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            erstellteNummerDefinition = JsonConvert.DeserializeObject<ErstellteNummerDefinition>(apiResponse);
                        }
                    }
                }
            }

            return erstellteNummerDefinition;
        }
        private static ErstelleNummerInformation CreateRandomErstelleNummerInformation(NummerDefinition nummerDefinition)
        {
            ErstelleNummerInformation erstelleNummerInformation = new ErstelleNummerInformation();
            if (nummerDefinition != null)
            {
                erstelleNummerInformation.Nummer_definition_id = nummerDefinition.NummerDefinitionId;
                int anzahlQuellen = nummerDefinition.NummerDefinitionQuellen.Count;
                object[] quellen = new object[anzahlQuellen];
                int index = 0;
                foreach (var NummerDefinitionQuelle in nummerDefinition.NummerDefinitionQuellen)
                {
                    object value = null;
                    switch ((Datentyp)NummerDefinitionQuelle.NummerDefinitionQuelleDatentypId)
                    {
                        case Datentyp.String:
                            value = Guid.NewGuid().ToString();
                            break;
                        case Datentyp.Integer:
                            value = Random_Helper.GetLong(0L, 100000L);
                            break;
                        case Datentyp.Guid:
                            value = Guid.NewGuid().ToString();
                            break;
                        default:
                            break;
                    }
                    quellen[index] = value;
                    index++;



                }
                erstelleNummerInformation.Quellen = quellen;

            }
            return erstelleNummerInformation;

        }
        private static async Task<Guid?> ErstelleNummerInformationAsync(ErstelleNummerInformation erstelleNummerInformation)
        {
            Guid? guid = null;



                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(erstelleNummerInformation), Encoding.UTF8, "application/json");
                    using (HttpResponseMessage response = await httpClient.PostAsync(BaseAPIURL + "ErstelleNummerInformation/", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            guid = JsonConvert.DeserializeObject<Guid>(apiResponse);
                        }
                    }
                }

            return guid;
        }
        private static SetzeZielFürNummerInformation ErstelleSetzeZielFürNummerInformation(Guid? guid, NummerDefinition nummerDefinition)
        {
            SetzeZielFürNummerInformation setzeZielFürNummerInformation = null;
            if (guid.HasValue && nummerDefinition != null)
            {
                object ziel = null;
                switch ((Datentyp)nummerDefinition.NummerDefinitionZielDatentypId)
                {
                    case Datentyp.String:
                        ziel = Guid.NewGuid().ToString();
                        break;
                    case Datentyp.Integer:
                        ziel = Random_Helper.GetLong(0L, 100000L);
                        break;
                    case Datentyp.Guid:
                        ziel = Guid.NewGuid();
                        break;
                    default:
                        break;
                }
                setzeZielFürNummerInformation = new SetzeZielFürNummerInformation();
                setzeZielFürNummerInformation.NummerInformationGuid = guid.Value;
                setzeZielFürNummerInformation.Ziel = ziel;

            }

            return setzeZielFürNummerInformation;

        }

        private static async Task<bool> SetzeZielFürNummerInformationAsync(SetzeZielFürNummerInformation setzeZielFürNummerInformation)
        {
            bool success = false;


            if (setzeZielFürNummerInformation != null)
            {




                using (var httpClient = new HttpClient())
                {

                    StringContent content = new StringContent(JsonConvert.SerializeObject(setzeZielFürNummerInformation), Encoding.UTF8, "application/json");
                    using (HttpResponseMessage response = await httpClient.PutAsync(BaseAPIURL + "SetzeZielFürNummerInformation/", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            NummerInformation nummerInformation = JsonConvert.DeserializeObject<NummerInformation>(apiResponse);
                            if (nummerInformation != null)
                            {
                                success = true;
                            }
                        }
                    }
                }

            }


            return success;
        }
        public async Task<NummerInformation> HoleNummerInformationAsync(NummerDefinition nummerDefinition, ErstelleNummerInformation erstelleNummerInformation)
        {
            NummerInformation nummerInformation = null;
            HoleNummerInformation holeNummerInformation = new HoleNummerInformation();
            holeNummerInformation.Nummer_definition_id = nummerDefinition.NummerDefinitionId;
            holeNummerInformation.DurchQuellen = true;
            holeNummerInformation.Quellen = erstelleNummerInformation.Quellen;
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(holeNummerInformation), Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = await httpClient.PostAsync(BaseAPIURL + "HoleNummerInformation/", content))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        nummerInformation = JsonConvert.DeserializeObject<NummerInformation>(apiResponse);
                    }
                }
            }
            return nummerInformation;

        }






    }

}