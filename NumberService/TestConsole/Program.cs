using Common.Models;
using Common.Responses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    //see
    //https://www.yogihosting.com/aspnet-core-consume-api/#readid
    class Program
    {

        //private const string BaseAPIURL = "http://localhost:51868/api/Nummern/";

        static async Task  Main(string[] args)
        {
            //string bezeichnung = NumberDefinitionBezeichnung.DEUWOAuftragsnummerZuGEMASAuftragsnummer.ToString();
            //bool existiert = await StandardRequirement.Instance.PrüfeExistenzNummerDefinitionAsync(bezeichnung);
            //if (!existiert)
            //{
            //    ErstellteNummerDefinition erstellteNummerDefinition = await StandardRequirement.Instance.ErstelleNummerDefinitionAsync();
            //    existiert = await StandardRequirement.Instance.PrüfeExistenzNummerDefinitionAsync(bezeichnung);
            //}

            await StandardRequirement.Instance.PrüfeUndErstelleNummerDefinition();
        }

        //public static async Task<bool> PrüfeExistenzNummerDefinitionAsync(string bezeichnung)
        //{
        //    bool existiert = false;
        //    NummerDefinition nummerDefinition = null;
        //    using (var httpClient = new HttpClient())
        //    {
        //        using (HttpResponseMessage response = await httpClient.GetAsync(BaseAPIURL + "HoleNummerDefinitionÜberBezeichnung/" + bezeichnung))
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                string apiResponse = await response.Content.ReadAsStringAsync();
        //                nummerDefinition = JsonConvert.DeserializeObject<NummerDefinition>(apiResponse);
        //                existiert = nummerDefinition != null;
        //            }
        //        }
        //    }

        //    return existiert;
        //}
        //public static async Task<ErstellteNummerDefinition> ErstelleNummerDefinition()
        //{
        //    ErstellteNummerDefinition erstellteNummerDefinition = null;
        //    using (var httpClient = new HttpClient())
        //    {
        //        NummerDefinition nummerDefinition = LieferNummerDefinition();
        //        StringContent content = new StringContent(JsonConvert.SerializeObject(nummerDefinition), Encoding.UTF8, "application/json");
        //        using (HttpResponseMessage response = await httpClient.PostAsync(BaseAPIURL + "ErstelleNummerDefinition/", content))
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                string apiResponse = await response.Content.ReadAsStringAsync();
        //                erstellteNummerDefinition = JsonConvert.DeserializeObject<ErstellteNummerDefinition>(apiResponse);
        //            }
        //        }
        //    }

        //    return erstellteNummerDefinition;
        //}

        //public static NummerDefinition LieferNummerDefinition()
        //{
        //    NummerDefinition nummerDefinition = new NummerDefinition();
        //    nummerDefinition.NummerDefinitionBezeichnung = NumberDefinitionBezeichnung.DEUWOAuftragsnummerZuGEMASAuftragsnummer.ToString();
        //    nummerDefinition.NummerDefinitionQuelleBezeichnung = "DEUWOAuftragsnummer";
        //    nummerDefinition.NummerDefinitionZielBezeichnung = "GEMASAuftragsnummer";
        //    nummerDefinition.NummerDefinitionZielDatentypId = (long)Datentyp.Integer;

        //    //ICollection<NummerDefinitionQuelle> nummerDefinitionQuellen = new List<NummerDefinitionQuelle>();
        //    NummerDefinitionQuelle nummerDefinitionQuelle = new NummerDefinitionQuelle();
        //    nummerDefinitionQuelle.NummerDefinitionQuelleBezeichnung = "Auftragsnummer";
        //    nummerDefinitionQuelle.NummerDefinitionQuelleDatentypId = (long)Datentyp.Integer;
        //    nummerDefinition.NummerDefinitionQuellen.Add(nummerDefinitionQuelle);


        //    return nummerDefinition;
        //}

        //public static async Task TestHoleNummerDefinitionen()
        //{
        //    List<NummerDefinition> nummerDefinitionList = new List<NummerDefinition>();
        //    using (var httpClient = new HttpClient())
        //    {
        //        using (var response = await httpClient.GetAsync(BaseAPIURL + "HoleNummerDefinitionen"))
        //        {
        //            string apiResponse = await response.Content.ReadAsStringAsync();
        //            nummerDefinitionList = JsonConvert.DeserializeObject<List<NummerDefinition>>(apiResponse);
        //        }
        //    }
        //}

    }
}
