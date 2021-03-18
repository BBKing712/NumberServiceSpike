namespace Common.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public partial class Datentyp
    {
        public Datentyp()
        {
            this.NummerDefinitionen = new HashSet<NummerDefinition>();
            this.NummerDefinitionQuellen = new HashSet<NummerDefinitionQuelle>();
        }

        public long ID { get; set; }

        public string DatentypBezeichnung { get; set; }

        public virtual ICollection<NummerDefinition> NummerDefinitionen { get; set; }

        public virtual ICollection<NummerDefinitionQuelle> NummerDefinitionQuellen { get; set; }
    }
}
