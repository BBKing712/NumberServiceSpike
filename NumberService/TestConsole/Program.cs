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


        static async Task  Main(string[] args)
        {

            bool existiert = await StandardRequirement.Instance.PrüfeUndErstelleNummerDefinition();
            if(existiert)
            {
                Guid? guid = await StandardRequirement.Instance.ErstelleNummerInformationAsync();
                var xxx = "xxx";
            }
        }


    }
}
