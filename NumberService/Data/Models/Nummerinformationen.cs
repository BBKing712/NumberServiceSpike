﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Data.Models
{
    public partial class Nummerinformationen
    {
        public long Id { get; set; }
        public Guid Guid { get; set; }
        public long NummerdefinitionId { get; set; }
        public string Quelle { get; set; }
        public string Ziel { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Nummerdefinitionen Nummerdefinition { get; set; }
    }
}
