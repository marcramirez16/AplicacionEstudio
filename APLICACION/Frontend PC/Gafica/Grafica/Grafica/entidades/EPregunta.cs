using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafica.entidades
{
    public class EPregunta
    {
        public long id_pregunta { get; set; }
        public long id_resumen { get; set; }
        public long id_tema { get; set; }
        public long id_asignatura { get; set; }
        public long id_usuario { get; set; }
        public string pregunta { get; set; }
        public String Tipo { get; set; }

        public String Imagen { get; set; }
    }
}


