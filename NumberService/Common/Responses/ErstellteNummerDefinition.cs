namespace Common.Responses
{
    using System;
    using System.Collections.Generic;
    using Common.Models;

    public class ErstellteNummerDefinition
    {
        public ErstellteNummerDefinition()
        {
            this.NummerDefinitionQuellen = new List<NummerDefinitionQuelle>();
        }

        public long Id { get; set; }

        public Guid Guid { get; set; }

        public string Bezeichnung { get; set; }

        public ICollection<NummerDefinitionQuelle> NummerDefinitionQuellen { get; set; }
    }
}
