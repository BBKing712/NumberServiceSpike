using Common.Helpers;
using Data.Models;
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


        static async Task  Main(string[] args)
        {

            bool existiert = await StandardRequirement.Instance.PrüfeUndErstelleNummerDefinitionAsync();
            if(existiert)
            {
                Guid? guid = await StandardRequirement.Instance.ErstelleNummerInformationAsync();
                if(guid.HasValue)
                {
                    bool success= await StandardRequirement.Instance.SetzeZielFürNummerInformationAsync();
                    if(success)
                    {
                        NummerInformation nummerInformation1 = await StandardRequirement.Instance.HoleNummerInformationAsync(true);
                        NummerInformation nummerInformation2 = await StandardRequirement.Instance.HoleNummerInformationAsync(false);
                    }
                }
            }
            long countOfMassTests = Random_Helper.GetLong(1L, 4000L);
            for (long i = 0; i < countOfMassTests; i++)
            {
                var aaa = await DoMassTestAsync();
            }

        }
        private static async Task<bool> DoMassTestAsync()
        {
            bool result = true;

            MassTest massTest = new MassTest();
            var aaa = "aaa";
            MassTestResult massTestResult = await massTest.RunAsync(40L);
            var xxx = massTestResult.MassTestMeasures.Count;
            var yyy = massTestResult.CountOfDefinitions * massTestResult.CountOfInformations;
            var bbb = "bbb";
            foreach (var item in massTestResult.MassTestMeasures)
            {
                Console.WriteLine($"{item.Milliseconds} Millsekunden bei {item.CountOfInformations} Einträgen.");
            }
            var ccc = "ccc";

            return result;
        }


    }
}
