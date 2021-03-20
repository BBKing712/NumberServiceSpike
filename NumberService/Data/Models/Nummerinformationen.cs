using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Data.Models
{
    public partial class Nummerinformationen
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public long NummerdefinitionenId { get; set; }
        public string Quelle { get; set; }
        public string Ziel { get; set; }
        public DateTime? ErstelltAm { get; set; }
        public DateTime AktualisiertAm { get; set; }

        public virtual Nummerdefinitionen Nummerdefinitionen { get; set; }
    }
}
