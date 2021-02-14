using Common.Models;
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
        static async Task Main(string[] args)
        {
            List<NummerDefinition> nummerDefinitionList = new List<NummerDefinition>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:51868/api/Nummern/HoleNummerDefinitionen"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    nummerDefinitionList = JsonConvert.DeserializeObject<List<NummerDefinition>>(apiResponse);
                }
            }
        }
    }
}
