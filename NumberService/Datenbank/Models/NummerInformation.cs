using System;
using System.Collections.Generic;

#nullable disable

namespace Datenbank.Models
{
    public partial class NummerInformation
    {
        public long NummerInformationId { get; set; }
        public long NummerDefinitionId { get; set; }
        public string NnmmerInformationQuelle { get; set; }
        public string NummerInformationZiel { get; set; }

        public virtual NummerDefinition NummerDefinition { get; set; }
    }
}
