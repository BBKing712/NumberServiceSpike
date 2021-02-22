using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Requests
{
    public class SetzeZielFürNummerInformation
    {
        [DataMember]
        public Guid NummerInformationGuid { get; set; }

        [DataMember]
        public object Ziel { get; set; }

    }
}
