using System;
using System.Collections.Generic;

#nullable disable

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
