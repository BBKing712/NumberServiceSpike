using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class NummerDefinition
    {
        public NummerDefinition()
        {
            NummerInformations = new HashSet<NummerInformation>();
            NummerDefinitionQuelles = new HashSet<NummerDefinitionQuelle>();
        }

        public long NummerDefinitionId { get; set; }
        public string NummerDefinitionBezeichnung { get; set; }
        public string NummerDefinitionQuelleBezeichnung { get; set; }
        public long NummerDefinitionZielDatentypId { get; set; }
        public string NummerDefinitionZielBezeichnung { get; set; }

        public virtual Datentyp NummerDefinitionZielDatentyp { get; set; }
        public virtual ICollection<NummerInformation> NummerInformations { get; set; }
        public virtual ICollection<NummerDefinitionQuelle> NummerDefinitionQuelles { get; set; }
    }
}
