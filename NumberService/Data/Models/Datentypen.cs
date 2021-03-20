using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Data.Models
{
    public partial class Datentypen
    {
        public Datentypen()
        {
            Nummerdefinitionen = new HashSet<Nummerdefinitionen>();
            Nummerdefinitionquellen = new HashSet<Nummerdefinitionquellen>();
        }

        public long Id { get; set; }
        public string Bezeichnung { get; set; }
        public DateTime? ErstelltAm { get; set; }
        public DateTime? AktualisiertAm { get; set; }

        public virtual ICollection<Nummerdefinitionen> Nummerdefinitionen { get; set; }
        public virtual ICollection<Nummerdefinitionquellen> Nummerdefinitionquellen { get; set; }
    }
}
