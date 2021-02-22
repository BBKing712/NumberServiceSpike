using Common.Models;
using Common.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Common.Requests;

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

        public ErstellteNummerDefinition ErstellteNummerDefinition
        {
            get { return _ErstellteNummerDefinition; }
            set { _ErstellteNummerDefinition = value; }

        }
        private Guid? _NummerInformationGuid;

        public Guid? NummerInformationGuid
        {
            get { return _NummerInformationGuid; }
            set { _NummerInformationGuid = value; }
        }
        private long _DeuWoAuftragsnummer;

        public long DeuWoAuftragsnummer
        {
            get { return _DeuWoAuftragsnummer; }
            set { _DeuWoAuftragsnummer = value; }
        }




        public  async Task<bool> PrüfeUndErstelleNummerDefinition()
        {
            string bezeichnung = NumberDefinitionBezeichnung.DEUWOAuftragsnummerZuGEMASAuftragsnummer.ToString();
            bool existiert = await StandardRequirement.Instance.PrüfeExistenzNummerDefinitionAsync(bezeichnung);
            if (!existiert)
            {
                ErstellteNummerDefinition erstellteNummerDefinition = await StandardRequirement.Instance.ErstelleNummerDefinitionAsync();
                existiert = await StandardRequirement.Instance.PrüfeExistenzNummerDefinitionAsync(bezeichnung);
            }
            return existiert;


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
                                StandardRequirement.Instance.ErstellteNummerDefinition = new ErstellteNummerDefinition();
                                StandardRequirement.Instance.ErstellteNummerDefinition.Id = StandardRequirement.Instance.NummerDefinition.NummerDefinitionId;
                                StandardRequirement.Instance.ErstellteNummerDefinition.Guid = StandardRequirement.Instance.NummerDefinition.NummerDefinitionGuid;
                                StandardRequirement.Instance.ErstellteNummerDefinition.Bezeichnung = StandardRequirement.Instance.NummerDefinition.NummerDefinitionBezeichnung;
                                StandardRequirement.Instance.ErstellteNummerDefinition.NummerDefinitionQuellen = StandardRequirement.Instance.NummerDefinition.NummerDefinitionQuellen.ToList();

                            }
                        }
                    }
                }
            }

            return existiert;


        }
        public  async Task<ErstellteNummerDefinition> ErstelleNummerDefinitionAsync()
        {
            ErstellteNummerDefinition erstellteNummerDefinition = StandardRequirement.Instance.ErstellteNummerDefinition;

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
                            StandardRequirement.Instance.ErstellteNummerDefinition = JsonConvert.DeserializeObject<ErstellteNummerDefinition>(apiResponse);
                            erstellteNummerDefinition = StandardRequirement.Instance.ErstellteNummerDefinition;
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

        public async Task<Guid?> ErstelleNummerInformationAsync()
        {
            Guid? guid = StandardRequirement.Instance.NummerInformationGuid;


            if (!guid.HasValue && StandardRequirement.Instance.ErstellteNummerDefinition != null)
            {
                long auftragsnummer = Common.Helpers.Random_Helper.GetLong(0, 10000);

                using (var httpClient = new HttpClient())
                {
                    ErstelleNummerInformation erstelleNummerInformation = LieferErstelleNummerInformation(auftragsnummer);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(erstelleNummerInformation), Encoding.UTF8, "application/json");
                    using (HttpResponseMessage response = await httpClient.PostAsync(BaseAPIURL + "ErstelleNummerInformation/", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            StandardRequirement.Instance.NummerInformationGuid = JsonConvert.DeserializeObject<Guid>(apiResponse);
                            if (StandardRequirement.Instance.NummerInformationGuid.HasValue)
                            {
                                guid = StandardRequirement.Instance.NummerInformationGuid;
                                StandardRequirement.Instance.DeuWoAuftragsnummer = auftragsnummer;
                            }
                        }
                    }
                }

            }
            return guid;
        }
        public static ErstelleNummerInformation LieferErstelleNummerInformation(long auftragsnummer)
        {
            ErstelleNummerInformation erstelleNummerInformation = new ErstelleNummerInformation();
            if (StandardRequirement.Instance.ErstellteNummerDefinition != null)
            {
                erstelleNummerInformation.Nummer_definition_id = StandardRequirement.Instance.ErstellteNummerDefinition.Id;
                erstelleNummerInformation.Quellen = new object[] { auftragsnummer };
            }
            return erstelleNummerInformation;

        }

    }
}
