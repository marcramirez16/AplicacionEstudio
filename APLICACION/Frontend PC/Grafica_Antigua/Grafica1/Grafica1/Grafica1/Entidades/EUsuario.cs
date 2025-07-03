using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EUsuario
{
    public long? idusuario { get; set; }  // puede ser null al enviar
    public string usuario { get; set; }
    public string contraseña { get; set; }
    public string email { get; set; }
}