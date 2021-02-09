
// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace API.Models
{
    using System.Collections.Generic;

    public partial class Datentyp
    {
        public Datentyp()
        {
            NummerDefinitionen = new HashSet<NummerDefinition>();
            NummerDefinitionQuellen = new HashSet<NummerDefinitionQuelle>();
        }

        public long DatentypId { get; set; }
        public string DatentypBezeichnung { get; set; }

        public virtual ICollection<NummerDefinition> NummerDefinitionen { get; set; }
        public virtual ICollection<NummerDefinitionQuelle> NummerDefinitionQuellen { get; set; }
    }
}
