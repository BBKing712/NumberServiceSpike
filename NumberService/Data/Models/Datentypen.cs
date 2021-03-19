using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class Datentypen
    {
        public Datentypen()
        {
            Nummerdefinitionens = new HashSet<Nummerdefinitionen>();
            Nummerdefinitionquellens = new HashSet<Nummerdefinitionquellen>();
        }

        public long Id { get; set; }
        public string Bezeichnung { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Nummerdefinitionen> Nummerdefinitionens { get; set; }
        public virtual ICollection<Nummerdefinitionquellen> Nummerdefinitionquellens { get; set; }
    }
}
