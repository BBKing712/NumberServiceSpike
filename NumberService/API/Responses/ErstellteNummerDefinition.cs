using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Responses
{
    public class ErstellteNummerDefinition
    {

        public ErstellteNummerDefinition()
        {
            NummerDefinitionQuellen = new List<NummerDefinitionQuelle>();
        }


        public long Id { get; set; }

        public Guid Guid { get; set; }

        public string Bezeichnung { get; set; }

        public ICollection<NummerDefinitionQuelle> NummerDefinitionQuellen { get; set; }
    }
}
