// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API.Models
{
    using System;

    public partial class NummerInformation
    {
        public long NummerInformationId { get; set; }
        public Guid NummerInformationGuid { get; set; }
        public long NummerDefinitionId { get; set; }
        public string NnmmerInformationQuelle { get; set; }
        public string NummerInformationZiel { get; set; }

        public virtual NummerDefinition NummerDefinition { get; set; }
    }
}
