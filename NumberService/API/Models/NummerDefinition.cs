using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API.Models
{
    public partial class NummerDefinition
    {
        public NummerDefinition()
        {
            NummerInformationen = new HashSet<NummerInformation>();
            NummerDefinitionQuellen = new HashSet<NummerDefinitionQuelle>();

        }

        public long NummerDefinitionId { get; set; }
        public string NummerDefinitionBezeichnung { get; set; }
        public string NummerDefinitionQuelleBezeichnung { get; set; }
        public long NummerDefinitionZielDatentypId { get; set; }
        public string NummerDefinitionZielBezeichnung { get; set; }

        public virtual Datentyp NummerDefinitionZielDatentyp { get; set; }
        public virtual ICollection<NummerInformation> NummerInformationen { get; set; }
        public ICollection<NummerDefinitionQuelle> NummerDefinitionQuellen { get; set; }


    }
}
