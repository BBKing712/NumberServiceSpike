using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class NummerDefinitionQuelle
    {
        public long NummerDefinitionQuelleId { get; set; }
        public long NummerDefinitionId { get; set; }
        public long NummerDefinitionPos { get; set; }
        public long NummerDefinitionDatentypId { get; set; }
        public string NummerDefinitionBezeichnung { get; set; }

        public virtual Datentyp NummerDefinitionDatentyp { get; set; }
    }
}
