using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace API.Requests
{
    public class HoleNummerInformation
    {
        [DataMember]
        public long nummer_definition_id { get; set; }

        [DataMember]
        public object[] quellen { get; set; }

    }
}
