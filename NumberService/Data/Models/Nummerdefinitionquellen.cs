using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class Nummerdefinitionquellen
    {
        public long Id { get; set; }
        public long NummerDefinitionenId { get; set; }
        public long Position { get; set; }
        public long DatentypenId { get; set; }
        public string Bezeichnung { get; set; }
        public DateTime? ErstelltAm { get; set; }
        public DateTime? AktualisiertAm { get; set; }

        public virtual Datentypen Datentypen { get; set; }
        public virtual Nummerdefinitionen NummerDefinitionen { get; set; }
    }
}
