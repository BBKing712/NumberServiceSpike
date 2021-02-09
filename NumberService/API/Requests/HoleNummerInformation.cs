namespace API.Requests
{
    using System.Runtime.Serialization;

    public class HoleNummerInformation
    {
        [DataMember]
        public long nummer_definition_id { get; set; }

        [DataMember]
        public object[] quellen { get; set; }
    }
}
