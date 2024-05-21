using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRINAPIREST.Models
{
    public class Evidencia
    {
        public long rowNumber { get; set; }
        public long lineaNotaPeso { get; set; }
        public Int16 numeroPartida { get; set; }
        public Int16 sectionPhoto { get; set; }
        public string file { get; set; }

    }
}
