using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Data.Models
{
    public partial class Nummerdefinitionquellen
    {
        public long Id { get; set; }
        public long NummerdefinitionenId { get; set; }
        public long Position { get; set; }
        public long DatentypenId { get; set; }
        public string Bezeichnung { get; set; }
        public DateTime? ErstelltAm { get; set; }
        public DateTime? AktualisiertAm { get; set; }

        public virtual Datentypen Datentypen { get; set; }
        public virtual Nummerdefinitionen Nummerdefinitionen { get; set; }
    }
}
