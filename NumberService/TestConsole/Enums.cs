using System;
using System.Collections.Generic;
using System.Text;

namespace TestConsole
{

   public enum NumberDefinitionBezeichnung
    {
        None,
        DEUWOAuftragsnummerZuGEMASAuftragsnummer,
        High
    }
    public enum Datentyp : long
    {
        String = 1L,
        Integer = 2L,
        Guid = 3L
    }
}
