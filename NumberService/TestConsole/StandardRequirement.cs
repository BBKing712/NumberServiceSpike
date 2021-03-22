using Common.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Common.Requests;
using Data.Models;

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
        private ErstellteNummerDefinitionResponse _ErstellteNummerDefinitionResponse;

        public ErstellteNummerDefinitionResponse ErstellteNummerDefinitionResponse
        {
            get { return _ErstellteNummerDefinitionResponse; }
            set { _ErstellteNummerDefinitionResponse = value; }

        }
        private Guid? _BonsaiTransferGuid;

        public Guid? BonsaiTransferGuid
        {
            get { return _BonsaiTransferGuid; }
            set { _BonsaiTransferGuid = value; }
        }
        private long _DeuWoAuftragsnummer;

        public long DeuWoAuftragsnummer
        {
            get { return _DeuWoAuftragsnummer; }
            set { _DeuWoAuftragsnummer = value; }
        }
        private long _GemasAuftragsnummer;

        public long GemasAuftragsnummer
        {
            get { return _GemasAuftragsnummer; }
            set { _GemasAuftragsnummer = value; }
        }




        public async Task<bool> PrüfeUndErstelleNummerDefinitionAsync()
        {
            string bezeichnung = NumberDefinitionBezeichnung.DEUWOAuftragsnummerZuGEMASAuftragsnummer.ToString();
            bool existiert = await StandardRequirement.Instance.PrüfeExistenzNummerDefinitionAsync(bezeichnung);
            if (!existiert)
            {
                ErstellteNummerDefinitionResponse ErstellteNummerDefinitionResponse = await StandardRequirement.Instance.ErstelleNummerDefinitionAsync();
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
                    using (HttpResponseMessage response = await httpClient.GetAsync(BaseAPIURL + "HoleNummerDefinitionUeberBezeichnung/" + bezeichnung))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            StandardRequirement.Instance.NummerDefinition = JsonConvert.DeserializeObject<NummerDefinition>(apiResponse);
                            existiert = StandardRequirement.Instance.NummerDefinition != null;
                            if (existiert)
                            {
                                StandardRequirement.Instance.ErstellteNummerDefinitionResponse = new ErstellteNummerDefinitionResponse();
                                StandardRequirement.Instance.ErstellteNummerDefinitionResponse.Id = StandardRequirement.Instance.NummerDefinition.Id;
                                StandardRequirement.Instance.ErstellteNummerDefinitionResponse.Guid = StandardRequirement.Instance.NummerDefinition.Guid;
                                StandardRequirement.Instance.ErstellteNummerDefinitionResponse.Bezeichnung = StandardRequirement.Instance.NummerDefinition.Bezeichnung;
                                StandardRequirement.Instance.ErstellteNummerDefinitionResponse.NummerDefinitionQuellen = StandardRequirement.Instance.NummerDefinition.NummerDefinitionQuellen.ToList();

                            }
                        }
                    }
                }
            }

            return existiert;


        }
        public  async Task<ErstellteNummerDefinitionResponse> ErstelleNummerDefinitionAsync()
        {
            ErstellteNummerDefinitionResponse ErstellteNummerDefinitionResponse = StandardRequirement.Instance.ErstellteNummerDefinitionResponse;

            if(ErstellteNummerDefinitionResponse == null)
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
                            StandardRequirement.Instance.ErstellteNummerDefinitionResponse = JsonConvert.DeserializeObject<ErstellteNummerDefinitionResponse>(apiResponse);
                            ErstellteNummerDefinitionResponse = StandardRequirement.Instance.ErstellteNummerDefinitionResponse;
                        }
                    }
                }
            }

            return ErstellteNummerDefinitionResponse;
        }

        public static NummerDefinition LieferNummerDefinition()
        {
            NummerDefinition nummerDefinition = new NummerDefinition();
            nummerDefinition.Bezeichnung = NumberDefinitionBezeichnung.DEUWOAuftragsnummerZuGEMASAuftragsnummer.ToString();
            nummerDefinition.QuelleBezeichnung = "DEUWOAuftragsnummer";
            nummerDefinition.ZielBezeichnung = "GEMASAuftragsnummer";
            nummerDefinition.ZielDatentypenId = (long)Datentypwerte.Integer;

            //ICollection<NummerDefinitionQuelle> nummerDefinitionQuellen = new List<NummerDefinitionQuelle>();
            NummerDefinitionQuelle nummerDefinitionQuelle = new NummerDefinitionQuelle();
            nummerDefinitionQuelle.Bezeichnung = "Auftragsnummer";
            nummerDefinitionQuelle.DatentypenId = (long)Datentypwerte.Integer;
            nummerDefinition.NummerDefinitionQuellen.Add(nummerDefinitionQuelle);


            return nummerDefinition;
        }

        public async Task<Guid?> ErstelleNummerInformationAsync()
        {
            Guid? guid = StandardRequirement.Instance.BonsaiTransferGuid;


            if (!guid.HasValue && StandardRequirement.Instance.ErstellteNummerDefinitionResponse != null)
            {
                long deuWoauftragsnummer = Common.Helpers.Random_Helper.GetLong(0, 10000);

                using (var httpClient = new HttpClient())
                {
                    ErstelleNummerInformationRequest erstelleNummerInformationRequest = LieferErstelleNummerInformation(deuWoauftragsnummer);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(erstelleNummerInformationRequest), Encoding.UTF8, "application/json");
                    using (HttpResponseMessage response = await httpClient.PostAsync(BaseAPIURL + "ErstelleNummerInformation/", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            StandardRequirement.Instance.BonsaiTransferGuid = JsonConvert.DeserializeObject<Guid>(apiResponse);
                            if (StandardRequirement.Instance.BonsaiTransferGuid.HasValue)
                            {
                                guid = StandardRequirement.Instance.BonsaiTransferGuid;
                                StandardRequirement.Instance.DeuWoAuftragsnummer = deuWoauftragsnummer;
                            }
                        }
                    }
                }

            }
            return guid;
        }
        public static ErstelleNummerInformationRequest LieferErstelleNummerInformation(long auftragsnummer)
        {
            ErstelleNummerInformationRequest erstelleNummerInformationRequest = new ErstelleNummerInformationRequest();
            if (StandardRequirement.Instance.ErstellteNummerDefinitionResponse != null)
            {
                erstelleNummerInformationRequest.Nummer_definition_id = StandardRequirement.Instance.ErstellteNummerDefinitionResponse.Id;
                erstelleNummerInformationRequest.Quellen = new object[] { auftragsnummer };
            }
            return erstelleNummerInformationRequest;

        }
        public async Task<bool> SetzeZielFürNummerInformationAsync()
        {
            bool success = false;
            Guid? guid = StandardRequirement.Instance.BonsaiTransferGuid;


            if (guid.HasValue && StandardRequirement.Instance.ErstellteNummerDefinitionResponse != null)
            {
                long gemasauftragsnummer = Common.Helpers.Random_Helper.GetLong(0, 10000);

                using (var httpClient = new HttpClient())
                {
                    SetzeZielFürNummerInformationRequest setzeZielFürNummerInformationRequest = LieferSetzeZielFürNummerInformation(guid,gemasauftragsnummer);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(setzeZielFürNummerInformationRequest), Encoding.UTF8, "application/json");
                    using (HttpResponseMessage response = await httpClient.PutAsync(BaseAPIURL + "SetzeZielFürNummerInformation/", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            NummerInformation nummerInformation = JsonConvert.DeserializeObject<NummerInformation>(apiResponse);
                            if(nummerInformation != null && nummerInformation.Ziel == gemasauftragsnummer.ToString())
                            {
                                StandardRequirement.Instance.GemasAuftragsnummer = gemasauftragsnummer;
                                success = true;
                            }
                        }
                    }
                }

            }


            return success;
        }
        public static SetzeZielFürNummerInformationRequest LieferSetzeZielFürNummerInformation(Guid? nummerInformationGuid, long auftragsnummer)
        {


            SetzeZielFürNummerInformationRequest setzeZielFürNummerInformationRequest = new SetzeZielFürNummerInformationRequest();
            setzeZielFürNummerInformationRequest.NummerInformationGuid = nummerInformationGuid.Value;
            setzeZielFürNummerInformationRequest.Ziel = auftragsnummer;

            return setzeZielFürNummerInformationRequest;

        }
        public async Task<NummerInformation> HoleNummerInformationAsync(bool durchQuellen)
        {
            NummerInformation nummerInformation = null;
            HoleNummerInformationRequest holeNummerInformationRequest = new HoleNummerInformationRequest();
            holeNummerInformationRequest.Nummer_definition_id = StandardRequirement.Instance.NummerDefinition.Id;
            if (durchQuellen)
            {
                holeNummerInformationRequest.DurchQuellen = true;
                holeNummerInformationRequest.Quellen = new object[] { StandardRequirement.Instance.DeuWoAuftragsnummer };
            }
            else
            {
                holeNummerInformationRequest.DurchQuellen = false;
                holeNummerInformationRequest.Ziel = StandardRequirement.Instance.GemasAuftragsnummer;
            }
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
