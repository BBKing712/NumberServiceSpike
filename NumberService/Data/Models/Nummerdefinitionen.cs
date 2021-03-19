using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class Nummerdefinitionen
    {
        public Nummerdefinitionen()
        {
            Nummerdefinitionquellens = new HashSet<Nummerdefinitionquellen>();
            Nummerinformationens = new HashSet<Nummerinformationen>();
        }

        public long Id { get; set; }
        public Guid Guid { get; set; }
        public string Bezeichnung { get; set; }
        public string QuelleBezeichnung { get; set; }
        public long ZielDatentypenId { get; set; }
        public string ZielBezeichnung { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Datentypen ZielDatentypen { get; set; }
        public virtual ICollection<Nummerdefinitionquellen> Nummerdefinitionquellens { get; set; }
        public virtual ICollection<Nummerinformationen> Nummerinformationens { get; set; }
    }
}
