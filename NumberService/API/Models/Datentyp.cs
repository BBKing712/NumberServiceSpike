using System;
using System.Collections.Generic;

#nullable disable

namespace API.Models
{
    public partial class Datentyp
    {
        public Datentyp()
        {
            NummerDefinitionQuelles = new HashSet<NummerDefinitionQuelle>();
            NummerDefinitions = new HashSet<NummerDefinition>();
        }

        public long DatentypId { get; set; }
        public string DatentypBezeichnung { get; set; }

        public virtual ICollection<NummerDefinitionQuelle> NummerDefinitionQuelles { get; set; }
        public virtual ICollection<NummerDefinition> NummerDefinitions { get; set; }
    }
}
