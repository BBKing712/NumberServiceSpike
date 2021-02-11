namespace API.Requests
{
    using System.Runtime.Serialization;

    public class ErstelleNummerInformation
    {
        [DataMember]
        public long Nummer_definition_id { get; set; }

        [DataMember]
        public object[] Quellen { get; set; }

        [DataMember]
        public object Ziel { get; set; }
    }
}
