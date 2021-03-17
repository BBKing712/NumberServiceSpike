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
                    using (HttpResponseMessage response = await httpClient.GetAsync(BaseAPIURL + "HoleNummerDefinitionUeberBezeichnung/" + bezeichnung))
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
            Guid? guid = StandardRequirement.Instance.BonsaiTransferGuid;


            if (!guid.HasValue && StandardRequirement.Instance.ErstellteNummerDefinition != null)
            {
                long deuWoauftragsnummer = Common.Helpers.Random_Helper.GetLong(0, 10000);

                using (var httpClient = new HttpClient())
                {
                    ErstelleNummerInformation erstelleNummerInformation = LieferErstelleNummerInformation(deuWoauftragsnummer);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(erstelleNummerInformation), Encoding.UTF8, "application/json");
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
        public async Task<bool> SetzeZielFürNummerInformationAsync()
        {
            bool success = false;
            Guid? guid = StandardRequirement.Instance.BonsaiTransferGuid;


            if (guid.HasValue && StandardRequirement.Instance.ErstellteNummerDefinition != null)
            {
                long gemasauftragsnummer = Common.Helpers.Random_Helper.GetLong(0, 10000);

                using (var httpClient = new HttpClient())
                {
                    SetzeZielFürNummerInformation setzeZielFürNummerInformation = LieferSetzeZielFürNummerInformation(guid,gemasauftragsnummer);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(setzeZielFürNummerInformation), Encoding.UTF8, "application/json");
                    using (HttpResponseMessage response = await httpClient.PutAsync(BaseAPIURL + "SetzeZielFürNummerInformation/", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            NummerInformation nummerInformation = JsonConvert.DeserializeObject<NummerInformation>(apiResponse);
                            if(nummerInformation != null && nummerInformation.NummerInformationZiel == gemasauftragsnummer.ToString())
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
        public static SetzeZielFürNummerInformation LieferSetzeZielFürNummerInformation(Guid? nummerInformationGuid, long auftragsnummer)
        {


            SetzeZielFürNummerInformation setzeZielFürNummerInformation = new SetzeZielFürNummerInformation();
            setzeZielFürNummerInformation.NummerInformationGuid = nummerInformationGuid.Value;
            setzeZielFürNummerInformation.Ziel = auftragsnummer;

            return setzeZielFürNummerInformation;

        }
        public async Task<NummerInformation> HoleNummerInformationAsync(bool durchQuellen)
        {
            NummerInformation nummerInformation = null;
            HoleNummerInformation holeNummerInformation = new HoleNummerInformation();
            holeNummerInformation.Nummer_definition_id = StandardRequirement.Instance.NummerDefinition.NummerDefinitionId;
            if (durchQuellen)
            {
                holeNummerInformation.DurchQuellen = true;
                holeNummerInformation.Quellen = new object[] { StandardRequirement.Instance.DeuWoAuftragsnummer };
            }
            else
            {
                holeNummerInformation.DurchQuellen = false;
                holeNummerInformation.Ziel = StandardRequirement.Instance.GemasAuftragsnummer;
            }
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
