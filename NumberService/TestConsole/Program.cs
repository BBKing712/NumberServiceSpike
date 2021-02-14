using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestConsole
{
    //see
    //https://www.yogihosting.com/aspnet-core-consume-api/#readid
    class Program
    {

        private const string BaseAPIURL = "http://localhost:51868/api/Nummern/";

        static async Task  Main(string[] args)
        {
            NummerDefinition nummerDefinition = null;
            string bezeichnung = NumberDefinitionBezeichnung.DEUWOAuftragsnummerZuGEMASAuftragsnummer.ToString();
            using (var httpClient = new HttpClient())
            {
                using (HttpResponseMessage response = await httpClient.GetAsync(BaseAPIURL + "HoleNummerDefinitionÜberBezeichnung/" + bezeichnung))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        nummerDefinition = JsonConvert.DeserializeObject<NummerDefinition>(apiResponse);
                    }

                }
            }


        }

        public static async Task PrüfeExistenzNummerDefinitionAsync(string bezeichnung)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(BaseAPIURL + "HoleNummerDefinitionÜberBezeichnung/" + bezeichnung))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    NummerDefinition nummerDefinition = JsonConvert.DeserializeObject<NummerDefinition>(apiResponse);
                }
            }


        }

        public static async Task TestHoleNummerDefinitionen()
        {
            List<NummerDefinition> nummerDefinitionList = new List<NummerDefinition>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(BaseAPIURL + "HoleNummerDefinitionen"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    nummerDefinitionList = JsonConvert.DeserializeObject<List<NummerDefinition>>(apiResponse);
                }
            }
        }

    }
}
