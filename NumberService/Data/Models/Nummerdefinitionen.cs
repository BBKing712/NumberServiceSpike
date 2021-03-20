using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Data.Models
{
    public partial class Nummerdefinitionen
    {
        public Nummerdefinitionen()
        {
            Nummerdefinitionquellen = new HashSet<Nummerdefinitionquellen>();
            Nummerinformationen = new HashSet<Nummerinformationen>();
        }

        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string Bezeichnung { get; set; }
        public string QuelleBezeichnung { get; set; }
        public long ZielDatentypenId { get; set; }
        public string ZielBezeichnung { get; set; }
        public DateTime? ErstelltAm { get; set; }
        public DateTime? AktualisiertAm { get; set; }

        public virtual Datentypen ZielDatentypen { get; set; }
        public virtual ICollection<Nummerdefinitionquellen> Nummerdefinitionquellen { get; set; }
        public virtual ICollection<Nummerinformationen> Nummerinformationen { get; set; }
    }
}
