using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensajeroModel
{
    public class Mensaje
    {
        private string remitente;
        private string cuerpoMensaje;
        private string tipo;

        public string Remitente { get => remitente; set => remitente = value; }
        public string CuerpoMensaje { get => cuerpoMensaje; set => cuerpoMensaje = value; }
        public string Tipo { get => tipo; set => tipo = value; }

        public override string ToString()
        {
            return "[" + tipo + "] " + remitente + " dice: " + cuerpoMensaje;
        }
    }
}
