using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRINAPIREST.Models
{
    public class LineasNotaPeso
    {
        public long rowNumber { get; set; }
        public string ticket { get; set; }
        public string codigoItem { get; set; }
        public string descripcionItem { get; set; }
        public long lineaNotaPeso { get; set; }
        public Int16 numeroPartida { get; set; }
        public string tipoCacao { get; set; }
        public decimal numeroSacos { get; set; }
        public decimal humedad { get; set; }
        public decimal buenFermento { get; set; }
        public decimal ligeroFermento { get; set; }
        public decimal pizarra { get; set; }
        public decimal violeta { get; set; }
        public decimal moho { get; set; }
        public decimal insecto { get; set; }
        public decimal peso100Pepas { get; set; }
        public decimal granosMultiples { get; set; }
        public decimal materialCacaoResiduos { get; set; }
        public decimal granoPlano { get; set; }
        public decimal materialextranio { get; set; }
        public decimal trituradoTamisado { get; set; }
        public decimal sedoHumedad { get; set; }
        public decimal sedoImpurezas { get; set; }
        public bool isExport { get; set; }
        public string definitive { get; set; }
        public decimal granoQuebrado { get; set; }
        public bool itchgrass { get; set; }
        public int accionSecado { get; set; }
        public int accionImpureza { get; set; }
    }
}
