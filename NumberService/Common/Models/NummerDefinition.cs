namespace Common.Models
{
    using System;
    using System.Collections.Generic;

    public partial class NummerDefinition
    {
        public NummerDefinition()
        {
            this.NummerInformationen = new HashSet<NummerInformation>();
            this.NummerDefinitionQuellen = new HashSet<NummerDefinitionQuelle>();
        }

        public long ID { get; set; }

        public Guid NummerDefinitionGuid { get; set; }

        public string NummerDefinitionBezeichnung { get; set; }

        public string NummerDefinitionQuelleBezeichnung { get; set; }

        public long NummerDefinitionZielDatentypId { get; set; }

        public string NummerDefinitionZielBezeichnung { get; set; }

        public virtual Datentyp NummerDefinitionZielDatentyp { get; set; }

        public virtual ICollection<NummerInformation> NummerInformationen { get; set; }

        public ICollection<NummerDefinitionQuelle> NummerDefinitionQuellen { get; set; }
    }
}
