using Common.Helpers;
using Data.Models;
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
        private readonly NummernserviceContext _context;

        public MassTest()
        {
            this._context = new NummernserviceContext();
        }

        public  async Task<MassTestResult> RunAsync(long max)
        {
            MassTestResult massTestResult = new MassTestResult();
            List<NummerDefinition> nummerDefinitionen = new List<NummerDefinition>();
            List<ErstelleNummerInformationRequest> erstelleNummerInformationRequests = new List<ErstelleNummerInformationRequest>();
            long countOfDefinitions = Random_Helper.GetLong(1L, max);
            massTestResult.CountOfDefinitions = countOfDefinitions;
            for (long i = 0; i < countOfDefinitions; i++)
            {
                NummerDefinition nummerDefinition = CreateRandomNummerDefinition();
                ErstellteNummerDefinitionResponse ErstellteNummerDefinitionResponse = await ErstelleNummerDefinitionAsync(nummerDefinition);
                if (ErstellteNummerDefinitionResponse != null)
                {
                    nummerDefinition.Guid = ErstellteNummerDefinitionResponse.Guid;
                    nummerDefinition.Id = ErstellteNummerDefinitionResponse.Id;
                    nummerDefinitionen.Add(nummerDefinition);
                }
                else
                {
                    WriteRedTextToConsole("NummerDefinition konnte nicht erstellt werden.");
                }
            }
            long countOfInformations = Random_Helper.GetLong(1L, max);
            massTestResult.CountOfInformations = countOfInformations;
            foreach (var nummerDefinition in nummerDefinitionen)
            {
                for (int i = 0; i < countOfInformations; i++)
                {
                    ErstelleNummerInformationRequest erstelleNummerInformationRequest = CreateRandomErstelleNummerInformation(nummerDefinition);
                    Guid? guid = await ErstelleNummerInformationAsync(erstelleNummerInformationRequest);
                    if(guid.HasValue)
                    {
                        erstelleNummerInformationRequests.Add(erstelleNummerInformationRequest);
                        SetzeZielFürNummerInformationRequest setzeZielFürNummerInformationRequest = ErstelleSetzeZielFürNummerInformation(guid, nummerDefinition);
                        bool success = await SetzeZielFürNummerInformationAsync(setzeZielFürNummerInformationRequest);
                        if(success)
                        {
                            MassTestMeasure massTestMeasure = new MassTestMeasure();
                            massTestMeasure.CountOfInformations = await this._context.Nummerinformationen.CountAsync();
                            massTestMeasure.Start = DateTime.Now;
                            NummerInformation nummerInformation = await HoleNummerInformationAsync(nummerDefinition, erstelleNummerInformationRequest);
                            if(nummerInformation != null && nummerInformation.Ziel.ToString() == setzeZielFürNummerInformationRequest.Ziel.ToString())
                            {
                                massTestMeasure.End = DateTime.Now;
                                massTestMeasure.Milliseconds = (massTestMeasure.End - massTestMeasure.Start).TotalMilliseconds;
                                massTestResult.MassTestMeasures.Add(massTestMeasure);
                            }
                            else
                            {
                                WriteRedTextToConsole("NummerInformation konnte nicht geholt werden.");
                            }
                        }
                    }
                    else
                    {
                        WriteRedTextToConsole("NummerInformation konnte nicht erstellt werden.");
                    }
                }
            }
            massTestResult.CountOfErstelleNummerInformationen = erstelleNummerInformationRequests.Count;


            return massTestResult;

        }
        private static void WriteRedTextToConsole(string text)
        {
            ConsoleColor currentForeground = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = currentForeground;
        }
        private static NummerDefinition CreateRandomNummerDefinition()
        {
            NummerDefinition nummerDefinition = new NummerDefinition();
            nummerDefinition.Bezeichnung = Guid.NewGuid().ToString();
            nummerDefinition.QuelleBezeichnung = Guid.NewGuid().ToString();
            nummerDefinition.ZielBezeichnung = Guid.NewGuid().ToString();
            nummerDefinition.ZielDatentypenId = (long)Random_Helper.GetLong((long)Datentypwerte.String, (long)Datentypwerte.Guid);

            ICollection<NummerDefinitionQuelle> nummerDefinitionQuellen = new List<NummerDefinitionQuelle>();
            long length = Random_Helper.GetLong(2L, 100L);
            List<string> quellenBezeichnungen = new List<string>();
            for (long i = 1L; i < length; i++)
            {
                NummerDefinitionQuelle nummerDefinitionQuelle = new NummerDefinitionQuelle();
                string quelleBezeichnung = Random_Helper.GetString(1, 20, false);
                do
                {
                    quelleBezeichnung = Random_Helper.GetString(1, 20, false);
                } while (quellenBezeichnungen.Contains(quelleBezeichnung));

                nummerDefinitionQuelle.Bezeichnung = quelleBezeichnung;
                quellenBezeichnungen.Add(quelleBezeichnung);
                nummerDefinitionQuelle.DatentypenId = (long)Random_Helper.GetLong((long)Datentypwerte.String, (long)Datentypwerte.Guid);
                nummerDefinition.NummerDefinitionQuellen.Add(nummerDefinitionQuelle);
            }

            return nummerDefinition;

        }
        private static async Task<ErstellteNummerDefinitionResponse> ErstelleNummerDefinitionAsync(NummerDefinition nummerDefinition)
        {
            ErstellteNummerDefinitionResponse ErstellteNummerDefinitionResponse = null;

                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(nummerDefinition), Encoding.UTF8, "application/json");
                    using (HttpResponseMessage response = await httpClient.PostAsync(BaseAPIURL + "ErstelleNummerDefinition/", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            ErstellteNummerDefinitionResponse = JsonConvert.DeserializeObject<ErstellteNummerDefinitionResponse>(apiResponse);
                        }
                    }
                }
            return ErstellteNummerDefinitionResponse;
        }
        private static ErstelleNummerInformationRequest CreateRandomErstelleNummerInformation(NummerDefinition nummerDefinition)
        {
            ErstelleNummerInformationRequest erstelleNummerInformationRequest = new ErstelleNummerInformationRequest();
            if (nummerDefinition != null)
            {
                erstelleNummerInformationRequest.Nummer_definition_id = nummerDefinition.Id;
                int anzahlQuellen = nummerDefinition.NummerDefinitionQuellen.Count;
                object[] quellen = new object[anzahlQuellen];
                int index = 0;
                foreach (var NummerDefinitionQuelle in nummerDefinition.NummerDefinitionQuellen)
                {
                    object value = null;
                    switch ((Datentypwerte)NummerDefinitionQuelle.DatentypenId)
                    {
                        case Datentypwerte.String:
                            value = Guid.NewGuid().ToString();
                            break;
                        case Datentypwerte.Integer:
                            value = Random_Helper.GetLong(0L, 100000L);
                            break;
                        case Datentypwerte.Guid:
                            value = Guid.NewGuid().ToString();
                            break;
                        default:
                            break;
                    }
                    quellen[index] = value;
                    index++;



                }
                erstelleNummerInformationRequest.Quellen = quellen;

            }
            return erstelleNummerInformationRequest;

        }
        private static async Task<Guid?> ErstelleNummerInformationAsync(ErstelleNummerInformationRequest erstelleNummerInformationRequest)
        {
            Guid? guid = null;



                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(erstelleNummerInformationRequest), Encoding.UTF8, "application/json");
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
        private static SetzeZielFürNummerInformationRequest ErstelleSetzeZielFürNummerInformation(Guid? guid, NummerDefinition nummerDefinition)
        {
            SetzeZielFürNummerInformationRequest setzeZielFürNummerInformationRequest = null;
            if (guid.HasValue && nummerDefinition != null)
            {
                object ziel = null;
                switch ((Datentypwerte)nummerDefinition.ZielDatentypenId)
                {
                    case Datentypwerte.String:
                        ziel = Guid.NewGuid().ToString();
                        break;
                    case Datentypwerte.Integer:
                        ziel = Random_Helper.GetLong(0L, 100000L);
                        break;
                    case Datentypwerte.Guid:
                        ziel = Guid.NewGuid();
                        break;
                    default:
                        break;
                }
                setzeZielFürNummerInformationRequest = new SetzeZielFürNummerInformationRequest();
                setzeZielFürNummerInformationRequest.NummerInformationGuid = guid.Value;
                setzeZielFürNummerInformationRequest.Ziel = ziel;

            }

            return setzeZielFürNummerInformationRequest;

        }

        private static async Task<bool> SetzeZielFürNummerInformationAsync(SetzeZielFürNummerInformationRequest setzeZielFürNummerInformationRequest)
        {
            bool success = false;


            if (setzeZielFürNummerInformationRequest != null)
            {




                using (var httpClient = new HttpClient())
                {

                    StringContent content = new StringContent(JsonConvert.SerializeObject(setzeZielFürNummerInformationRequest), Encoding.UTF8, "application/json");
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
        public async Task<NummerInformation> HoleNummerInformationAsync(NummerDefinition nummerDefinition, ErstelleNummerInformationRequest erstelleNummerInformationRequest)
        {
            NummerInformation nummerInformation = null;
            HoleNummerInformationRequest holeNummerInformationRequest = new HoleNummerInformationRequest();
            holeNummerInformationRequest.Nummer_definition_id = nummerDefinition.Id;
            holeNummerInformationRequest.DurchQuellen = true;
            holeNummerInformationRequest.Quellen = erstelleNummerInformationRequest.Quellen;
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(holeNummerInformationRequest), Encoding.UTF8, "application/json");
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