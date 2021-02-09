// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API.Models
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
