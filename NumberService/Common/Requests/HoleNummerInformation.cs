namespace Common.Requests
{
    using System.Runtime.Serialization;

    public class HoleNummerInformation
    {
        [DataMember]
        public long Nummer_definition_id { get; set; }

        [DataMember]
        public object[] Quellen { get; set; }
    }
}
