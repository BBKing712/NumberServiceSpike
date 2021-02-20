using Common.Models;
using Common.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace TestConsole
{
    public sealed class StandardRequirement
    {
        private const string BaseAPIURL = "http://localhost:51868/api/Nummern/";
        private static volatile StandardRequirement _instance;
        private static object _syncRoot = new object();

        private StandardRequirement() { }

        public static StandardRequirement Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                            _instance = new StandardRequirement();
                    }
                }

                return _instance;
            }
        }

        private NummerDefinition _nummerDefinition;

        public NummerDefinition NummerDefinition
        {
            get { return _nummerDefinition; }
            set { _nummerDefinition = value; }
        }
        private ErstellteNummerDefinition _ErstellteNummerDefinition;

        public ErstellteNummerDefinition MyProperty
        {
            get { return _ErstellteNummerDefinition; }
            set { _ErstellteNummerDefinition = value; }
        }

        public  async Task PrüfeUndErstelleNummerDefinition()
        {
            string bezeichnung = NumberDefinitionBezeichnung.DEUWOAuftragsnummerZuGEMASAuftragsnummer.ToString();
            bool existiert = await StandardRequirement.Instance.PrüfeExistenzNummerDefinitionAsync(bezeichnung);
            if (!existiert)
            {
                ErstellteNummerDefinition erstellteNummerDefinition = await StandardRequirement.Instance.ErstelleNummerDefinitionAsync();
                existiert = await StandardRequirement.Instance.PrüfeExistenzNummerDefinitionAsync(bezeichnung);
            }


        }





        public async Task<bool> PrüfeExistenzNummerDefinitionAsync(string bezeichnung)
        {
            bool existiert = StandardRequirement.Instance.NummerDefinition != null;
            if(!existiert)
            {
                using (var httpClient = new HttpClient())
                {
                    using (HttpResponseMessage response = await httpClient.GetAsync(BaseAPIURL + "HoleNummerDefinitionÜberBezeichnung/" + bezeichnung))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            StandardRequirement.Instance.NummerDefinition = JsonConvert.DeserializeObject<NummerDefinition>(apiResponse);
                            existiert = StandardRequirement.Instance.NummerDefinition != null;
                            if (existiert)
                            {
                                StandardRequirement.Instance._ErstellteNummerDefinition = new ErstellteNummerDefinition();
                                StandardRequirement.Instance._ErstellteNummerDefinition.Id = StandardRequirement.Instance.NummerDefinition.NummerDefinitionId;
                                StandardRequirement.Instance._ErstellteNummerDefinition.Guid = StandardRequirement.Instance.NummerDefinition.NummerDefinitionGuid;
                                StandardRequirement.Instance._ErstellteNummerDefinition.Bezeichnung = StandardRequirement.Instance.NummerDefinition.NummerDefinitionBezeichnung;
                                StandardRequirement.Instance._ErstellteNummerDefinition.NummerDefinitionQuellen = StandardRequirement.Instance.NummerDefinition.NummerDefinitionQuellen.ToList();

                            }
                        }
                    }
                }
            }

            return existiert;


        }
        public  async Task<ErstellteNummerDefinition> ErstelleNummerDefinitionAsync()
        {
            ErstellteNummerDefinition erstellteNummerDefinition = StandardRequirement.Instance._ErstellteNummerDefinition;

            if(erstellteNummerDefinition == null)
            {
                using (var httpClient = new HttpClient())
                {
                    NummerDefinition nummerDefinition = LieferNummerDefinition();
                    StringContent content = new StringContent(JsonConvert.SerializeObject(nummerDefinition), Encoding.UTF8, "application/json");
                    using (HttpResponseMessage response = await httpClient.PostAsync(BaseAPIURL + "ErstelleNummerDefinition/", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            StandardRequirement.Instance._ErstellteNummerDefinition = JsonConvert.DeserializeObject<ErstellteNummerDefinition>(apiResponse);
                            erstellteNummerDefinition = StandardRequirement.Instance._ErstellteNummerDefinition;
                        }
                    }
                }
            }

            return erstellteNummerDefinition;
        }

        public static NummerDefinition LieferNummerDefinition()
        {
            NummerDefinition nummerDefinition = new NummerDefinition();
            nummerDefinition.NummerDefinitionBezeichnung = NumberDefinitionBezeichnung.DEUWOAuftragsnummerZuGEMASAuftragsnummer.ToString();
            nummerDefinition.NummerDefinitionQuelleBezeichnung = "DEUWOAuftragsnummer";
            nummerDefinition.NummerDefinitionZielBezeichnung = "GEMASAuftragsnummer";
            nummerDefinition.NummerDefinitionZielDatentypId = (long)Datentyp.Integer;

            //ICollection<NummerDefinitionQuelle> nummerDefinitionQuellen = new List<NummerDefinitionQuelle>();
            NummerDefinitionQuelle nummerDefinitionQuelle = new NummerDefinitionQuelle();
            nummerDefinitionQuelle.NummerDefinitionQuelleBezeichnung = "Auftragsnummer";
            nummerDefinitionQuelle.NummerDefinitionQuelleDatentypId = (long)Datentyp.Integer;
            nummerDefinition.NummerDefinitionQuellen.Add(nummerDefinitionQuelle);


            return nummerDefinition;
        }
    }
}
