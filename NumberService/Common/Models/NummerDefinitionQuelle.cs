namespace Common.Models
{
    public partial class NummerDefinitionQuelle
    {
        public long NummerDefinitionQuelleId { get; set; }

        public long NummerDefinitionId { get; set; }

        public long NummerDefinitionQuellePos { get; set; }

        public long NummerDefinitionQuelleDatentypId { get; set; }

        public string NummerDefinitionQuelleBezeichnung { get; set; }

        public virtual Datentyp NummerDefinitionQuelleDatentyp { get; set; }
    }
}
